using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Covalence.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Covalence.ViewModels;
using System;
using System.Text.RegularExpressions;

namespace Covalence {
    public interface IPostService {
        Task<Post> CreatePost(ApplicationUser user, PostViewModel model);
        Task<List<Post>> GetAllPosts(int startIndex, int pageSize);
        Task<Post> GetPost(int postId);
        Task DeletePost(int postId);
        Task<Post> UpdatePost(int postId, PostViewModel model);
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

        public async Task<Post> CreatePost(ApplicationUser user, PostViewModel model) {
            var postTags = new HashSet<PostTag>();
            var category = (PostType)model.Category;

            var post = new Post(){
                Author = user,
                Content = model.Content,
                Title = model.Title,
                Tags = postTags,
                Category = category,
                DateCreated = DateTime.UtcNow,
                DateModified = DateTime.UtcNow
            };

            var newPost = await _context.AddAsync(post);
            await _context.SaveChangesAsync();

            var key = newPost.Entity.PostId;

            foreach (var tag in model.Tags)
            {
                var entityTag = await _tagService.GetTag(tag);

                var postTag = await _context.PostTags.Where(pt => pt.PostId == key).Where(pt => pt.Name == entityTag.Name).FirstOrDefaultAsync(); //Should probably always be null
                if (postTag == null)
                    postTag = new PostTag() { PostId = post.PostId, Post = post, Name = entityTag.Name, Tag = entityTag };

                postTags.Add(postTag);
            };

            post.Tags = postTags;

            _context.Update(post);

            await _context.SaveChangesAsync();

            return post;
        }

        public async Task<List<Post>> GetAllPosts(int startIndex, int pageSize) 
        {
            return await _context.Posts
                    .Include(p => p.Author)
                    .Include(p => p.Tags)
                        .ThenInclude(pt => pt.Tag)
                    .Skip(startIndex)
                    .Take(pageSize)
                    .ToListAsync();
        }

        //TODO: Implement Search
        public async Task<List<Post>> SearchPosts(string query, int startIndex, int pageSize)
        {
            //https://stackoverflow.com/questions/12730251/convert-result-of-matches-from-regex-into-list-of-string/21123574#21123574
            var matches = MatchQuery(query);
            var tagsMatch = matches.Groups["tags"];
            var contentMatch = matches.Groups["content"];
            var unorderedPosts = await _context.Posts
                                .Include(p => p.Content)
                                .Include(p => p.Tags)
                                    .ThenInclude(pt => pt.Tag)
                                .Skip(startIndex)
                                .Take(pageSize)
                                .ToListAsync();

            var posts = unorderedPosts.OrderByDescending(x => x.Tags.Select(t => t.Name)).ToList();

            return posts;
        }

        private Match MatchQuery(string query)
        {
            var pattern = @"";
            var regex = new Regex(pattern, RegexOptions.IgnoreCase);

            var match = regex.Match(query);

            return match;
        }

        public async Task<Post> GetPost(int postId)
        {
            return await _context.Posts
                    .Include(p => p.Author)
                    .Include(p => p.Tags)
                        .ThenInclude(pt => pt.Tag)
                    .FirstOrDefaultAsync(x => x.PostId == postId);
        }

        public async Task DeletePost(int postId)
        {
            var post = await _context.Posts.FirstOrDefaultAsync(x => x.PostId == postId);
            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
        }

        public async Task<Post> UpdatePost(int postId, PostViewModel model)
        {
            var post = await _context.Posts
                        .Include(p => p.Author)
                        .Include(p => p.Tags)
                            .ThenInclude(pt => pt.Tag)
                        .FirstOrDefaultAsync(x => x.PostId == postId);

            if(post != null) 
            {
                var category = (PostType)model.Category;
                var postTags = new HashSet<PostTag>();

                foreach (var tag in model.Tags)
                {
                    var entityTag = await _tagService.GetTag(tag);
                    var postTag = await _context.PostTags.Where(pt => pt.PostId == postId).Where(pt => pt.Name == entityTag.Name).FirstOrDefaultAsync();
                    if(postTag == null)
                        postTag = new PostTag() { PostId = post.PostId, Post = post, Name = entityTag.Name, Tag = entityTag };

                    postTags.Add(postTag);
                };

                post.Content = model.Content;
                post.Title = model.Title;
                post.Tags = postTags;
                post.Category = category;
                post.DateModified = DateTime.UtcNow;
                
                _context.Posts.Update(post);
                await _context.SaveChangesAsync();
                return post;
            }

            return post;
        }
    }
}