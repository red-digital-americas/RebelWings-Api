using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace biz.bd1.Paged
{
    public class PagedList<T>
    {
        public int TotalRows { get; set; }
        public IList<T> Paged { get; set; }

        public PagedList()
        {
            TotalRows = 0;
            Paged = new List<T>();
        }

        public PagedList(int totalRows, IList<T> result)
        {
            TotalRows = totalRows;
            Paged = result;
        }
    }
}
