using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedditReader.Logic
{
    public interface IStatsLogger
    {
        Task LogStats();
    }
}
