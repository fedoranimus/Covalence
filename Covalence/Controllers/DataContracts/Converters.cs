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

        public static RemoteUserContract ConvertRemoteUserToContract(ApplicationUser currentUser, ApplicationUser remoteUser, ICollection<Connection> connections)
        {
            var connectionStatus = Converters.ConvertRemoteConnectionStatus(currentUser.Id, connections, remoteUser.Id);

            return new RemoteUserContract(){
                Id = remoteUser.Id,
                FirstName = remoteUser.FirstName,
                LastName = remoteUser.LastName,
                Bio = remoteUser.Bio,
                Tags = remoteUser.Tags
                    .Select(ut => Converters.ConvertTagToContract(ut.Tag)).ToList(),
                ConnectionStatus = connectionStatus,
                Email = connectionStatus == RemoteConnectionStatus.Connected ? remoteUser.Email : null
            };
        }

        public static RemoteConnectionStatus ConvertRemoteConnectionStatus(string currentUserId, ICollection<Connection> connections, string userId) {
            var status = RemoteConnectionStatus.Available;

            var existingConnection = connections.Where(c => (c.RequestedUserId == currentUserId && c.RequestingUserId == userId) || (c.RequestingUserId == currentUserId && c.RequestedUserId == userId)).FirstOrDefault();
            if(existingConnection != null)
            {
                if(existingConnection.State == ConnectionState.Connected)
                {
                    status = RemoteConnectionStatus.Connected;
                }
                else 
                {
                    if(existingConnection.RequestingUserId == currentUserId)
                    {
                        status = RemoteConnectionStatus.Requested;
                    }
                    else 
                    {
                        status = RemoteConnectionStatus.Pending;
                    }
                }
            }

            return status;
        }

        private static List<ConnectionContract> FindPendingConnections(ICollection<Connection> connections, string userId) {
            return connections.Where(c => c.RequestedUserId == userId && c.State == ConnectionState.Pending).Select(connection => ConvertConnectionToContract(connection, false)).ToList();
        }

        private static List<ConnectionContract> FindRequestedConnections(ICollection<Connection> connections, string userId) {
            return connections.Where(c => c.RequestingUserId == userId && c.State == ConnectionState.Pending).Select(connection => ConvertConnectionToContract(connection, true)).ToList();
        }

        private static List<ConnectionContract> FindActiveConnections(ICollection<Connection> connections, string userId) {
            return connections.Where(c => (c.RequestedUserId == userId || c.RequestingUserId == userId) && c.State == ConnectionState.Connected).Select(connection => ConvertConnectionToContract(connection, connection.RequestingUserId == userId)).ToList();
        }

        public static ConnectionListContract ConvertConnectionListToContract(ICollection<Connection> connections, string userId) {

            return new ConnectionListContract() {
                PendingConnections = FindPendingConnections(connections, userId),
                RequestedConnections = FindRequestedConnections(connections, userId),
                ActiveConnections = FindActiveConnections(connections, userId)
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

        public static List<RemoteUserContract> ConvertRemoteUserListToContract(ApplicationUser currentUser, List<ApplicationUser> users, ICollection<Connection> connections)
        {
            return users.Select(remoteUser => ConvertRemoteUserToContract(currentUser, remoteUser, connections)).ToList();
        }

        public static PagingContract<T> ConvertPagingListToContract<T>(PaginatedList<T> items)
        {
            return new PagingContract<T>() {
                Items = items.ToList(),
                PageNumber = items.PageIndex,
                TotalPages = items.TotalPages,
                HasPreviousPage = items.HasPreviousPage,
                HasNextPage = items.HasNextPage
            };
        }
    }
}