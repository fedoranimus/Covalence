using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Covalence.Authentication;
using Microsoft.Extensions.Logging;

namespace Covalence
{
    public interface ITagService {
        IEnumerable<Tag> GetAllTags();
        IEnumerable<Tag> QueryTags(string query);
        Tag GetTagById(int id);
        Tag GetTagByName(string name);
        Task<ApplicationUser> AddUserToTag(Tag tag, TagType tagType, ApplicationUser user);

        HashSet<Tag> PopulateTags(ICollection<int> tagIds);
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

        public IEnumerable<Tag> GetAllTags() 
        {
            _logger.LogDebug("Getting all tags...");
            return _context.Tags.AsEnumerable();
        }

        public IEnumerable<Tag> QueryTags(string query)
        {
            _logger.LogDebug("Finding Tags which contain '{0}'", query);
            return _context.Tags.ToList().Where(t => t.Name.Contains(query));
        }

        public Tag GetTagById(int id)
        {
            _logger.LogDebug("Getting tag with id: {0}", id);
            return _context.Tags.FirstOrDefault(t => t.TagId == id);
        }

        public Tag GetTagByName(string name)
        {
            _logger.LogDebug("Getting with name: {0}", name);
            return _context.Tags.FirstOrDefault(t => t.Name.ToLowerInvariant() == name.ToLowerInvariant());
        }

        public async Task<ApplicationUser> AddUserToTag(Tag tag, TagType tagType, ApplicationUser user)
        {
            if(tagType == TagType.Study)
            {
                var studyUserTag = new StudyUserTag() { UserId = user.Id, User = user, TagId = tag.TagId, Tag = tag };
                tag.StudyUsers.Add(studyUserTag);
                user.StudyTags.Add(studyUserTag);
            }
            else if(tagType == TagType.Expert)
            {
                var expertUserTag = new ExpertUserTag() { UserId = user.Id, User = user, TagId = tag.TagId, Tag = tag };
                tag.ExpertUsers.Add(expertUserTag);
                user.ExpertTags.Add(expertUserTag);
            }

            await _context.SaveChangesAsync();

            return user;
        }

        public HashSet<Tag> PopulateTags(ICollection<int> tagIds)
        {
            HashSet<Tag> tags = new HashSet<Tag>();

            foreach(var tagId in tagIds)
            {
                tags.Add(GetTagById(tagId));
            }

            return tags;
        }
    }
}