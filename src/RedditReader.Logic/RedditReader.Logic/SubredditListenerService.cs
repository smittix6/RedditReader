using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RedditReader.Data;
using RedditReader.Data.RedditDtos;
using RedditReader.Data.Repositories;
using RedditReader.Data.RestClients;

namespace RedditReader.Logic
{
    public class SubredditListenerService : IHostedService
    {
        private readonly ILogger _logger;
        private readonly IRedditRestClient _redditRestClient;
        private readonly IPostsRepository _postsRepository;
        private readonly string _subredditName = "gaming";
        private readonly IStatsRepository _statsRepository;
        private readonly IStatsLogger _statsLogger;

        public SubredditListenerService(
            ILogger<SubredditListenerService> logger,
            IRedditRestClient redditRestClient,
            IPostsRepository postsRepository,
            IStatsRepository statsRepository,
            IStatsLogger statsLogger)
        {
            _logger = logger;
            _redditRestClient = redditRestClient;
            _postsRepository = postsRepository;
            _postsRepository.OnAfterCreateOrUpdate += OnAfterPostCreatedOrUpdated;
            _statsRepository = statsRepository;
            _statsLogger = statsLogger;
        }

        // Use this observer method to watch for changes to the Posts repo
        private void OnAfterPostCreatedOrUpdated(object? sender, OnCreateOrUpdatePostArgs args)
        {
            // If a post was just added or updated, check is this is the highest voted
            var posts = _postsRepository.GetAll();
            if (!posts.Where(x => x.Ups > args.PostData.Ups).Any())
            {
                _statsRepository.PostWithMaxUpvotes = args.PostData;
            }

            // The call to OnAfterPostCreatedOrUpdated is thread-safe so we don't have to worry about corrupt data here
            if (args.EventType == Data.Enums.RepositoryEventType.Created)
            {
                if (args.PostData.Author_Fullname != null)
                {
                    if (!_statsRepository.UserStats.ContainsKey(args.PostData.Author_Fullname))
                        _statsRepository.UserStats[args.PostData.Author_Fullname] = 0;

                    _statsRepository.UserStats[args.PostData.Author_Fullname]++;
                }
            }
        }

        // Use up all API calls available in this RateLimit period spaced out evenly
        public async Task StartPostSyncForRatePeriod()
        {
            var apiResponse = await _redditRestClient.GetPostsForSubreddit(_subredditName);
            var timeToNextRequest = apiResponse.RateLimitReset / apiResponse.RateLimitRemaining;
            var tasks = new List<Task>();
            for (var i = 0; i < apiResponse.RateLimitRemaining; i++)
            {
                tasks.Add(GetSubredditPosts());
                await Task.Delay(timeToNextRequest * 1000);
            }
            await Task.WhenAll(tasks.ToArray());
        }

        // Get latest 100 posts in subreddit and add them to the repo
        public async Task GetSubredditPosts()
        {
            var apiResponse = await _redditRestClient.GetPostsForSubreddit(_subredditName);
            if (apiResponse.Data != null)
            {
                foreach (var post in apiResponse.Data)
                {
                    await _postsRepository.CreateOrUpdate(post);
                }
            }
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Listening..");

            var syncingTask = StartPostSyncForRatePeriod();
            var loggingTask = _statsLogger.LogStats();

            // Enter the main loop
            while (true)
            {
                if (syncingTask.IsCompleted)
                    syncingTask = StartPostSyncForRatePeriod();

                if (loggingTask.IsCompleted)
                    loggingTask = _statsLogger.LogStats();
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
        }
    }
}