using System.Collections.Generic;

namespace Covalence.Contracts
{

    public enum RemoteConnectionStatus {
        Requested,
        Pending,
        Connected,
        Blocked,
        Available
    }
    public class RemoteUserContract
    {
        public RemoteUserContract()
        {
            Tags = new List<TagContract>();
        }

        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Bio { get; set; }
        public ICollection<TagContract> Tags { get; set; }
        public RemoteConnectionStatus ConnectionStatus { get; set; }
        public double? DistanceToUser { get; set; }
        public string Email { get; set; }
    }
}