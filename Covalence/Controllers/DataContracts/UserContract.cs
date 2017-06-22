using System.Collections.Generic;

namespace Covalence.Contracts
{
    public class UserContract
    {
        public UserContract()
        {
            Tags = new List<TagContract>();
            AuthoredPosts = new List<PostContract>();
        }

        public string Email { get; set; }
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Location { get; set;}
        public ICollection<TagContract> Tags { get; set; }
        public ICollection<PostContract> AuthoredPosts { get; set; }
        //public List<Connection> Connections { get; set; }
    }
}