using Microsoft.AspNetCore.Mvc;

namespace ProjectWeb.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
