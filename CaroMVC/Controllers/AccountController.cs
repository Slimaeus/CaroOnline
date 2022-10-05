using System.Text.Encodings.Web;
using AutoMapper;
using CaroMVC.Models;
using Data.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Model.ActionModels;
using Model.RequestModels;
using NuGet.Protocol;
using Service.APIClientServices;

namespace CaroMVC.Controllers;
[Authorize]
public class AccountController : Controller
{
    private readonly IUserApiClient _userApiClient;
    private readonly IJwtManager _jWtManager;
    private readonly IMapper _mapper;
    private readonly IEmailSender _emailSender;

    public AccountController(
        IUserApiClient userApiClient,
        IJwtManager jWtManager,
        IMapper mapper,
        IEmailSender emailSender)
    {
        _userApiClient = userApiClient;
        _jWtManager = jWtManager;
        _mapper = mapper;
        _emailSender = emailSender;
    }
    public IActionResult Index()
    {
        return View();
    }
    public async Task<IActionResult> Profile()
    {
        var userIdentity = User.Identity;
        if (userIdentity == null)
        {
            return RedirectToAction(nameof(Login), new LoginModel() { ReturnUrl = Request.Path });
        }
        var userName = userIdentity.Name;
        if (userName == null)
        {
            return RedirectToAction(nameof(Login), new LoginModel() { ReturnUrl = Request.Path });
        }
        var result = await _userApiClient.GetByUserName(userName);
        if (!result.Succeeded)
        {
            ModelState.AddModelError("", result.Message);
            return View("Error", new ErrorViewModel
            {
                RequestId = userName
            });
        }
        var user = result.ResultObject;
        return View(user);
    }
    [AllowAnonymous]
    public IActionResult Login(string? returnUrl)
    {
        if (User.Identity!.IsAuthenticated)
            return RedirectToAction("Index", "Home");
        LoginModel model = new();
        if (returnUrl != null)
            model.ReturnUrl = returnUrl;
        return View(model);
    }
    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Login(LoginModel loginModel)
    {
        if (!ModelState.IsValid)
        {
            ViewData["Error"] = "Invalid Input!";
            return View(loginModel);
        }
        var response = await _userApiClient.Authenticate(loginModel.Input);
        if (response == null)
        {
            ViewData["Error"] = "Cannot connect to Server";
            return View(loginModel);
        }
        if (!response.Succeeded)
        {
            ViewData["Error"] = response.Message;
            return View(loginModel);
        }
        var token = response.ResultObject;
        if (token == null)
        {
            ViewData["Error"] = "Cannot Get Token";
            return View(loginModel);
        }
        var userPrincipal = _jWtManager.Validate(token);
        var expire = _jWtManager.GetExpireDate(token);
        var authProperties = new AuthenticationProperties
        {
            ExpiresUtc = expire,
            IsPersistent = loginModel.RememberMe
        };
        HttpContext.Session.SetString("Token", token);
        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            userPrincipal,
            authProperties
        );
        if (!string.IsNullOrEmpty(loginModel.ReturnUrl))
            return LocalRedirect(loginModel.ReturnUrl);
        return RedirectToAction("Index", "Home");
    }
    public async Task<IActionResult> Logout(string returnUrl)
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        if (!string.IsNullOrEmpty(returnUrl))
            return LocalRedirect(returnUrl);
        return RedirectToAction("Index", "Home");
    }
    [AllowAnonymous]
    public IActionResult Register(string? returnUrl)
    {
        if (User.Identity!.IsAuthenticated)
            return RedirectToAction("Index", "Home");
        RegisterModel model = new();
        if (returnUrl != null)
            model.ReturnUrl = returnUrl;
        return View(model);
    }
    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Register(RegisterModel model)
    {

        if (!ModelState.IsValid)
            return View(model);
        var response = await _userApiClient.Register(model.Input);
        if (response == null)
        {
            ModelState.AddModelError("", "Cannot connect to Server");
            return View(model);
        }
        if (!response.Succeeded)
        {
            ModelState.AddModelError("", response.Message);
            return View(model);
        }
        // Login 
        var loginModel = _mapper.Map<LoginModel>(model);
        var loginResponse = await _userApiClient.Authenticate(loginModel.Input);
        if (!loginResponse.Succeeded)
        {
            ModelState.AddModelError("", "Login Failure");
            return View(model);
        }
        var token = loginResponse.ResultObject;
        if (token == null)
        {
            ModelState.AddModelError("", "Cannot Get Token");
            return View(model);
        }
        var userPrincipal = _jWtManager.Validate(token);
        var authProperties = new AuthenticationProperties
        {
            ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
            IsPersistent = false
        };
        HttpContext.Session.SetString("Token", token);
        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            userPrincipal,
            authProperties
        );

        if (!string.IsNullOrEmpty(loginModel.ReturnUrl))
            return LocalRedirect(loginModel.ReturnUrl);
        return RedirectToAction(nameof(HomeController.Index), "Home");
    }
    [AllowAnonymous]
    public IActionResult ForgotPassword(string returnUrl)
    {
        ForgotPasswordModel model = new();
        if (returnUrl != null)
            model.ReturnUrl = returnUrl;
        return View(model);
    }
    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordModel model)
    {
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError("", "Invalid Input");
            return View(model);
        }
        var response = await _userApiClient.GetConfirmCode(model.Input);
        if (response == null)
        {
            ModelState.AddModelError("", "Cannot connect to Server");
            return View(model);
        }
        if (!response.Succeeded)
        {
            ModelState.AddModelError("", response.Message);
            return View(model);
        }
        var code = response.ResultObject;
        if (code == null)
        {
            ModelState.AddModelError("", "Cannot get the Code");
            return View(model);
        }
        var callbackUrl = Url.Action(nameof(ConfirmEmail), new { code, email = model.Input.Email });
        await _emailSender.SendEmailAsync(
            model.Input.Email, 
            "Reset Password", 
            $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl!)}'>CLICKING HERE</a>."
        );
        if (!string.IsNullOrEmpty(model.ReturnUrl))
            return LocalRedirect(model.ReturnUrl);
        return RedirectToAction(nameof(Index), "Home");
    }
    public IActionResult ChangePassword()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> ChangePassword(ChangePasswordModel model)
    {
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError("", "Invalid Input!");

            return View(model);
        }
        if (model.ConfirmPassword != model.Input.NewPassword)
        {
            ModelState.AddModelError("", "Confirm Password and New Password not match!");
            return View(model);
        }
        var userIdentity = User.Identity;
        if (userIdentity == null)
        {
            return RedirectToAction(nameof(Login), new LoginModel() { ReturnUrl = Request.Path });
        }
        var userName = userIdentity.Name;
        if (userName == null)
        {
            return RedirectToAction(nameof(Login), new LoginModel() { ReturnUrl = Request.Path });
        }
        model.Input.UserName = userName;
        var response = await _userApiClient.ChangePassword(model.Input);
        if (response == null)
        {
            ModelState.AddModelError("", "Cannot Connect to Server!");

            return View(model);
        }
        if (!response.Succeeded)
        {
            ModelState.AddModelError("", response.Message);

            return View(model);
        }
        var result = response.ResultObject;
        if (!result)
        {
            ModelState.AddModelError("", "Change Password Failure!");

            return View(model);
        }
        return RedirectToAction("Index", "Account");
    }
    public async Task<IActionResult> ConfirmEmail([FromQuery] ConfirmEmailRequest request)
    {
        if (!ModelState.IsValid)
        {
            ViewData["Error"] = "Invalid Input!";
            return View(request);
        }
        var response = await _userApiClient.ConfirmEmail(request);
        if (response == null)
        {
            ViewData["Error"] = "Cannot connect to Server";
            return View(request);
        }
        if (!response.Succeeded)
        {
            ViewData["Error"] = response.Message;
            return View(request);
        }
        var result = response.ResultObject;
        if (!result)
        {
            ViewData["Error"] = "Email confirmed failure!";
            return View(request);
        }
        return RedirectToAction(nameof(ConfirmEmailSuccess));
    }
    public IActionResult ConfirmEmailSuccess([FromQuery] ConfirmEmailRequest request)
    {
        return View(request);
    }
    public IActionResult ResendEmailConfirmation()
    {
        throw new NotImplementedException();
    }
}