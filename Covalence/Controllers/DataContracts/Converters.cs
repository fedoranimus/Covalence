using System.Collections.Generic;
using System.Linq;

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
                Bio = user.Bio,
                Tags = user.Tags
                    .Select(ut => Converters.ConvertTagToContract(ut.Tag)).ToList(),
                NeedsOnboarding = user.NeedsOnboarding
            };
        }

        public static RemoteUserContract ConvertRemoteUserToContract(ApplicationUser user)
        {
            return new RemoteUserContract(){
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Bio = user.Bio,
                Tags = user.Tags
                    .Select(ut => Converters.ConvertTagToContract(ut.Tag)).ToList()
            };
        }

        public static List<RemoteUserContract> ConvertRemoteUserListToContract(List<ApplicationUser> users)
        {
            return users.Select(user => ConvertRemoteUserToContract(user)).ToList();
        }
    }
}