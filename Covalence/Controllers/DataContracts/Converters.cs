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
                ZipCode = user.ZipCode,
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
                DistanceToUser = currentUser.Location.GetDistanceTo(remoteUser.Location),
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

        public static RemoteConnectionStatus ConvertRemoteConnectionStatus(string currentUserId, Connection connection) {
            var status = RemoteConnectionStatus.Available;

            if(connection.State == ConnectionState.Connected)
                {
                    status = RemoteConnectionStatus.Connected;
                }
                else 
                {
                    if(connection.RequestingUserId == currentUserId)
                    {
                        status = RemoteConnectionStatus.Requested;
                    }
                    else 
                    {
                        status = RemoteConnectionStatus.Pending;
                    }
                }

            return status;
        }

        // private static List<ConnectionContract> FindPendingConnections(ICollection<Connection> connections, string userId) {
        //     return connections.Where(c => c.RequestedUserId == userId && c.State == ConnectionState.Pending).Select(connection => ConvertConnectionToContract(connection, userId, false)).ToList();
        // }

        // private static List<ConnectionContract> FindRequestedConnections(ICollection<Connection> connections, string userId) {
        //     return connections.Where(c => c.RequestingUserId == userId && c.State == ConnectionState.Pending).Select(connection => ConvertConnectionToContract(connection, userId, true)).ToList();
        // }

        // private static List<ConnectionContract> FindActiveConnections(ICollection<Connection> connections, string userId) {
        //     return connections.Where(c => (c.RequestedUserId == userId || c.RequestingUserId == userId) && c.State == ConnectionState.Connected).Select(connection => ConvertConnectionToContract(connection, userId, connection.RequestingUserId == userId)).ToList();
        // }

        public static List<ConnectionContract> ConvertConnectionListToContract(ICollection<Connection> connections, string userId) {
            var connectionList = connections.Select(connection => ConvertConnectionToContract(connection, userId)).ToList();
    
            return connectionList;
        }

        public static ConnectionContract ConvertConnectionToContract(Connection connection, string userId) {
            var isRequestingUser = connection.RequestingUserId == userId;
            var emailAddress = isRequestingUser ? connection.RequestedUser.Email : connection.RequestingUser.Email;
            
            return new ConnectionContract() {
                RequestingUserId = connection.RequestingUserId,
                RequestedUserId = connection.RequestedUserId,
                DisplayName = ConvertConnectionDisplayName(connection, isRequestingUser),
                Email = connection.State == ConnectionState.Connected ? emailAddress : null,
                ConnectionStatus = ConvertRemoteConnectionStatus(userId, connection)
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