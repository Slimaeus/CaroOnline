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

namespace Service.APIClientServices;

public interface IUserApiClient
{

    Task<ApiResult<string>> Authenticate(LoginRequest request);
    Task<ApiResult<bool>> Register(RegisterRequest request);
    Task<ApiResult<bool>> ChangePassword(ChangePasswordRequest request);
    Task<ApiResult<bool>> Update(UpdateUserRequest request);
    Task<ApiResult<UserResponse>> GetByUserName(string userName);
    Task<ApiResult<string>> GetConfirmCode(GetConfirmCodeRequest request);
    Task<ApiResult<PagedList<RankingResponse>>> GetRanking(PagingRequest request);
    Task<ApiResult<bool>> ConfirmEmail(ConfirmEmailRequest request);
    Task<ApiResult<PagedList<UserResponse>>> GetPagedList(PagingRequest request);
    Task<ApiResult<bool>> Delete(DeleteUserRequest request);
    Task<ApiResult<bool>> RoleAssign(RoleAssignRequest request);
    Task<ApiResult<TResult>> ResultReturn<TResult>(HttpResponseMessage response);
}