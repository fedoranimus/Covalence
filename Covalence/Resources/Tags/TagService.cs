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
        Task<Tag> CreateTag(string name);

        Task<ApplicationUser> RemoveTags(List<Tag> tags, ApplicationUser user);

        Task<ApplicationUser> AddTags(List<Tag> tags, ApplicationUser user);

        Task<ApplicationUser> RemoveTags(List<string> tags, ApplicationUser user);

        Task<ApplicationUser> AddTags(List<string> tags, ApplicationUser user);
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
            _logger.LogDebug($"Finding Tags which contain '{query}'");
            return await _context.Tags.Where(t => t.Name.ToUpperInvariant().Contains(query.ToUpperInvariant())).ToListAsync();
        }

        public async Task<Tag> GetTag(string name)
        {
            _logger.LogDebug($"Getting with name: {name}");
            var tag = await _context.Tags.FirstOrDefaultAsync(t => t.Name.ToUpperInvariant() == name.ToUpperInvariant());
            if(tag == null)
            {
                _logger.LogDebug($"Tag {name} does not exist, creating new tag");
                tag = await CreateTag(name);
            }
            return tag;
        }

        public async Task<Tag> CreateTag(string tagName)
        {
            _logger.LogDebug($"Creating tag {tagName}");
            var tag = new Tag(){ Name = tagName };
            await _context.Tags.AddAsync(tag);

            await _context.SaveChangesAsync();

            return tag;
        }

        public async Task<ApplicationUser> RemoveTags(List<string> tags, ApplicationUser user) {
            foreach(var stringTag in tags) {
                var tag = await GetTag(stringTag);
                user = await RemoveTag(tag, user);
            }

            return user;
        }

        public async Task<ApplicationUser> RemoveTags(List<Tag> tags, ApplicationUser user)
        {
            foreach(var tag in tags) {
                user = await RemoveTag(tag, user);
            }

            return user;
        }

        public async Task<ApplicationUser> AddTags(List<string> tags, ApplicationUser user)
        {
            foreach(var stringTag in tags) {
                var tag = await GetTag(stringTag);
                user = await AddTag(tag, user);
            }

            return user;
        }

        public async Task<ApplicationUser> AddTags(List<Tag> tags, ApplicationUser user)
        {
            foreach(var tag in tags) {
                user = await AddTag(tag, user);
            }

            return user;
        }

        public async Task<ApplicationUser> AddTag(Tag tag, ApplicationUser user)
        {
            user = await _context.Users.Where(u => u.Id == user.Id).Include(x => x.Tags).ThenInclude(ut => ut.Tag).FirstOrDefaultAsync();
            if(user.Tags.Select(ut => ut.Tag).Contains(tag))
            {
                _logger.LogInformation($"{tag.ToString()} already assigned to {user.ToString()}");
            }
            else
            {
                _logger.LogInformation($"{tag.ToString()} added to {user.ToString()}");
                var userTag = new UserTag() { UserId = user.Id, User = user, Name = tag.Name, Tag = tag };
                tag.Users.Add(userTag);
                user.Tags.Add(userTag);
            }

            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<ApplicationUser> RemoveTag(Tag tag, ApplicationUser user) 
        {
            user = await _context.Users.Where(u => u.Id == user.Id).Include(x => x.Tags).ThenInclude(ut => ut.Tag).FirstOrDefaultAsync();
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
    }
}