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
                    .Select(ut => Converters.ConvertTagToContract(ut.Tag)).ToList(),
                AuthoredPosts = user.AuthoredPosts
                    .Select(post => Converters.ConvertPostToContract(post)).ToList()
            };
        }

        public static RemoteUserContract ConvertRemoteUserToContract(ApplicationUser user)
        {
            return new RemoteUserContract(){
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName
            };
        }

        public static PostContract ConvertPostToContract(Post post) 
        {

            return new PostContract()
            {
                PostId = post.PostId,
                Title = post.Title,
                Author = Converters.ConvertRemoteUserToContract(post.Author),
                Category = post.Category,
                Content = post.Content,
                DateModified = post.DateModified.ToString(),
                DateCreated = post.DateCreated.ToString(),
                Tags = post.Tags
                    .Select(ut => Converters.ConvertTagToContract(ut.Tag)).ToList()  
            };
        }
    }
}