using Microsoft.AspNetCore.Mvc;

namespace ProjectWebsiteMVC.Controllers
{
    public class TodoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
