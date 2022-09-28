﻿using System.Text;
using System.Text.Encodings.Web;
using AutoMapper;
using CaroMVC.Models;
using Data.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Model.ActionModels;
using Model.DbModels;
using Model.ResponseModels;
using Service.APIClientServices;

namespace CaroMVC.Controllers;

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
        return RedirectToAction(nameof(Profile));
    }
    public async Task<IActionResult> Profile()
    {
        // Handle result.ResultObject null
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
        if (returnUrl == null)
            return View(new LoginModel());
        LoginModel model = new()
        {
            ReturnUrl = returnUrl
        };

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
        return RedirectToAction("Index", "Home");
    }

    public async Task<IActionResult> ForgotPassword()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> ForgotPassword(ForgetPasswordModel model)
    {
        if (!ModelState.IsValid)
            return View(model);
        // Add Send 
        // await _emailSender.SendEmailAsync(
        //     Input.Email,
        //     "Reset Password",
        //     $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
        return Content("NO CONTENT");
    }
    public IActionResult EmailConfirmation()
    {
        return View();
    }
}