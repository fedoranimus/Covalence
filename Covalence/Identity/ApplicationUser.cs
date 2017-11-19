using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Covalence.Authentication
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            Tags = new HashSet<UserTag>();
            //Connections = new List<Connection>();
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Location { get; set; }
        public bool IsMentor { get; set; }
        public ICollection<UserTag> Tags { get; set; }

        //public ICollection<Connection> Connections { get; set; }
    }
}
