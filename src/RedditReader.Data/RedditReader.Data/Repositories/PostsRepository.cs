using RedditReader.Data.RedditDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RedditReader.Data.Repositories
{
    public class PostsRepository : IPostsRepository
    {
        private IList<PostDataDto> _posts;
        private readonly SemaphoreSlim _semaphore;
        public event EventHandler<OnCreateOrUpdatePostArgs>? OnAfterCreateOrUpdate;

        public PostsRepository()
        {
            _posts = new List<PostDataDto>();
            _semaphore = new SemaphoreSlim(1, 1);
        }

        public async Task CreateOrUpdate(PostDataDto post)
        {
            try
            {
                await _semaphore.WaitAsync();
                var eventArgs = new OnCreateOrUpdatePostArgs()
                {
                    PostData = post,
                    EventType = Enums.RepositoryEventType.None
                };
                var existingPost = _posts.Where(x => x.Id == post.Id).FirstOrDefault();
                if (existingPost == null)
                {
                    _posts.Add(post);
                    eventArgs.EventType = Enums.RepositoryEventType.Created;
                }
                else if (existingPost.SyncedAt < post.SyncedAt)
                {
                    existingPost.Author = post.Author;
                    existingPost.Author_Fullname = post.Author_Fullname;
                    existingPost.Ups = post.Ups;
                    existingPost.SyncedAt = post.SyncedAt;
                    eventArgs.EventType = Enums.RepositoryEventType.Updated;
                }

                OnAfterCreateOrUpdate?.Invoke(this, eventArgs);
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public IEnumerable<PostDataDto> GetAll()
        {
            return _posts.AsReadOnly();
        }
    }
}
