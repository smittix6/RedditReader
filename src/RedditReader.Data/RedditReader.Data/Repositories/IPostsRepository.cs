using RedditReader.Data.RedditDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace RedditReader.Data.Repositories
{
    public interface IPostsRepository
    {
        public Task CreateOrUpdate(PostDataDto post);
        public IEnumerable<PostDataDto> GetAll();

        public event EventHandler<OnCreateOrUpdatePostArgs> OnAfterCreateOrUpdate;
    }
}
