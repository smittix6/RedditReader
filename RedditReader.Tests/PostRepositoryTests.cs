using RedditReader.Data.Repositories;
using System.Runtime.CompilerServices;

namespace RedditReader.Tests
{
    public class PostRepositoryTests
    {
        [Fact]
        public async Task  CreatePost()
        {
            var postRepository = new PostsRepository();
            await postRepository.CreateOrUpdate(new Data.RedditDtos.PostDataDto()
            {
                Id = "xxx",
                Author = "xxx",
                Author_Fullname = "xxx",
                Subreddit = "xxx",
                SyncedAt = DateTime.Now,
                Title = "Title",
                Ups = 7
            });
            var posts = postRepository.GetAll();
            Assert.Single(posts);
        }

        [Fact]
        public async Task UpdateDoesNotOverrideNewerData()
        {
            var latestTimestamp = DateTime.Now;
            var post1 = new Data.RedditDtos.PostDataDto()
            {
                Id = "xxx",
                Author = "xxx",
                Author_Fullname = "xxx",
                Subreddit = "xxx",
                SyncedAt = latestTimestamp,
                Title = "Title",
                Ups = 7
            };

            var post2 = new Data.RedditDtos.PostDataDto()
            {
                Id = "xxx",
                Author = "xxx",
                Author_Fullname = "xxx",
                Subreddit = "xxx",
                SyncedAt = DateTime.Now.AddMinutes(-1),
                Title = "Older Title",
                Ups = 1
            };
            var postRepository = new PostsRepository();
            await postRepository.CreateOrUpdate(post1);
            await postRepository.CreateOrUpdate(post2);
            var postInRepo = postRepository.GetAll().Where(x => x.Id == "xxx").First();
            Assert.Equal(latestTimestamp, postInRepo.SyncedAt);
        }
    }
}