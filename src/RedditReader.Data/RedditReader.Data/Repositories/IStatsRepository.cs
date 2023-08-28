﻿using RedditReader.Data.RedditDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedditReader.Data.Repositories
{
    public interface IStatsRepository
    {
        public Dictionary<string, int> UserStats { get; set; }
        public PostDataDto? PostWithMaxUpvotes { get; set; }
    }
}
