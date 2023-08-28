using RedditReader.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedditReader.Logic
{
    public class StatsLogger : IStatsLogger
    {
        private readonly IPostsRepository _postsRepository;
        private readonly IStatsRepository _statsRepository;

        public StatsLogger(IPostsRepository postsRepository, IStatsRepository statsRepository)
        {
            _postsRepository = postsRepository;
            _statsRepository = statsRepository;
        }

        public async Task LogStats()
        {
            var posts = _postsRepository.GetAll();
            if (posts.Any())
            {
                var userWithMostPosts = _statsRepository.UserStats?.MaxBy(x => x.Value);
                Console.WriteLine($"==Dumping stats===");
                Console.WriteLine($"# Posts: {posts.Count()}");

                if (_statsRepository.PostWithMaxUpvotes != null)
                    Console.WriteLine($"# Post w/ most upvotes: {_statsRepository.PostWithMaxUpvotes.Title} ({_statsRepository.PostWithMaxUpvotes.Ups})");
                
                if (userWithMostPosts != null)
                    Console.WriteLine($"Author w/ most posts: {userWithMostPosts?.Key} ({userWithMostPosts?.Value})");
            }
            await Task.Delay(5000);
        }
    }
}
