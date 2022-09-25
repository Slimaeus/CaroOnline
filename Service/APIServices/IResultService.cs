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
        Task<ApiResult<string>> AddResult (ResultRequest resultRequest);
        Task<ApiResult<string>> DeleteResultByUserName (string userName, DeleteResultRequest resultRequest);
        Task<ApiResult<string>> DeleteResultById (Guid resultId, DeleteResultRequest resultRequest);
        ApiResult<IEnumerable<ResultResponse>> GetResults(PagingRequest pagingRequest);
        Task<ApiResult<IEnumerable<ResultResponse>>> GetResultsByUserName(string userName, PagingRequest pagingRequest);
    }
}
