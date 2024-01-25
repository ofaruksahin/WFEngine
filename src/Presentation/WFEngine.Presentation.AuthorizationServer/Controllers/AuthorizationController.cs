using Microsoft.AspNetCore.Mvc;
using WFEngine.Presentation.AuthorizationServer.ViewModels;

namespace WFEngine.Presentation.AuthorizationServer.Controllers
{
    public class AuthorizationController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginViewModel loginViewModel)
        {
            return View();
        }

        [HttpGet]
        public IActionResult Error()
        {
            return View();
        }
    }
}
