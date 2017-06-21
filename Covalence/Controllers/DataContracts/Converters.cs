using System.Linq;
using Covalence.Authentication;

namespace Covalence.Contracts
{
    public static class Converters
    {
        public static TagContract ConvertTagToContract(Tag tag)
        {
            return new TagContract() {
                Name = tag.Name.ToLowerInvariant(),
                UserCount = tag.Users.Count
            };
        }

        public static UserContract ConvertUserToContract(ApplicationUser user)
        {
            return new UserContract()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Location = user.Location,
                Email = user.Email,
                Tags = user.Tags
                    .Select(ut => Converters.ConvertTagToContract(ut.Tag))//,
                //AuthoredPosts = populatedUser.AuthoredPosts //TODO: Return list of authored posts
            };
        }

        public static PostContract ConvertPostToContract(Post post) 
        {
            var remoteUserContract = new RemoteUserContract(){
                Id = post.Author.Id,
                FirstName = post.Author.FirstName,
                LastName = post.Author.LastName
            };

            return new PostContract()
            {
                PostId = post.PostId,
                Title = post.Title,
                Author = remoteUserContract,
                Category = post.Category,
                Content = post.Content,
                DateModified = post.DateModified.ToString(),
                DateCreated = post.DateCreated.ToString(),
                Tags = post.Tags
                    .Select(ut => Converters.ConvertTagToContract(ut.Tag))  
            };
        }
    }
}