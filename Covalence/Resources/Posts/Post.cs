using System.Collections.Generic;
using Covalence.Authentication;

namespace Covalence
{
    public enum PostType {
        Question,
        Mentor,
        Item
    }
    public class Post
    {
        public Post() {
            Tags = new HashSet<PostTag>();
        }
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public HashSet<PostTag> Tags { get; set; }
        public ApplicationUser Author { get; set; }
        public PostType Category { get; set; }

        public override string ToString()
        {
            return $"Post [ Category = {Category.ToString()}, Title = {Title} ]";
        }
    }
}