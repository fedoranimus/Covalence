using System.Collections.Generic;

namespace Covalence.Contracts
{
    public class ConnectionContract {
        public string RequestingUserId { get; set; }
        public string RequestedUserId { get; set; }
        public string DisplayName { get; set; }
    }

    public class ConnectionListContract {
        public List<ConnectionContract> RequestedConnections { get; set; }
        public List<ConnectionContract> ActiveConnections { get; set; }
        public List<ConnectionContract> PendingConnections { get; set; }
    }
}