using System.Collections.Generic;

namespace Covalence {
    public class PagingContract<T> {
        public PagingContract() {
            Items = new List<T>();
        }

        public ICollection<T> Items { get; set; }
        public int PageNumber { get; set; }
        public int TotalPages { get; set; }
        public bool HasNextPage { get; set; }
        public bool HasPreviousPage { get; set; }
    }
}