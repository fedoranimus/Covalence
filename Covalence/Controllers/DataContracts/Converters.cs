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
                EmailConfirmed = user.EmailConfirmed,
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

        public static List<ConnectionContract> ConvertConnectionListToContract(ICollection<Connection> connections) {
            return connections.Select(connection => ConvertConnectionToContract(connection)).ToList();     
        }

        public static ConnectionContract ConvertConnectionToContract(Connection connection) {
            return new ConnectionContract() {
                RequestingUserId = connection.RequestingUserId,
                RequestedUserId = connection.RequestedUserId,
                RequestedFirstName = connection.RequestedUser.FirstName,
                RequestedLastName = connection.RequestedUser.LastName,
                RequestingFirstName = connection.RequestingUser.FirstName,
                RequestingLastName = connection.RequestingUser.LastName,
                State = connection.State
            };
        }

        public static List<RemoteUserContract> ConvertRemoteUserListToContract(List<ApplicationUser> users)
        {
            return users.Select(user => ConvertRemoteUserToContract(user)).ToList();
        }
    }
}