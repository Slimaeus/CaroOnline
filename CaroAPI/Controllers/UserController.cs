using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Model.RequestModels;
using Model.DbModels;
using Model.ResponseModels;
using Model.ResultModels;
using Service.APIServices;

namespace CaroAPI.Controllers
{
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
        [HttpGet("GetPagedList")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserResponse))]
        public async Task<IActionResult> GetPagedList([FromQuery] PagingRequest pagingRequest)
        {
            var result = await _userService.GetUserPagingList(pagingRequest);
            if (result.Succeeded)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        /// <summary>
        /// Regist an account 
        /// </summary>
        /// <param name="signUpModel">Register information</param>
        /// <returns>Rigister Status</returns>
        [AllowAnonymous]
        [HttpPost("Register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] RegisterRequest signUpModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _userService.Register(signUpModel);
            if (!result.Succeeded)
                return BadRequest(result);
            return Ok(result);
        }
        /// <summary>
        /// Authenticate your Login
        /// </summary>
        /// <param name="loginModel">Login infomations</param>
        /// <returns>Your token</returns>
        [AllowAnonymous]
        [HttpPost("Authenticate")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Authenticate([FromBody] LoginRequest loginModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var authResult = await _userService.Authenticate(loginModel);
            if (string.IsNullOrEmpty(authResult.ResultObject))
                return Unauthorized(authResult);
            return Ok(authResult);
        }
        /// <summary>
        /// Update User Profile
        /// </summary>
        /// <param name="userName">UserName</param>
        /// <param name="updateUserRequest">Update Infomations</param>
        /// <returns>Update Status</returns>
        [HttpPut("Update/{userName}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(string userName, [FromBody] UpdateUserRequest updateUserRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _userService.Update(userName, updateUserRequest);
            if (result.Succeeded)
                return Ok(result);
            return BadRequest(result);
        }
        /// <summary>
        /// Delete an account
        /// </summary>
        /// <param name="userName">UserName</param>
        /// <returns>Delete Status</returns>
        [HttpDelete("Delete/{userName}")]
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
        [HttpGet("GetByUserName/{userName}")]
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
        /// <param name="roleAssignRequest">Role Assign Informations</param>
        /// <returns>Assign Status</returns>
        [HttpPut("RoleAssign/{userName}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RoleAssign(string userName, RoleAssignRequest roleAssignRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _userService.RoleAssign(userName, roleAssignRequest);
            if (result.Succeeded)
                return Ok(result);
            return BadRequest(result);
        }
    }
}
