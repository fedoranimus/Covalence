using System;
using System.Collections.Generic;
using Covalence.Authentication;

namespace Covalence.Contracts
{
    public class PostContract
    {
        public PostContract()
        {
            Tags = new HashSet<TagContract>();
        }
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public RemoteUserContract Author { get; set; }
        public PostType Category { get; set; }
        public string DateCreated { get; set; }
        public string DateModified { get; set; }
        public ICollection<TagContract> Tags { get; set; }

    }
}