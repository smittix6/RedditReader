using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedditReader.Data.RedditDtos
{
    public class ApiResponseDto<T>
    {
        /// <summary>
        /// Approximate number of requests left to use
        /// </summary>
        public int RateLimitRemaining { get; set; }
        /// <summary>
        /// Approximate number of requests used in this period
        /// </summary>
        public int RateLimitUsed { get; set; }
        /// <summary>
        /// Approximate number of seconds to end of period
        /// </summary>
        public int RateLimitReset { get; set; }
        /// <summary>
        /// Timestamp for the response
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// JSON data returned from the request
        /// </summary>
        public T? Data { get; set; }
    }
}
