using Microsoft.AspNetCore.Mvc;

namespace ProjectWeb.Controllers
{
    public class TodoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
