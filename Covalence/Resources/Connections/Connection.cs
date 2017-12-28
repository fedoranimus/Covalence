namespace Covalence
{

    public enum ConnectionState
    {
        Pending,
        Connected,
        Blocked
    }
    public class Connection
    {
        public Connection()
        {
            State = ConnectionState.Pending;
        }
        public string RequestingUserId { get; set; }
        public ApplicationUser RequestingUser { get; set; }
        public string RequestedUserId { get; set; }
        public ApplicationUser RequestedUser { get; set; }
        public ConnectionState State { get; set; }
    }
}