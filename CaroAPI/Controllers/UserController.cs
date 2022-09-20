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
        [HttpGet("GetUsers")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserResponse))]
        public async Task<IActionResult> GetUsers()
        {
            var result = await _userService.GetUserList(
                orderBy: userList => userList.OrderBy(user => user.InGameName)
              );
            if (result.Succeeded)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [AllowAnonymous]
        [HttpPost("SignUp")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SignUp([FromBody] RegisterRequest signUpModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _userService.Register(signUpModel);
            if (!result.Succeeded)
                return BadRequest(result);
            return Ok(result);
        }
        [AllowAnonymous]
        [HttpPost("Authenticate")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Authenticate([FromBody] LoginRequest loginModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (!ModelState.IsValid)
                return Unauthorized(ModelState);
            var authResult = await _userService.Authenticate(loginModel);
            if (string.IsNullOrEmpty(authResult.ResultObject))
                return Unauthorized(authResult);
            return Ok(authResult);
        }
        [HttpPut("Update")]
        public async Task<IActionResult> Update(string userName, [FromBody] UpdateUserRequest updateUserRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _userService.Update(userName, updateUserRequest);
            if (result.Succeeded)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(string userName)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _userService.Delete(userName);
            if (result.Succeeded)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpGet("GetByUserName")]
        public async Task<IActionResult> GetByUserName(string userName)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _userService.GetByUserName(userName);
            if (result.Succeeded)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpPut("RoleAssign")]
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
