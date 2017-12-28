using System.Collections.Generic;

namespace Covalence.Contracts
{
    public class ConnectionContract {
        public string RequestingUserId { get; set; }
        public string RequestedUserId { get; set; }
        public string RequestingFirstName { get; set; }
        public string RequestingLastName { get; set; }
        public string RequestedFirstName { get; set; }
        public string RequestedLastName { get; set; }
        public ConnectionState State { get; set; }
    }
}