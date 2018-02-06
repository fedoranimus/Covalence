using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Covalence
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            Tags = new HashSet<UserTag>();
            NeedsOnboarding = true;
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Location Location { get; set; }
        public string ZipCode { get; set; }
        public bool IsMentor { get; set; }
        public string Bio { get; set; }
        public ICollection<UserTag> Tags { get; set; }
        public bool NeedsOnboarding { get; set; }
    }
}
