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
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public UserController(IUserService userService, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userService = userService;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [Authorize]
        [HttpGet("GetAllUsers")]
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
            //User foundUser = await _userManager.FindByNameAsync(signUpModel.UserName);
            //if (foundUser != null)
            //    return BadRequest("This account already exists!");
            //User user = new()
            //{
            //    UserName = signUpModel.UserName,
            //    Email = signUpModel.Email,
            //    PhoneNumber = signUpModel.PhoneNumber,
            //    InGameName = signUpModel.InGameName,
            //};
            //var result = await _userManager.CreateAsync(user, signUpModel.Password);
            //if (result.Succeeded)
            //{
            //    await _signInManager.SignInAsync(user, isPersistent: false);
            //    return CreatedAtAction("Login", new LoginRequest { 
            //        UserName = user.UserName,
            //        Password = signUpModel.Password,
            //    });
            //}
            //return BadRequest();
        }
        [HttpPost("Login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginModel)
        {
            if (!ModelState.IsValid)
                return Unauthorized(ModelState);
            var authResult = await _userService.Authenticate(loginModel);
            if (string.IsNullOrEmpty(authResult.ResultObject))
                return Unauthorized(authResult);
            return Ok(authResult);
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> LogOut()
        {
            if (_signInManager.IsSignedIn(User))
            {
                await _signInManager.SignOutAsync();
                return Ok("Sign Out successfully!");
            }
            return BadRequest("You did not sign in yet!");
        }
        [Authorize]
        [HttpPut("UpdateProfile")]
        public async Task<IActionResult> UpdateProfile(UpdateUserRequest updateUserRequest)
        {
            var user = await _userManager.FindByIdAsync(updateUserRequest.Id.ToString());
            if (user != null && await _userManager.CheckPasswordAsync(user, updateUserRequest.CurrentPassword))
            {
                bool isEmailExists = updateUserRequest.Email != null;
                bool isPhoneNumberExists = updateUserRequest.PhoneNumber != null;
                bool isUserNameExists = updateUserRequest.UserName != null;
                bool isNewPasswordExists = updateUserRequest.UpdatePassword != null;
                var getTokenTasks = new List<Task<string>>();
                if (isEmailExists)
                    getTokenTasks.Add(_userManager.GenerateChangeEmailTokenAsync(user, updateUserRequest.Email));
                if (isPhoneNumberExists)
                    getTokenTasks.Add(_userManager.GenerateChangePhoneNumberTokenAsync(user, updateUserRequest.PhoneNumber));
                var tokens = await Task.WhenAll(getTokenTasks);

                List<Task> updateTasks = new List<Task>();
                if (isUserNameExists)
                    updateTasks.Add(_userManager.SetUserNameAsync(user, updateUserRequest.UserName));
                if (isEmailExists)
                    updateTasks.Add(_userManager.ChangeEmailAsync(user, updateUserRequest.Email, tokens[0]));
                if (isPhoneNumberExists)
                    updateTasks.Add(_userManager.ChangePhoneNumberAsync(user, updateUserRequest.PhoneNumber, tokens[1]));
                if (isNewPasswordExists)
                    updateTasks.Add(_userManager.ChangePasswordAsync(user, updateUserRequest.CurrentPassword, updateUserRequest.UpdatePassword));
                await Task.WhenAll(
                    updateTasks
                );

                await _userManager.UpdateAsync(user);
            }
            return NotFound();
        }
    }
}
