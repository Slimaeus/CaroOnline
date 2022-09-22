using AutoMapper;
using CaroMVC.Models;
using Data.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Model.ActionModels;
using Model.DbModels;
using Model.RequestModels;
using Service.APIClientServices;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CaroMVC.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserAPIClient _userAPIClient;
        private readonly IConfiguration _configuration;
        private readonly IJWTManager _jWTManager;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _memoryCache;

        public UserController(
            IUserAPIClient userAPIClient,
            IConfiguration configuration,
            IJWTManager jWTManager,
            IMapper mapper,
            IMemoryCache memoryCache)
        {
            _userAPIClient = userAPIClient;
            _configuration = configuration;
            _jWTManager = jWTManager;
            _mapper = mapper;
            _memoryCache = memoryCache;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Profile()
        {
            // Handle result.ResultObject null
            var userIdentity = User.Identity;
            if (userIdentity == null)
                return RedirectToAction(nameof(Login));
            var userName = userIdentity.Name;
            if (userName == null)
                return RedirectToAction(nameof(Login));
            var result = await _userAPIClient.GetByUserName(userName);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "UserName not found");
                
                return View("Error", new ErrorViewModel
                {
                    RequestId = "Profile"
                });
            }
            var user = result.ResultObject;
            return View(user);
        }
        public IActionResult Login(string? returnUrl)
        {
            if (returnUrl == null)
                return View(new LoginModel());
            LoginModel model = new()
            {
                ReturnUrl = returnUrl
            };
            return View(model);
        }
        public async Task<IActionResult> Logout(string returnUrl)
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (!string.IsNullOrEmpty(returnUrl))
                return LocalRedirect(returnUrl);
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            if (!ModelState.IsValid)
                return View(loginModel);
            var response = await _userAPIClient.Authenticate(loginModel.Input);
            if (!response.Succeeded)
            {
                ModelState.AddModelError("", "Login Failure");
            }
            var token = response.ResultObject;
            if (token == null)
            {
                ViewData["Warning"] = "Cannot Get Token";
                return View(loginModel);
            }
            var userPrincipal = _jWTManager.Validate(token);
            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddSeconds(10),
                IsPersistent = false
            };
            var options = new MemoryCacheEntryOptions
            {
                AbsoluteExpiration = DateTimeOffset.UtcNow.AddSeconds(10)
            };
            _memoryCache.Set("Token", token, options);
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                userPrincipal,
                authProperties
            );

            if (!string.IsNullOrEmpty(loginModel.ReturnUrl))
                return LocalRedirect(loginModel.ReturnUrl);
            return RedirectToAction("Index", "Home");
        }
        public IActionResult Register(string returnUrl)
        {
            RegisterModel model = new()
            {
                ReturnUrl = returnUrl
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {

            if (!ModelState.IsValid)
                return View(model);
            var response = await _userAPIClient.Register(model.Input);
            if (!response.Succeeded)
            {
                ModelState.AddModelError("", "Register Failure");
            }
            // Login 
            var loginModel = _mapper.Map<LoginModel>(model);
            var loginResponse = await _userAPIClient.Authenticate(loginModel.Input);

            if (!loginResponse.Succeeded)
            {
                ModelState.AddModelError("", "Login Failure");
            }
            var token = loginResponse.ResultObject;
            var userPrincipal = _jWTManager.Validate(token);
            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddSeconds(10),
                IsPersistent = false
            };
            var options = new MemoryCacheEntryOptions
            {
                AbsoluteExpiration = DateTimeOffset.UtcNow.AddSeconds(10)
            };
            _memoryCache.Set("Token", token, options);
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                userPrincipal,
                authProperties
            );

            if (!string.IsNullOrEmpty(loginModel.ReturnUrl))
                return LocalRedirect(loginModel.ReturnUrl);
            return RedirectToAction("Index", "Home");
        }
    }
}
