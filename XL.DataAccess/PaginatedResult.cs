using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

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
