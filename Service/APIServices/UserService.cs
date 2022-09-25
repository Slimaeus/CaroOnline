using AutoMapper;
using Data.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Model.DbModels;
using Model.RequestModels;
using Model.ResponseModels;
using Model.ResultModels;
using System.Linq;
using System.Linq.Expressions;

namespace Service.APIServices
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly IJwtManager _jwtManager;
        private readonly IMapper _mapper;
        private readonly SignInManager<User> _signInManager;

        public UserService(UserManager<User> userManager, IJwtManager jwtManager, IMapper mapper, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _jwtManager = jwtManager;
            _mapper = mapper;
            _signInManager = signInManager;
        }
        public async Task<ApiResult<string>> Authenticate(LoginRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null)
            {
                return new ApiErrorResult<string>("User does not exist!");
            }
            var result = await _userManager.CheckPasswordAsync(user, request.Password);
            //var result = await _signInManager.PasswordSignInAsync(user, request.Password, request.RememberMe, true);
            if (!result)
            {
                return new ApiErrorResult<string>("Username or Password Uncorrect!");
            }
            var roles = await _userManager.GetRolesAsync(user);

            var token = _jwtManager.Authenticate(user, roles);
            return new ApiSuccessResult<string>(token);
        }

        public async Task<ApiResult<bool>> Delete(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return new ApiErrorResult<bool>("User does not exist!");
            }
            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
                return new ApiSuccessResult<bool>(true);
            return new ApiErrorResult<bool>("Delete Fail!");
        }

        public async Task<ApiResult<bool>> Delete(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return new ApiErrorResult<bool>("User does not exist!");
            }
            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
                return new ApiSuccessResult<bool>(true);
            return new ApiErrorResult<bool>("Delete failed!");
        }

        public async Task<ApiResult<UserResponse>> GetById(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
                return new ApiErrorResult<UserResponse>("User does not exist!");
            var roles = await _userManager.GetRolesAsync(user);
            var userResponse = _mapper.Map<UserResponse>(user);
            userResponse.Roles = roles;
            return new ApiSuccessResult<UserResponse>(userResponse);

        }

        public async Task<ApiResult<UserResponse>> GetByUserName(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
                return new ApiErrorResult<UserResponse>("User Does Not Exist!");
            var roles = await _userManager.GetRolesAsync(user);
            var userResponse = _mapper.Map<UserResponse>(user);
            userResponse.Roles = roles;
            return new ApiSuccessResult<UserResponse>(userResponse);
        }

        public async Task<IEnumerable<User>> GetUserList(
            Expression<Func<User, bool>>? filter = null,
            Func<IQueryable<User>, IOrderedQueryable<User>>? orderBy = null,
            string includeProperties = "",
            int take = 0,
            int skip = 0)
        {
            IQueryable<User> query = _userManager.Users.AsQueryable<User>();
            IList<User> userList;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
            {
                query = query.Include(includeProperty);
            }
            if (skip > 0)
            {
                query = query.Skip(skip);
            }
            if (take > 0)
            {
                query = query.Take(take);
            }
            if (orderBy != null)
            {
                userList = await orderBy(query).ToListAsync();
            }
            else
            {
                userList = await query.ToListAsync();
            }

            return userList;
        }

        public async Task<ApiResult<PagedList<UserResponse>>> GetUserPagingList(PagingRequest request)
        {
            const int DEFAULT_PAGE_SIZE = 10;
            const int DEFAULT_PAGE_INDEX = 1;
            int pageSize = DEFAULT_PAGE_SIZE;
            int pageIndex = DEFAULT_PAGE_INDEX;
            if (request.PageSize > 0)
            {
                pageSize = request.PageSize;
            }
            if (request.PageIndex > 0)
            {
                pageIndex = request.PageIndex;
            }
            int totalUser = await _userManager.Users.CountAsync();
            var userList = await GetUserList(
                skip: pageSize * (pageIndex - 1),
                take: pageSize
            );
            var userResponseList = _mapper.Map<IEnumerable<UserResponse>>(userList);
            var pageResult = new PagedList<UserResponse>
            {
                TotalCount = totalUser,
                PageIndex = pageIndex,
                PageSize = pageSize,
                Items = userResponseList
            };
            return new ApiSuccessResult<PagedList<UserResponse>>(pageResult);
        }

        public async Task<ApiResult<bool>> Register(RegisterRequest request)
        {
            var findUserName = await _userManager.FindByNameAsync(request.UserName);
            var findEmail = await _userManager.FindByEmailAsync(request.Email);

            bool isUserNameExists = findUserName != null;
            bool isEmailExists = findEmail != null;

            if (isUserNameExists)
                return new ApiErrorResult<bool>("This Username Already Used!");
            if (isEmailExists)
                return new ApiErrorResult<bool>("This Email Already Used");
            if (request.Password != request.ConfirmPassword)
                return new ApiErrorResult<bool>("Password and Confirm Password are not the same");
            var user = _mapper.Map<User>(request);
            var result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {
                return new ApiSuccessResult<bool>();
            }
            return new ApiErrorResult<bool>("Register Fail!");
        }

        public async Task<ApiResult<bool>> RoleAssign(string userName, RoleAssignRequest request)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return new ApiErrorResult<bool>("User Does Not Exist");
            }
            var removedRoles = request.Roles.Where(x => x.Selected == false).Select(x => x.Name).ToList();
            foreach (var roleName in removedRoles)
            {
                if (await _userManager.IsInRoleAsync(user, roleName) == true)
                {
                    await _userManager.RemoveFromRoleAsync(user, roleName);
                }
            }
            await _userManager.RemoveFromRolesAsync(user, removedRoles);

            var addedRoles = request.Roles.Where(x => x.Selected).Select(x => x.Name).ToList();
            foreach (var roleName in addedRoles)
            {
                if (await _userManager.IsInRoleAsync(user, roleName) == false)
                {
                    await _userManager.AddToRoleAsync(user, roleName);
                }
            }

            return new ApiSuccessResult<bool>();
        }

        public async Task<ApiResult<bool>> Update(string userName, UpdateUserRequest request)
        {
            if (request.Email != null && await _userManager.Users.AnyAsync(x => x.Email == request.Email))
            {
                return new ApiErrorResult<bool>("Email Already Exists");
            }
            if (userName != null && await _userManager.Users.AnyAsync(x => x.UserName == userName))
            {
                return new ApiErrorResult<bool>("UserName Already Exists");
            }
            var user = await _userManager.FindByIdAsync(request.UserName);
            bool isEmailExists = request.Email != null;
            bool isPhoneNumberExists = request.PhoneNumber != null;
            bool isUserNameExists = request.UserName != null;
            bool isNewPasswordExists = request.UpdatePassword != null;

            var getTokenTasks = new List<Task<string>>();
            if (isEmailExists)
                getTokenTasks.Add(_userManager.GenerateChangeEmailTokenAsync(user, request.Email));
            if (isPhoneNumberExists)
                getTokenTasks.Add(_userManager.GenerateChangePhoneNumberTokenAsync(user, request.PhoneNumber));
            var tokens = await Task.WhenAll(getTokenTasks);

            List<Task> updateTasks = new();
            if (isUserNameExists)
                updateTasks.Add(_userManager.SetUserNameAsync(user, request.UserName));
            if (isEmailExists)
                updateTasks.Add(_userManager.ChangeEmailAsync(user, request.Email, tokens[0]));
            if (isPhoneNumberExists)
                updateTasks.Add(_userManager.ChangePhoneNumberAsync(user, request.PhoneNumber, tokens[1]));
            if (isNewPasswordExists)
                updateTasks.Add(_userManager.ChangePasswordAsync(user, request.CurrentPassword, request.UpdatePassword));
            await Task.WhenAll(
                updateTasks
            );

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return new ApiSuccessResult<bool>();
            }
            return new ApiErrorResult<bool>("Update Failed!");
        }
    }
}
