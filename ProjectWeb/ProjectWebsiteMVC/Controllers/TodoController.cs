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
                List<TodoDTO> ret;
                ret = JsonConvert.DeserializeObject<List<TodoDTO>>(result.Content.ReadAsStringAsync().Result);
                return View(ret);
            }
            return View();
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
                    TempData["Sucess"] = "Todo sucessfully created";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Error"] = "Todo was not created";
                    return View();
                }
            }
            TempData["Error"] = "Wrong input";
            return View();
        }

        public IActionResult Update(int id)
        {
            var user = JsonConvert.DeserializeObject<UserDTO>(HttpContext.Session.GetString(SessionKeyManager.SessionKey));
            var result = _httpClient.GetAsync(URLTODO + "/" + user.Id + "/" + id).Result;
            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                TodoDTO todo = JsonConvert.DeserializeObject<TodoDTO>(result.Content.ReadAsStringAsync().Result);
                return View(todo);
            }
            else
            {
                TempData["Error"] = "Todo could not be located";
                return View("Index");
            }
        }

        [HttpPost]
        public IActionResult Update(TodoDTO newTodoDTO)
        {
            if (ModelState.IsValid)
            {
                var result = _httpClient.PutAsJsonAsync(URLTODO + "?id=" + newTodoDTO.Id, newTodoDTO).Result;
                if (result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    TempData["Sucess"] = "Todo was updated";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Error"] = "Todo was not updated";
                    return View();
                }
            }
            TempData["Error"] = "Wrong input";
            return View();
        }
        
        public IActionResult Delete(int id)
        {
            var result = _httpClient.DeleteAsync(URLTODO +"?id=" + id).Result;
            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                TempData["Sucess"] = "Todo was sucessfully deleted";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Error"] = "Todo was not deled";
                return View("Index");
            }
        }
    }
}
