using Covalence.Authentication;

namespace Covalence
{
    public class StudyUserTag
    {
        public virtual string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        
        public virtual string Name { get; set; }
        public virtual Tag Tag { get; set; }
    }
}