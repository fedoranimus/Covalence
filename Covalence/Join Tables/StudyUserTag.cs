using Covalence.Authentication;

namespace Covalence
{
    public class StudyUserTag
    {
        public virtual string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        
        public virtual int TagId { get; set; }
        public virtual Tag Tag { get; set; }
    }
}