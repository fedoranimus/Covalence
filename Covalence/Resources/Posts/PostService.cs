using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Covalence.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Covalence.ViewModels;
using System;

namespace Covalence {
    public interface IPostService {
        Task CreatePost(ApplicationUser user, PostViewModel model);
        Task<List<Post>> GetAllPosts();
        Task<Post> GetPost(int postId);
    }

    public class PostService : IPostService {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<TagService> _logger;
        private readonly ITagService _tagService;
        public PostService(ITagService tagService, ApplicationDbContext context, ILoggerFactory loggerFactory) 
        {
            _tagService = tagService;
            _context = context;
            _logger = loggerFactory.CreateLogger<TagService>();
        }

        public async Task CreatePost(ApplicationUser user, PostViewModel model) {
            var postTags = new HashSet<PostTag>();
            var category = (PostType)model.Category;

            var post = new Post(){
                Author = user,
                Content = model.Content,
                Title = model.Title,
                Tags = postTags,
                Category = category
            };

            var newPost = await _context.AddAsync(post);
            await _context.SaveChangesAsync();

            var key = newPost.Entity.PostId;

            foreach (var tag in model.Tags)
            {
                var entityTag = await _tagService.GetTag(tag);
                var postTag = new PostTag() { PostId = key, Post = post, Name = entityTag.Name, Tag = entityTag };
                postTags.Add(postTag);
            };

            post.Tags = postTags;

            _context.Update(post);

            await _context.SaveChangesAsync();
        }

        public async Task<List<Post>> GetAllPosts() 
        {
            return await _context.Posts.ToListAsync();
        }

        public async Task<Post> GetPost(int postId)
        {
            return await _context.Posts.FirstOrDefaultAsync(x => x.PostId == postId);
        }
    }
}