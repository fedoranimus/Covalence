using System.Collections.Generic;

namespace Covalence.Contracts
{
    public class ConnectionContract {
        public string RequestingUserId { get; set; }
        public string RequestedUserId { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public RemoteConnectionStatus ConnectionStatus { get; set; }
        public string Id { get; set; }
    }
}