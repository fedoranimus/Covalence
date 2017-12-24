using System.Collections.Generic;

namespace Covalence.Contracts
{
    public class UserContract
    {
        public UserContract()
        {
            Tags = new List<TagContract>();
        }

        public string Email { get; set; }
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Bio { get; set; }
        public string Location { get; set;}
        public bool IsMentor { get; set; }
        public ICollection<TagContract> Tags { get; set; }
        public bool NeedsOnboarding { get; set; }
        //public List<Connection> Connections { get; set; }
    }
}