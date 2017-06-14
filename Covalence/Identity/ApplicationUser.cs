using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Covalence.Authentication
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            Tags = new HashSet<UserTag>();
            AuthoredPosts = new List<Post>();
            //Connections = new List<Connection>();
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Location { get; set; }
        public ICollection<UserTag> Tags { get; set; }
        public ICollection<Post> AuthoredPosts { get; set; }

        //public ICollection<Connection> Connections { get; set; }
    }
}
