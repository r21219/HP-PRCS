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

                    return View("detail");
                }
                else
                {
                    return View("index");
                }
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult Create(NewTodoDTO newUser)
        {
            if (ModelState.IsValid)
            {
                var result = _httpClient.PostAsJsonAsync(URLUser,newUser).Result;
                if (result.StatusCode == System.Net.HttpStatusCode.OK)
                {

                    return RedirectToAction("Detail");
                }
            }
            return View();
        }

        [HttpPost]
        public IActionResult Delete(int? id)
        {
            var response = _httpClient.DeleteAsync(URLUser + "?id=" + id.ToString()).Result;

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                LogOff();
            }
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return View("Error has occured");
            }
            return View("Detail");
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
                    return RedirectToAction("Detail");
                }
                else
                {
                    return View("Error has occured");
                }
            }
            return View("Detail");
        }

        public IActionResult LogOff()
        {
            HttpContext.Session.Remove(SessionKeyManager.SessionKey);
            return RedirectToAction("Index");
        }
    }
}

