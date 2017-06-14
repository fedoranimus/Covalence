namespace Covalence
{
    public class PostTag
    {
        public virtual int PostId { get; set; }
        public virtual Post Post { get; set; }
        public virtual string Name { get; set; }
        public virtual Tag Tag { get; set; }
    }
}