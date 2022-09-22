using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.RequestModels
{
    public class PagingRequest
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
