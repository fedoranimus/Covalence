using System;
using System.Collections.Generic;
using Covalence.Authentication;

namespace Covalence.Contracts
{
    public class PostContract
    {
        public PostContract()
        {
            Tags = new List<TagContract>();
        }
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public RemoteUserContract Author { get; set; }
        public PostType Category { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public IEnumerable<TagContract> Tags { get; set; }

    }
}