using System.Collections.Generic;

namespace Covalence.Contracts
{
    public class RemoteUserContract
    {
        public RemoteUserContract()
        {

        }

        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; } 
        public List<Tag> Tags { get; set; }
    }
}