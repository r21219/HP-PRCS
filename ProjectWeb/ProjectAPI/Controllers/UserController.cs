using Microsoft.AspNetCore.Mvc;
using ProjectWeb.Model.Data;

namespace ProjectAPI.Controllers
{
    [Route("api/[User]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly TodoDbContext todoDbContext;
        public UserController(TodoDbContext todoDbContext)
        {
            this.todoDbContext = todoDbContext;
        }

    }
}
