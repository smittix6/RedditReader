using RedditReader.Data.RedditDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace RedditReader.Data.RestClients
{
    public class RedditRestClient : IRedditRestClient
    {
        private readonly HttpClient _httpClient;
        private readonly IRedditAuthClient _redditAuthClient;

        public RedditRestClient(HttpClient httpClient, IRedditAuthClient redditAuthClient)
        {
            _httpClient = httpClient;
            _redditAuthClient = redditAuthClient;
        }

        public async Task<ApiResponseDto<IEnumerable<PostDataDto>>> GetPostsForSubreddit(string subredditName)
        {
            var authToken = await _redditAuthClient.GetAuthToken();
            var responseDto = new ApiResponseDto<IEnumerable<PostDataDto>>();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, $"https://oauth.reddit.com/r/{subredditName}/new.json?limit=100");
            request.Headers.Add("Authorization", $"Bearer {authToken}"); 
            request.Headers.Add("User-Agent", "christestscript/0.1 by smittix6");
            HttpResponseMessage response = await _httpClient.SendAsync(request);
            if (response.Headers.Contains("x-ratelimit-remaining"))
            {
                responseDto.RateLimitRemaining = (int)float.Parse(response.Headers.GetValues("x-ratelimit-remaining").First());
            }
            if (response.Headers.Contains("x-ratelimit-used"))
            {
                responseDto.RateLimitUsed = int.Parse(response.Headers.GetValues("x-ratelimit-used").First());
            }
            if (response.Headers.Contains("x-ratelimit-reset"))
            {
                responseDto.RateLimitReset = int.Parse(response.Headers.GetValues("x-ratelimit-reset").First());
            }
            var data = await response.EnsureSuccessStatusCode().Content.ReadFromJsonAsync<ListingDto>();
            responseDto.Data = data?.Data.Children.Select(x => x.Data).ToList();
            return responseDto;
        }
    }
}
