using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedditReader.Data.RestClients
{
    public interface IRedditAuthClient
    {
        public Task<string> GetAuthToken();
    }
}
