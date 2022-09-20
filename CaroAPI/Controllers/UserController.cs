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
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        //[Authorize]
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
            IEnumerable<UserResponse> users = result.ResultObject;
            return Ok(users);
        }
        [HttpPost("SignUp")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SignUp([FromBody] RegisterRequest signUpModel)
        {
            var result = await _userService.Register(signUpModel);
            if (!result.Succeeded)
                return BadRequest(result);
            return Ok(result);
        }
        [HttpPost("Authenticate")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Authenticate([FromBody] LoginRequest loginModel)
        {
            if (!ModelState.IsValid)
                return Unauthorized(ModelState);
            var authResult = await _userService.Authenticate(loginModel);
            if (string.IsNullOrEmpty(authResult.ResultObject))
                return Unauthorized(authResult);
            return Ok(authResult);
        }
        [Authorize]
        [HttpPut("Update")]
        public async Task<IActionResult> Update(UpdateUserRequest updateUserRequest)
        {
            var result = await _userService.Update(updateUserRequest.Id ,updateUserRequest);
            if (result.Succeeded)
                return Ok(result);
            return BadRequest(result);
        }
    }
}
