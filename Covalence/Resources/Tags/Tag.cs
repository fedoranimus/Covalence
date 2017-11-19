using System.Collections.Generic;

namespace Covalence
{
    public enum TagType
    {
        Study = 0,
        Expert = 1
    }
    
    public class Tag {
        public Tag() {
            Users = new HashSet<UserTag>();
        }
        public string Name { get; set; }
        public HashSet<UserTag> Users { get; set; }

        public override string ToString()
        {
            return $"Tag [ Name = {Name}]";
        }
    }
}