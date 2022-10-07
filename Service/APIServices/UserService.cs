using AutoMapper;
using Data.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Model.DbModels;
using Model.RequestModels;
using Model.ResponseModels;
using Model.ResultModels;
using System.Linq.Expressions;
using System.Text;
using Microsoft.AspNetCore.WebUtilities;

namespace Service.APIServices;

public class UserService : IUserService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IJwtManager _jwtManager;
    private readonly IMapper _mapper;

    public UserService(UserManager<AppUser> userManager, IJwtManager jwtManager, IMapper mapper)
    {
        _userManager = userManager;
        _jwtManager = jwtManager;
        _mapper = mapper;
    }
    public async Task<ApiResult<string>> Authenticate(LoginRequest request)
    {
        var user = await _userManager.FindByNameAsync(request.UserName);
        if (user == null)
            return new ApiErrorResult<string>("User does not exist!");
        var result = await _userManager.CheckPasswordAsync(user, request.Password);
        if (!result)
            return new ApiErrorResult<string>("Username or Password Incorrect!");
        var roles = await _userManager.GetRolesAsync(user);
        var token = _jwtManager.Authenticate(user, roles);
        return new ApiSuccessResult<string>(token);
    }

    public async Task<ApiResult<bool>> Delete(Guid id)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());
        if (user == null)
            return new ApiErrorResult<bool>("User does not exist!");
        var result = await _userManager.DeleteAsync(user);
        if (result.Succeeded)
            return new ApiSuccessResult<bool>(true);
        return new ApiErrorResult<bool>(string.Join(' ', result.Errors.Select(error => error.Description)));
    }

    public async Task<ApiResult<bool>> Delete(string userName)
    {
        var user = await _userManager.FindByNameAsync(userName);
        if (user == null)
            return new ApiErrorResult<bool>("User does not exist!");
        var result = await _userManager.DeleteAsync(user);
        if (result.Succeeded)
            return new ApiSuccessResult<bool>(true);
        return new ApiErrorResult<bool>(string.Join(' ', result.Errors.Select(error => error.Description)));
    }

    public async Task<ApiResult<UserResponse>> GetById(Guid id)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());
        if (user == null)
            return new ApiErrorResult<UserResponse>("User does not exist!");
        var userResponse = _mapper.Map<UserResponse>(user);
        return new ApiSuccessResult<UserResponse>(userResponse);

    }

    public async Task<ApiResult<UserResponse>> GetByUserName(string userName)
    {
        var user = await _userManager.FindByNameAsync(userName);
        if (user == null)
            return new ApiErrorResult<UserResponse>("User Does Not Exist!");
        var userResponse = _mapper.Map<UserResponse>(user);
        return new ApiSuccessResult<UserResponse>(userResponse);
    }

    public async Task<IEnumerable<AppUser>> GetUserList(
        Expression<Func<AppUser, bool>>? filter = null,
        Func<IQueryable<AppUser>, IOrderedQueryable<AppUser>>? orderBy = null,
        string includeProperties = "",
        int take = 0,
        int skip = 0)
    {
        var query = _userManager.Users.AsQueryable();
        IList<AppUser> userList;
        if (filter != null) query = query.Where(filter);
        foreach (var includeProperty in includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
            query = query.Include(includeProperty);
        if (skip > 0) query = query.Skip(skip);
        if (take > 0) query = query.Take(take);
        if (orderBy != null)
            userList = await orderBy(query).ToListAsync();
        else
            userList = await query.ToListAsync();
        return userList;
    }

    public async Task<ApiResult<PagedList<UserResponse>>> GetUserPagingList(PagingRequest request)
    {
        const int defaultPageSize = 10;
        const int defaultPageIndex = 1;
        var pageSize = defaultPageSize;
        var pageIndex = defaultPageIndex;
        
        if (request.PageSize > 0) pageSize = request.PageSize;
        if (request.PageIndex > 0) pageIndex = request.PageIndex;
        
        var totalUser = await _userManager.Users.CountAsync();
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

        var isUserNameExists = findUserName != null;
        var isEmailExists = findEmail != null;

        if (isUserNameExists)
            return new ApiErrorResult<bool>("This Username Already Used!");
        if (isEmailExists)
            return new ApiErrorResult<bool>("This Email Already Used");
        if (request.Password != request.ConfirmPassword)
            return new ApiErrorResult<bool>("Password and Confirm Password are not the same");
        var user = _mapper.Map<AppUser>(request);
        var result = await _userManager.CreateAsync(user, request.Password);

        if (result.Succeeded)
            return new ApiSuccessResult<bool>(true);
        return new ApiErrorResult<bool>(string.Join(' ', result.Errors.Select(error => error.Description)));
    }

    public async Task<ApiResult<bool>> ChangePassword(ChangePasswordRequest request)
    {
        var user = await _userManager.FindByNameAsync(request.UserName);
        if (user == null)
            return new ApiErrorResult<bool>("User Does Not Exist");
        var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
        if (result.Succeeded)
            return new ApiSuccessResult<bool>(true);
        return new ApiErrorResult<bool>(string.Join(' ', result.Errors.Select(error => error.Description))); 
    }

    public async Task<ApiResult<string>> GetConfirmCode(GetConfirmCodeRequest request)
    {
        if (string.IsNullOrEmpty(request.Email))
            return new ApiErrorResult<string>("Email cannot Null or Empty!");
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
            return new ApiErrorResult<string>($"Cannot find user with email: {request.Email}");
        
        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        if (code == null) 
            return new ApiErrorResult<string>("Generate code failure!");
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
        return new ApiSuccessResult<string>(code);
    }

    public async Task<ApiResult<bool>> ConfirmEmail(ConfirmEmailRequest request)
    {
        if (string.IsNullOrEmpty(request.Email))
            return new ApiErrorResult<bool>("Email cannot be Null or Empty!");
        if (string.IsNullOrEmpty(request.Code))
            return new ApiErrorResult<bool>("Code cannot be Null or Empty!");
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
            return new ApiErrorResult<bool>("Cannot find User!");

        var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Code));
        var result = await _userManager.ConfirmEmailAsync(user, code);
        if (result.Succeeded)
            return new ApiSuccessResult<bool>(true);
        return new ApiErrorResult<bool>(string.Join(' ', result.Errors.Select(error => error.Description)));
    }

    public async Task<ApiResult<bool>> RoleAssign(RoleAssignRequest request)
    {
        var user = await _userManager.FindByNameAsync(request.UserName);
        if (user == null)
            return new ApiErrorResult<bool>("User Does Not Exist");
        var removedRoles = request.Roles.Where(x => !x.Selected).Select(x => x.Name).ToList();
        foreach (var roleName in removedRoles)
            if (await _userManager.IsInRoleAsync(user, roleName))
                await _userManager.RemoveFromRoleAsync(user, roleName);
        await _userManager.RemoveFromRolesAsync(user, removedRoles);

        var addedRoles = request.Roles.Where(x => x.Selected).Select(x => x.Name).ToList();
        foreach (var roleName in addedRoles)
            if (!await _userManager.IsInRoleAsync(user, roleName))
                await _userManager.AddToRoleAsync(user, roleName);

        return new ApiSuccessResult<bool>(true);
    }

    public async Task<ApiResult<bool>> Update(UpdateUserRequest request)
    {
        if (request.Email != null && await _userManager.Users.AnyAsync(x => x.Email == request.Email))
            return new ApiErrorResult<bool>("Email Already Exists");
        var user = await _userManager.FindByNameAsync(request.UserName);
        if (!string.IsNullOrEmpty(request.Email))
            user.Email = request.Email;
        if (!string.IsNullOrEmpty(request.InGameName))
            user.InGameName = request.InGameName;
        if (!string.IsNullOrEmpty(request.PhoneNumber))
            user.PhoneNumber = request.PhoneNumber;
        if (request.Level != -1)
            user.Level = request.Level;
        if (request.Exp != -1)
            user.Exp = request.Exp;
        if (request.Score != -1)
            user.Score = request.Score;

        var result = await _userManager.UpdateAsync(user);
        if (result.Succeeded)
            return new ApiSuccessResult<bool>(true);
        return new ApiErrorResult<bool>(string.Join(' ', result.Errors.Select(error => error.Description)));
    }

    public async Task<ApiResult<PagedList<RankingResponse>>> GetRanking(PagingRequest pagingRequest)
    {
        const int defaultPageSize = 10;
        const int defaultPageIndex = 1;
        var pageSize = defaultPageSize;
        var pageIndex = defaultPageIndex;

        if (pagingRequest.PageSize > 0) pageSize = pagingRequest.PageSize;
        if (pagingRequest.PageIndex > 0) pageIndex = pagingRequest.PageIndex;
        var rankings = await _userManager.Users.AsQueryable()
            .Include(u => u.UserResults)
            .Select(u => new RankingResponse()
            {
                UserName = u.UserName,
                InGameName = u.InGameName!,
                Level = u.Level,
                Win = u.UserResults.Sum(ur => ur.IsWinner ? 1 : 0),
                Draw = 0,
                Lose = u.UserResults.Sum(ur => ur.IsWinner ? 0 : 1),
                Score = u.UserResults.Sum(ur => ur.Score)
            })
            .OrderByDescending(r => r.Score)
            .ThenByDescending(r => r.Win)
            .Skip(pageSize * (pageIndex - 1))
            .Take(pageSize)
            .ToListAsync();
        rankings.ForEach(r =>
        {
            r.Top = rankings.IndexOf(r) + pageSize * (pageIndex - 1) + 1;
        });
        var totalUser = _userManager.Users.Count();
        var pageResult = new PagedList<RankingResponse>
        {
            TotalCount = totalUser,
            PageIndex = pageIndex,
            PageSize = pageSize,
            Items = rankings
        };
        return new ApiSuccessResult<PagedList<RankingResponse>>(pageResult);
    }
}