using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Covalence.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace Covalence
{
    public interface ITagService {
        Task<IEnumerable<Tag>> GetAllTags();
        Task<IEnumerable<Tag>> QueryTags(string query);
        Task<Tag> GetTag(string name);
        Task<ApplicationUser> AddTag(Tag tag, ApplicationUser user);
        Task<ApplicationUser> RemoveTag(Tag tag, ApplicationUser user);
        Task<Post> AddTag(Tag tag, Post post);
        Task<Post> RemoveTag(Tag tag, Post post);
        Task CreateTag(string name);
    }

    public class TagService : ITagService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<TagService> _logger;
        public TagService(ApplicationDbContext context, ILoggerFactory loggerFactory) 
        {
            _context = context;
            _logger = loggerFactory.CreateLogger<TagService>();
        }

        public async Task<IEnumerable<Tag>> GetAllTags() 
        {
            _logger.LogDebug("Getting all tags...");
            return await _context.Tags.ToListAsync();
        }

        public async Task<IEnumerable<Tag>> QueryTags(string query)
        {
            _logger.LogDebug("Finding Tags which contain '{0}'", query);
            return await _context.Tags.Where(t => t.Name.ToUpperInvariant().Contains(query.ToUpperInvariant())).ToListAsync();
        }

        public async Task<Tag> GetTag(string name)
        {
            _logger.LogDebug("Getting with name: {0}", name);
            return await _context.Tags.FirstOrDefaultAsync(t => t.Name.ToUpperInvariant() == name.ToUpperInvariant());
        }

        public async Task CreateTag(string tagName)
        {
            _logger.LogDebug($"Creating tag {tagName}");
            var tag = new Tag(){ Name = tagName };
            await _context.Tags.AddAsync(tag);

            await _context.SaveChangesAsync();
        }

        public async Task<ApplicationUser> AddTag(Tag tag, ApplicationUser user)
        {
            user = await _context.Users.Include(x => x.Tags).ThenInclude(ut => ut.Tag).FirstOrDefaultAsync();
            if(user.Tags.Select(ut => ut.Tag).Contains(tag))
            {
                _logger.LogInformation("{0} already assigned to {1}", tag.ToString(), user.ToString());
            }
            else
            {
                _logger.LogInformation("{0} added to {1}", tag.ToString(), user.ToString());
                var userTag = new UserTag() { UserId = user.Id, User = user, Name = tag.Name, Tag = tag };
                tag.Users.Add(userTag);
                user.Tags.Add(userTag);
            }

            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<ApplicationUser> RemoveTag(Tag tag, ApplicationUser user) 
        {
            user = await _context.Users.Include(x => x.Tags).ThenInclude(ut => ut.Tag).FirstOrDefaultAsync();
            if(user.Tags.Select(ut => ut.Tag).Contains(tag))
            {
                var userTag = user.Tags.Where(x => x.Name == tag.Name).FirstOrDefault();
                _logger.LogInformation($"Removing {tag.ToString()} from {user.ToString()}");
                tag.Users.Remove(userTag);
                user.Tags.Remove(userTag);
            }
            else 
            {
                _logger.LogInformation($"{tag.ToString()} does not exist on {user.ToString()}");
            }

            await _context.SaveChangesAsync();
            
            return user;
        }

        public async Task<Post> AddTag(Tag tag, Post post) 
        {
            post = await _context.Posts.Include(x => x.Tags).ThenInclude(ut => ut.Tag).FirstOrDefaultAsync();
            if(post.Tags.Select(ut => ut.Tag).Contains(tag))
            {
                _logger.LogInformation($"{tag.ToString()} already exists on {post.ToString()}");
            }
            else
            {
                //TODO   
            }
            return post;
        }

        public async Task<Post> RemoveTag(Tag tag, Post post)
        {
            post = await _context.Posts.Include(x => x.Tags).ThenInclude(ut => ut.Tag).FirstOrDefaultAsync();
            if(post.Tags.Select(ut => ut.Tag).Contains(tag))
            {
                //TODO
            }
            else
            {
                _logger.LogInformation($"{tag.ToString()} does not exist on {post.ToString()}");
            }
            return post;
        }
    }
}