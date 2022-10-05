using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ResponseModels
{
    public class PagedList<T>
    {
        public int PageIndex { get; set; } = 1;
        public bool HasNext => PageIndex < PageCount;
        public bool HasPrevious => PageIndex > 1;
        public int PageSize { get; set; } = 10;
        public int TotalCount { get; set; }
        public int PageCount 
        {
            get
            {
                var pageCount = (double)TotalCount / PageSize;
                return (int)Math.Ceiling(pageCount);    
            } 
        }
        public IEnumerable<T> Items { get; set; } = new List<T>();
    }
}
