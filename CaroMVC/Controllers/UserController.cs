using Microsoft.AspNetCore.Mvc;
using Model.ActionModels;
using Model.RequestModels;
using Service.APIClientServices;

namespace CaroMVC.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserAPIClient _userAPIClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserController(IUserAPIClient userAPIClient, IHttpContextAccessor httpContextAccessor)
        {
            _userAPIClient = userAPIClient;
            _httpContextAccessor = httpContextAccessor;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel loginModel, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var result = await _userAPIClient.Authenticate(loginModel.Input);
                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(returnUrl))
                        return LocalRedirect(returnUrl);
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Invalid Credentials");
            }

            return View(loginModel);
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(RegisterRequest loginModel, string returnUrl)
        {
            return View();
        }
    }
}
