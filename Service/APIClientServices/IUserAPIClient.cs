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

namespace Service.APIClientServices
{
    public interface IUserAPIClient
    {

        Task<APIResult<string>> Authenticate(LoginRequest request);
        Task<APIResult<bool>> Register(RegisterRequest request);
        Task<APIResult<bool>> Update(string userName, UpdateUserRequest request);
        Task<APIResult<PagedList<UserResponse>>> GetPagedList(PagingRequest pagingRequest);
        Task<APIResult<IEnumerable<UserResponse>>> GetUserList(
            Expression<Func<User, bool>>? filter = null,
            Func<IQueryable<User>, IOrderedQueryable<User>>? orderBy = null,
            string includeProperties = "",
            int take = 0,
            int skip = 0
        );
        Task<APIResult<UserResponse>> GetByUserName(string userName);
        Task<APIResult<bool>> Delete(DeleteUserRequest deleteUserRequest);
        Task<APIResult<bool>> RoleAssign(string userName, RoleAssignRequest request);
        Task<APIResult<TResult>> ResultReturn<TResult>(HttpResponseMessage response);
    }
}
