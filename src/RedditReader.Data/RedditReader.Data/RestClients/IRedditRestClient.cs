using RedditReader.Data.RedditDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedditReader.Data.RestClients
{
    public interface IRedditRestClient
    {
        public Task<ApiResponseDto<IEnumerable<PostDataDto>>> GetPostsForSubreddit(string subredditName);
    }
}
