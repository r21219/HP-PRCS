using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProjectAPI.Model;
using ProjectAPI.Model.DTO;

namespace ProjectWebsiteMVC.Controllers
{
    public class TodoController : Controller
    {
        private static HttpClient _httpClient;
        private string URLTODO = "https://localhost:7115/api/Todo";
        static TodoController()
        {
            _httpClient = new HttpClient();
        }

        [HttpGet]
        public IActionResult Index()
        {
            var user = JsonConvert.DeserializeObject<UserDTO>(HttpContext.Session.GetString(SessionKeyManager.SessionKey));
            var result = _httpClient.GetAsync(URLTODO + "?userId=" + user.Id).Result;
            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                List<TodoDTO> ret = new();
                ret = JsonConvert.DeserializeObject<List<TodoDTO>>(result.Content.ReadAsStringAsync().Result);
                return View(ret);
            }
            return View("Error has occured");
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(NewTodoDTO newTodoDTO)
        {
            var user = JsonConvert.DeserializeObject<UserDTO>(HttpContext.Session.GetString(SessionKeyManager.SessionKey));
            if (ModelState.IsValid)
            {
                newTodoDTO.UserId = user.Id;
                var ret = _httpClient.PostAsJsonAsync(URLTODO,newTodoDTO).Result;

                if (ret.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return View("Index");
                }
                else
                {
                    return View();
                }
            }
            return View();
        }

        public IActionResult Update(int todoId)
        {
            var user = JsonConvert.DeserializeObject<UserDTO>(HttpContext.Session.GetString(SessionKeyManager.SessionKey));
            var result = _httpClient.GetAsync(URLTODO + "/" + user.Id + "/" + todoId).Result;
            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                TodoDTO todo = JsonConvert.DeserializeObject<TodoDTO>(result.Content.ReadAsStringAsync().Result);
                return View(todo);
            }
            else
            {
                return View("Index");
            }
        }

        [HttpPost]
        public IActionResult Update(NewTodoDTO newTodoDTO)
        {
            if (ModelState.IsValid)
            {
                var result = _httpClient.PutAsJsonAsync(URLTODO, newTodoDTO).Result;
                if (result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return View("Detail");
                }
                else
                {
                    return View();
                }
            }
            return View();
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var result = _httpClient.DeleteAsync(URLTODO +"?id=" + id).Result;
            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return View("Index");
            }
            else
            {
                return View("Index");
            }
        }
    }
}
