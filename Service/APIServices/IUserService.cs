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
        Task<APIResult<string>> Authenticate(LoginRequest request);
        Task<APIResult<bool>> Register(RegisterRequest request);
        Task<APIResult<bool>> Update(string userName, UpdateUserRequest request);
        Task<APIResult<UserResponse>> GetById(Guid id);
        Task<APIResult<UserResponse>> GetByUserName(string userName);
        Task<APIResult<PagedList<UserResponse>>> GetUserPagingList(PagingRequest request);
        Task<IEnumerable<User>> GetUserList(
            Expression<Func<User, bool>>? filter = null,
            Func<IQueryable<User>, IOrderedQueryable<User>>? orderBy = null,
            string includeProperties = "",
            int take = 0,
            int skip = 0
        );
        Task<APIResult<bool>> Delete(Guid id);
        Task<APIResult<bool>> Delete(string userName);
        Task<APIResult<bool>> RoleAssign(string userName, RoleAssignRequest request);
    }
}
