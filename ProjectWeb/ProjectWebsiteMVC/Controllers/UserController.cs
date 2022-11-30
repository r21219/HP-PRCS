using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProjectAPI.Model;
using ProjectWeb.Model;

namespace ProjectWebsiteMVC.Controllers
{
    public class UserController : Controller
    {
        private static HttpClient _httpClient;
        private string URLUser = "https://localhost:7115/api/User";
        public IActionResult Index()
        {
            return View();
        }
        static UserController()
        {
            _httpClient = new HttpClient();
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

