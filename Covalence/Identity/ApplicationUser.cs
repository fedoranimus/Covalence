using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Covalence
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            Tags = new HashSet<UserTag>();
            NeedsOnboarding = true;
            //Location = new Location();
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        //private string location { get; set; }
        //[NotMapped]
        public Location Location { get; set; }
        public bool IsMentor { get; set; }
        public string Bio { get; set; }
        public ICollection<UserTag> Tags { get; set; }
        public bool NeedsOnboarding { get; set; }
    }
}
