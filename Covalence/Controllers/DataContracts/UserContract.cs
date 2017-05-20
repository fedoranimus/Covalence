using System.Collections.Generic;

namespace Covalence.Contracts
{
    public class UserContract
    {
        public UserContract()
        {
            StudyTags = new List<Tag>();
            ExpertTags = new List<Tag>();
        }

        public string Email { get; set; }
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Location { get; set;}
        public IEnumerable<Tag> StudyTags { get; set; }
        public IEnumerable<Tag> ExpertTags { get; set; }
        //public List<Connection> Connections { get; set; }
    }
}