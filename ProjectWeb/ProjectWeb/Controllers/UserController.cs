using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProjectAPI.Model;
using ProjectWeb.Model;

namespace ProjectWeb.Controllers
{
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private static HttpClient _httpClient;
        private string URLUser = "https://localhost:7115/api/User";
        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
        }
        static UserController()
        {
            _httpClient = new HttpClient();
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login(LoginUser user)
        {
            if (ModelState.IsValid)
            {
                var response = _httpClient.GetAsync(URLUser + "?name=" + user.Username + "&password=" + user.Password);
                if (response.Result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    UserDTO currentUser = JsonConvert.DeserializeObject<UserDTO>(response.Result.Content.ReadAsStringAsync().Result);
                    HttpContext.Session.SetString(SessionKeyManager.SessionKey, JsonConvert.SerializeObject(currentUser));

                    return View("index");
                }
                return View();
            }
            return View();
        }
    }
}
