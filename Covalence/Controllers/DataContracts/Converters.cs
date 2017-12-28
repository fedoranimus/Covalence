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

        public static ConnectionListContract ConvertConnectionListToContract(ICollection<Connection> connections, string userId) {

            return new ConnectionListContract() {
                PendingConnections = connections.Where(c => c.RequestedUserId == userId && c.State == ConnectionState.Pending).Select(connection => ConvertConnectionToContract(connection, false)).ToList(),
                RequestedConnections = connections.Where(c => c.RequestingUserId == userId && c.State == ConnectionState.Pending).Select(connection => ConvertConnectionToContract(connection, true)).ToList(),
                ActiveConnections = connections.Where(c => (c.RequestedUserId == userId || c.RequestingUserId == userId) && c.State == ConnectionState.Connected).Select(connection => ConvertConnectionToContract(connection, connection.RequestingUserId == userId)).ToList(),
            };  
        }

        public static ConnectionContract ConvertConnectionToContract(Connection connection, bool isRequestingUser) {
            return new ConnectionContract() {
                RequestingUserId = connection.RequestingUserId,
                RequestedUserId = connection.RequestedUserId,
                DisplayName = ConvertConnectionDisplayName(connection, isRequestingUser)
            };
        }

        private static string ConvertConnectionDisplayName(Connection connection, bool isRequestingUser) {
            var displayName = "";

            if(isRequestingUser) 
            {
                displayName = $"{connection.RequestedUser.FirstName} {connection.RequestedUser.LastName}";
            }
            else 
            {
                displayName = $"{connection.RequestingUser.FirstName} {connection.RequestingUser.LastName}";
            }

            return displayName;
        }

        public static List<RemoteUserContract> ConvertRemoteUserListToContract(List<ApplicationUser> users)
        {
            return users.Select(user => ConvertRemoteUserToContract(user)).ToList();
        }
    }
}