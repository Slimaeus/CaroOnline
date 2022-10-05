using Model.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ActionModels
{
    public class HistoryModel
    {
        public PagedList<HistoryResponse> Input { get; set; } = new PagedList<HistoryResponse>();
    }
}
