using System.Collections.Generic;
using Covalence.API;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Covalence.Authentication
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            StudyTags = new HashSet<StudyUserTag>();
            ExpertTags = new HashSet<ExpertUserTag>();
            //Connections = new List<Connection>();
            //StudyTags = new HashSet<Tag>();
            //ExpertTags = new HashSet<UserTag>();
            //Connections = new List<Connection>();
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Location { get; set; }

        public ICollection<StudyUserTag> StudyTags { get; set; }
        public ICollection<ExpertUserTag> ExpertTags { get; set; }

        //public ICollection<Connection> Connections { get; set; }
    }
}
