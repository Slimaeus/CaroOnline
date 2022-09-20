using Model.DbModels;
using Model.RequestModels;
using Model.ResponseModels;
using Model.ResultModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Service.APIServices
{
    public interface IResultService
    {
        Task<APIResult<string>> AddResult (ResultRequest resultRequest);
        APIResult<IEnumerable<ResultResponse>> GetResults(
            Expression<Func<Result, bool>> filter = null!,
            Func<IQueryable<Result>, IOrderedQueryable<Result>> orderBy = null!,
            string includeProperties = "",
            int skip = 0,
            int take = 0);
    }
}
