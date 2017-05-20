using Covalence.Authentication;

namespace Covalence.API
{

    public enum ConnectionState
    {
        Requested,
        Approved,
        Blocked
    }
    public class Connection
    {
        public Connection()
        {
            State = ConnectionState.Requested;
        }
        public int ConnectionId { get; set; }
        public ApplicationUser RequestingUser { get; set; }
        public ApplicationUser RequestedUser { get; set; }
        public ConnectionState State { get; set; }
    }
}