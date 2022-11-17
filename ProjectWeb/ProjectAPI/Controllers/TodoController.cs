using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectAPI.Model;
using ProjectWeb.Model.Data;

namespace ProjectAPI.Controllers
{
    [Route("api/[Todo]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly TodoDbContext todoDbContext;
        
        public TodoController(TodoDbContext todoDbContext)
        {
            this.todoDbContext = todoDbContext;
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<IActionResult> GetCategories(int userId)
        {
            if (todoDbContext.Users.Where(o => o.Id == userId) == null)
            {
                return NotFound();
            }
            else if (todoDbContext.Users.Where(o => o.Id == userId && o.IsAdmin) != null)
            {
                return Ok(await todoDbContext.Todos
                    .Select(o => new TodoDTO
                    {
                        Date = o.Date,
                        Description = o.Description,
                        Id = o.Id,
                        Status = o.Status,
                        UserId = userId,
                    })
                    .ToListAsync());
            }
            else
            {
                return Ok(todoDbContext.Todos
                    .Where(o => o.UserId == userId)
                    .Select(o => new TodoDTO
                    {
                        Date = o.Date,
                        Description = o.Description,
                        Id = o.Id,
                        Status = o.Status,
                        UserId = userId,
                    })
                    .ToListAsync());
            }
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        
        public async Task<IActionResult> Create(int userId)
        {
            return NotImplementedException();
        }
    }
}
