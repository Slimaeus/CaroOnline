using Microsoft.AspNetCore.Mvc;

namespace CaroMVC.Controllers
{
    public class UserController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserController(IHttpContextAccessor httpContextAccessor)
        {
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
    }
}
