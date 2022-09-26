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
    public interface IUserService
    {
        Task<ApiResult<string>> Authenticate(LoginRequest request);
        Task<ApiResult<bool>> Register(RegisterRequest request);
        Task<ApiResult<bool>> Update(string userName, UpdateUserRequest request);
        Task<ApiResult<UserResponse>> GetById(Guid id);
        Task<ApiResult<UserResponse>> GetByUserName(string userName);
        Task<ApiResult<PagedList<UserResponse>>> GetUserPagingList(PagingRequest request);
        Task<IEnumerable<AppUser>> GetUserList(
            Expression<Func<AppUser, bool>>? filter = null,
            Func<IQueryable<AppUser>, IOrderedQueryable<AppUser>>? orderBy = null,
            string includeProperties = "",
            int take = 0,
            int skip = 0
        );
        Task<ApiResult<bool>> Delete(Guid id);
        Task<ApiResult<bool>> Delete(string userName);
        Task<ApiResult<bool>> RoleAssign(string userName, RoleAssignRequest request);
    }
}
