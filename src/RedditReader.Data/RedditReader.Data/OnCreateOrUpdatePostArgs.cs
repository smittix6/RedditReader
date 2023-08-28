using RedditReader.Data.Enums;
using RedditReader.Data.RedditDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace RedditReader.Data
{
    public class OnCreateOrUpdatePostArgs
    {
        public required PostDataDto PostData { get; set; }
        public required RepositoryEventType EventType { get; set; }
    }
}
