namespace Covalence
{
    public class UserTag
    {
        public virtual string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual string Name { get; set; }
        public virtual Tag Tag { get; set; }
    }
}