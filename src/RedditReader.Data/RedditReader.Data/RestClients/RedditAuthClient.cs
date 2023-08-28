using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Extensions.Configuration;
using System.Reflection.PortableExecutable;
using System.Net.Http;
using System.Net.Http.Json;
using System.Xml.Linq;

namespace RedditReader.Data.RestClients
{
    public class RedditAuthClient : IRedditAuthClient
    {
        private readonly HttpClient _httpClient;
        private RedditAuthToken? _cachedToken;
        private readonly SemaphoreSlim _semaphore;
        private readonly string _redditUsername;
        private readonly string _redditPassword;
        private readonly string _redditClientId;
        private readonly string _redditClientSecret;

        public RedditAuthClient(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _semaphore = new SemaphoreSlim(1, 1);
            _redditUsername = configuration["RedditConfig:username"] ?? "";
            _redditPassword = configuration["RedditConfig:password"] ?? "";
            _redditClientId = configuration["RedditConfig:clientId"] ?? "";
            _redditClientSecret = configuration["RedditConfig:clientSecret"] ?? "";
        }

        public async Task<string> GetAuthToken()
        {
            if (_cachedToken?.IsExpired() ?? true)
            {
                try
                {
                    await _semaphore.WaitAsync();
                    if (_cachedToken?.IsExpired() ?? true)
                    {
                        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "https://www.reddit.com/api/v1/access_token");
                        request.Headers.Authorization = new BasicAuthenticationHeaderValue(_redditClientId, _redditClientSecret);
                        request.Headers.Add("User-Agent", "christestscript/0.1 by smittix6");
                        request.Content = new FormUrlEncodedContent(new[]
                        {
                            new KeyValuePair<string, string>("grant_type", "password"),
                            new KeyValuePair<string, string>("username", _redditUsername),
                            new KeyValuePair<string, string>("password", _redditPassword)
                        });
                        HttpResponseMessage response = await _httpClient.SendAsync(request);
                        if (response.IsSuccessStatusCode)
                        {
                            var authTokenDto = await response.Content.ReadFromJsonAsync<RedditAuthTokenDto>();
                            if (authTokenDto != null)
                            {
                                _cachedToken = new RedditAuthToken()
                                {
                                    Token = authTokenDto.Access_Token,
                                    Expires = DateTime.UtcNow.AddSeconds(authTokenDto.ExpiresIn)
                                };
                            }
                        }
                    }
                }
                finally
                {
                    _semaphore.Release();
                }
            }

            return _cachedToken?.Token ?? string.Empty;
        }
    }

    public class RedditAuthToken
    {
        public string Token { get; set; }
        public DateTime Expires { get; set; }

        public bool IsExpired()
        {
            return DateTime.UtcNow >= Expires;
        }
    }

    public class RedditAuthTokenDto
    {
        public required string Access_Token { get; set; }
        public int ExpiresIn { get; set; }
    }
}
