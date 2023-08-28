using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedditReader.Data.RedditDtos
{
    public class ListingDto
    {
        public required string Kind { get; set; }
        public required ListingDataDto Data { get; set; }
    }
}
