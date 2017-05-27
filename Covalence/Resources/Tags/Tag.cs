using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Covalence
{
    public enum TagType
    {
        Study = 0,
        Expert = 1
    }
    
    public class Tag {
        public Tag() {
            StudyUsers = new HashSet<StudyUserTag>();
            ExpertUsers = new HashSet<ExpertUserTag>();
            //StudyUsers = new HashSet<ApplicationUser>();
            //ExpertUsers = new HashSet<ApplicationUser>();
        }
        public string Name { get; set; }
        public string Description { get; set; }

        public HashSet<StudyUserTag> StudyUsers { get; set; }
        public HashSet<ExpertUserTag> ExpertUsers { get; set; }

        public override string ToString()
        {
            return $"Tag [ Name = {Name}, Description = {Description}]";
        }
    }
}