using System.Collections.Generic;

namespace Covalence.Contracts
{
    public class RemoteUserContract
    {
        public RemoteUserContract()
        {
            Tags = new List<TagContract>();
        }

        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; } 
        public ICollection<TagContract> Tags { get; set; }
    }
}