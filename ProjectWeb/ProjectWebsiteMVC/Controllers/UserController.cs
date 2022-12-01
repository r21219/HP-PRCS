using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProjectAPI.Model;
using ProjectAPI.Model.DTO;


namespace ProjectWebsiteMVC.Controllers
{
    public class UserController : Controller
    {
        private static HttpClient _httpClient;
        private string URLUser = "https://localhost:7115/api/User";
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
                var response = _httpClient.GetAsync(URLUser + "?name=" + user.Username + "&password=" + user.Password).Result;
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    UserDTO currentUser = JsonConvert.DeserializeObject<UserDTO>(response.Content.ReadAsStringAsync().Result);
                    HttpContext.Session.SetString(SessionKeyManager.SessionKey, JsonConvert.SerializeObject(currentUser));
                    TempData["Sucess"] = "User sucessfully logged in";
                    return RedirectToAction("Update");
                }
                else
                {
                    TempData["Error"] = "Wrong username or password";
                    return RedirectToAction("index");
                }
            }
            return RedirectToAction("Index");
        }
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(NewTodoDTO newUser)
        {
            if (ModelState.IsValid)
            {
                var result = _httpClient.PostAsJsonAsync(URLUser,newUser).Result;
                if (result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    TempData["Sucess"] = "User sucessfully created";
                    return RedirectToAction("Update");
                }
            }
            TempData["Error"] = "User was not created";
            return View();
        }

        [HttpPost]
        public IActionResult Delete(int? id)
        {
            var response = _httpClient.DeleteAsync(URLUser + "?id=" + id.ToString()).Result;

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                TempData["Sucess"] = "User sucessfully deleted";
                Logout();
            }
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                TempData["Error"] = "User could not be deleted";
                return View("Update");
            }
            return View("Update");
        }
        public IActionResult Update()
        {
            var user = JsonConvert.DeserializeObject<UserDTO>(HttpContext.Session.GetString(SessionKeyManager.SessionKey));
            var result = _httpClient.GetAsync(URLUser + "?name=" + user.Username + "&password=" + user.Password).Result;
            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                UserDTO userDTO = JsonConvert.DeserializeObject<UserDTO>(result.Content.ReadAsStringAsync().Result);
                return View(userDTO);
            }
            else
            {
                TempData["Error"] = "User could not be found";
                return View("Index");
            }
        }
        [HttpPost]
        public IActionResult Update(NewUserDTO NewUserDTO)
        {
            if (ModelState.IsValid)
            {
                var user = JsonConvert.DeserializeObject<UserDTO>(HttpContext.Session.GetString(SessionKeyManager.SessionKey));
                var response = _httpClient.PutAsJsonAsync(URLUser + "?id=" + user.Id, NewUserDTO).Result;

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    TempData["Sucess"] = "User was sucessfully updated";
                    return RedirectToAction("Update");
                }
                else
                {
                    TempData["Error"] = "User could not be updated";
                    return View("Error has occured");
                }
            }
            return View("Update");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove(SessionKeyManager.SessionKey);
            TempData["Sucess"] = "User has logged off";
            return RedirectToAction("Index");
        }
    }
}

