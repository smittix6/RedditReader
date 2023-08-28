using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedditReader.Data.RedditDtos
{
    public class PostDataDto
    {
        public required string Subreddit { get; set; }
        public required string Id { get; set; }
        public int Ups { get; set; }
        public string? Author { get; set; }
        public string? Author_Fullname { get; set; }
        public string? Title { get; set; }
        public DateTime SyncedAt { get; set; }
    }
}
