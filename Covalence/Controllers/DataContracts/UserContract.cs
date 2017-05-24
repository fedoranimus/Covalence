using System.Collections.Generic;

namespace Covalence.Contracts
{
    public class UserContract
    {
        public UserContract()
        {
            StudyTags = new List<TagContract>();
            ExpertTags = new List<TagContract>();
        }

        public string Email { get; set; }
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Location { get; set;}
        public IEnumerable<TagContract> StudyTags { get; set; }
        public IEnumerable<TagContract> ExpertTags { get; set; }
        //public List<Connection> Connections { get; set; }
    }
}