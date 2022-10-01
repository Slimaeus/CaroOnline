using Model.RequestModels;
using Model.ResponseModels;
using Model.ResultModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.APIClientServices;

public interface IResultApiClient
{
    Task<ApiResult<string>> Create(ResultRequest request);
    Task<ApiResult<PagedList<ResultResponse>>> GetPagedList(PagingRequest request);
    Task<ApiResult<IEnumerable<ResultResponse>>> GetResultByUserName(string userName, PagingRequest request);
    Task<ApiResult<IEnumerable<HistoryResponse>>> GetHistoryByUserName(string userName, PagingRequest request);
}