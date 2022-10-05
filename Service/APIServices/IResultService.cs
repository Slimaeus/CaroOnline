using Model.RequestModels;
using Model.ResponseModels;
using Model.ResultModels;

namespace Service.APIServices;

public interface IResultService
{
    Task<ApiResult<string>> AddResult (ResultRequest resultRequest);
    Task<ApiResult<string>> DeleteResultByUserName (string userName, DeleteResultRequest resultRequest);
    Task<ApiResult<string>> DeleteResultById (Guid resultId, DeleteResultRequest resultRequest);
    ApiResult<IEnumerable<ResultResponse>> GetResults(PagingRequest pagingRequest);
    Task<ApiResult<IEnumerable<ResultResponse>>> GetResultsByUserName(string userName, PagingRequest pagingRequest);
    Task<ApiResult<PagedList<HistoryResponse>>> GetHistoryByUserName(string userName, PagingRequest pagingRequest);
}