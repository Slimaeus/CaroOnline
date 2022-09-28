using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model.RequestModels;
using Service.APIServices;

namespace CaroAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }
    /// <summary>
    /// Get User paged list
    /// </summary>
    /// <param name="pagingRequest">Paging params</param>
    /// <returns>The User paged list</returns>
    [HttpGet("get-paged-list")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetPagedList([FromQuery] PagingRequest pagingRequest)
    {
        var result = await _userService.GetUserPagingList(pagingRequest);
        if (result.Succeeded)
            return Ok(result);
        return BadRequest(result);
    }
    /// <summary>
    /// Register an account 
    /// </summary>
    /// <param name="signUpModel">Register information</param>
    /// <returns>Register Status</returns>
    [AllowAnonymous]
    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] RegisterRequest signUpModel)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var result = await _userService.Register(signUpModel);
        if (result.Succeeded)
            return Ok(result);
        return BadRequest(result);
    }
    /// <summary>
    /// Authenticate your Login
    /// </summary>
    /// <param name="loginModel">Login Information</param>
    /// <returns>Your token</returns>
    [AllowAnonymous]
    [HttpPost("authenticate")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Authenticate([FromBody] LoginRequest loginModel)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var authResult = await _userService.Authenticate(loginModel);
        if (string.IsNullOrEmpty(authResult.ResultObject))
            return BadRequest(authResult);
        return Ok(authResult);
    }
    /// <summary>
    /// Update User Profile
    /// </summary>
    /// <param name="userName">UserName</param>
    /// <param name="updateUserRequest">Update Information</param>
    /// <returns>Update Status</returns>
    [HttpPut("update")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update([FromBody] UpdateUserRequest updateUserRequest)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var result = await _userService.Update(updateUserRequest);
        if (result.Succeeded)
            return Ok(result);
        return BadRequest(result);
    }
    /// <summary>
    /// Delete an account
    /// </summary>
    /// <param name="userName">UserName</param>
    /// <returns>Delete Status</returns>
    [HttpDelete("delete/{userName}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete(string userName)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var result = await _userService.Delete(userName);
        if (result.Succeeded)
            return Ok(result);
        return BadRequest(result);
    }
    /// <summary>
    /// Get User information by UserName
    /// </summary>
    /// <param name="userName">UserName</param>
    /// <returns>User's information</returns>
    [HttpGet("get-by-username/{userName}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetByUserName(string userName)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var result = await _userService.GetByUserName(userName);
        if (result.Succeeded)
            return Ok(result);
        return BadRequest(result);
    }
    /// <summary>
    /// Assign Roles for User
    /// </summary>
    /// <param name="userName">UserName</param>
    /// <param name="roleAssignRequest">Role Assign Information</param>
    /// <returns>Assign Status</returns>
    // WARNING: AFTER DEPLOY MUST UNCOMMENT THIS
    //[Authorize(Roles = "Admin")]
    [HttpPut("role-assign")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RoleAssign(RoleAssignRequest roleAssignRequest)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var result = await _userService.RoleAssign(roleAssignRequest);
        if (result.Succeeded)
            return Ok(result);
        return BadRequest(result);
    }
    /// <summary>
    /// Get Email Confirm Code
    /// </summary>
    /// <param name="request">Get Confirm Code</param>
    /// <returns>Confirm Code</returns>
    [HttpPost("get-confirm-code")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetConfirmCode(GetConfirmCodeRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var result = await _userService.SendConfirmCode(request);
        if (result.Succeeded)
            return Ok(result);
        return BadRequest(result);
    }

    [HttpPost("confirm-email")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ConfirmEmail(ConfirmEmailRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var result = await _userService.ConfirmEmail(request);
        if (result.Succeeded)
            return Ok(result);
        return BadRequest(result);
    }
}