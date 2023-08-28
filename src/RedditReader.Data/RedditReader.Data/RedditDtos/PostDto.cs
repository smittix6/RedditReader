using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedditReader.Data.RedditDtos
{
    public class PostDto
    {
        public required string Kind { get; set; }
        public required PostDataDto Data { get; set; }
    }
}
