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
        private readonly SignInManager<User> _signInManager;
        private readonly IJWTManager _jwtManager;
        private readonly IMapper _mapper;

        public UserService(UserManager<User> userManager, SignInManager<User> signInManager, IJWTManager jwtManager, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtManager = jwtManager;
            _mapper = mapper;
        }
        public async Task<APIResult<string>> Authenticate(LoginRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null)
            {
                return new APIErrorResult<string>("Username does not exist!");
            }
            var result = await _userManager.CheckPasswordAsync(user, request.Password) ;
            if (result == false)
            {
                return new APIErrorResult<string>("Username or Password uncorrect!");
            }
            var roles = await _userManager.GetRolesAsync(user);

            var token = _jwtManager.Authenticate(user, roles);
            return new APISuccessResult<string>(token);
        }

        public async Task<APIResult<bool>> Delete(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return new APIErrorResult<bool>("User does not exist!");
            }
            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
                return new APISuccessResult<bool>();
            return new APIErrorResult<bool>("Delete failed!");
        }

        public async Task<APIResult<UserResponse>> GetById(Guid id)
        {

            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
                return new APIErrorResult<UserResponse>("User does not exist!");
            var roles = await _userManager.GetRolesAsync(user);
            var userResponse = _mapper.Map<UserResponse>(user);
            userResponse.Roles = roles;
            return new APISuccessResult<UserResponse>(userResponse);

        }

        public async Task<APIResult<IEnumerable<UserResponse>>> GetUserList(Expression<Func<User, bool>>? filter, Func<IQueryable<User>, IOrderedQueryable<User>>? orderBy, string includeProperties = "", int take = 0, int skip = 0)
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
            if (userList.Count <= 0)
                return new APIErrorResult<IEnumerable<UserResponse>>("There is no user!");
            var userResponseList = _mapper.Map<IEnumerable<UserResponse>>(userList);
            return new APISuccessResult<IEnumerable<UserResponse>>(userResponseList);
        }

        public async Task<APIResult<bool>> Register(RegisterRequest request)
        {
            var findUserName = await _userManager.FindByNameAsync(request.UserName);
            var findEmail = await _userManager.FindByEmailAsync(request.Email);
            //await Task.WhenAll(
            //        findUserName,
            //        findEmail
            //);

            bool isUserNameExists = findUserName != null;
            bool isEmailExists = findEmail != null;

            if (isUserNameExists)
                return new APIErrorResult<bool>("This username already used!");
            if (isEmailExists)
                return new APIErrorResult<bool>("This email already used");

            var user = _mapper.Map<User>(request);
            var result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {
                return new APISuccessResult<bool>();
            }
            return new APIErrorResult<bool>("Register failed!");
        }

        public async Task<APIResult<bool>> RoleAssign(Guid id, RoleAssignRequest request)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return new APIErrorResult<bool>("User does not exist");
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

            return new APISuccessResult<bool>();
        }

        public async Task<APIResult<bool>> Update(Guid id, UpdateUserRequest request)
        {
            if (await _userManager.Users.AnyAsync(x => x.Email == request.Email && x.Id != id))
            {
                return new APIErrorResult<bool>("Email already exists");
            }
            var user = await _userManager.FindByIdAsync(id.ToString());
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

            List<Task> updateTasks = new List<Task>();
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
                return new APISuccessResult<bool>();
            }
            return new APIErrorResult<bool>("Cập nhật không thành công");
        }
    }
}
