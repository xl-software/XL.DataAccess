using System.Collections.Generic;

namespace XL.DataAccess
{
    public class PaginatedResult<T> where T : class
    {
        public IList<T> Items { get; set; }
        public long Page { get; set; }
        public long Limit { get; set; }
        public long Total { get; set; }
    }
}
