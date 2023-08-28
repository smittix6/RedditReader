using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedditReader.Data.RedditDtos
{
    public class ListingDataDto
    {
        public string? After { get; set; }
        public int Dist { get; set; }
        public required IEnumerable<PostDto> Children { get; set; }
    }
}
