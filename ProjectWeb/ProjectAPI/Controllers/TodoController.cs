using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectAPI.Model;
using ProjectAPI.Model.Data;
using ProjectAPI.Model.DTO;

namespace ProjectAPI.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly TodoDbContext todoDbContext;
        
        public TodoController(TodoDbContext todoDbContext)
        {
            this.todoDbContext = todoDbContext;
        }

        /// <summary>
        /// Used for getting TODO
        /// </summary>
        /// <param name="userId">Id of the user</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<IActionResult> GetTodo(int userId)
        {
            if (!todoDbContext.Users.Where(o => o.Id == userId).Any())
            {
                return NotFound();
            }
            else if (todoDbContext.Users.Where(o => o.Id == userId && o.IsAdmin).Any())
            {
                return Ok(await todoDbContext.Todos
                    .Select(o => new TodoDTO
                    {
                        Date = o.Date,
                        Description = o.Description,
                        Id = o.Id,
                        Status = o.Status,
                        UserId = o.UserId,
                    })
                    .ToListAsync());
            }
            else
            {
                return Ok(await todoDbContext.Todos
                    .Where(o => o.UserId == userId)
                    .Select(o => new TodoDTO
                    {
                        Date = o.Date,
                        Description = o.Description,
                        Id = o.Id,
                        Status = o.Status,
                        UserId = o.UserId,
                    })
                    .ToListAsync());
            }
        }
        /// <summary>
        /// Used for getting TODO
        /// </summary>
        /// <param name="userId">Id of the user</param>
        /// <returns></returns>
        [HttpGet("{userId:int}/{todoId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<IActionResult> GetSingleTodo(int userId, int todoId)
        {
            if (!todoDbContext.Todos.
                Where(o => o.Id == userId && o.Id == todoId)
                .Any() 
                && !todoDbContext.Users
                .Where(o => o.Id == userId && o.IsAdmin != true)
                .Any())
            {
                return NotFound();
            }
            else if ((todoDbContext.Users.Where(o => o.Id == userId && o.IsAdmin == true).Any()))
            {
                return Ok(await todoDbContext.Todos
                        .Where(o => o.UserId == userId && o.Id == todoId)
                        .Select(o => new TodoDTO
                        {
                            Date = o.Date,
                            Description = o.Description,
                            Id = o.Id,
                            Status = o.Status,
                            UserId = o.UserId,
                        })
                        .FirstAsync());
            }
            else
            {
                return Ok(await todoDbContext.Todos
                    .Where(o => o.UserId == userId && o.Id == todoId)
                    .Select(o => new TodoDTO
                    {
                        Date = o.Date,
                        Description = o.Description,
                        Id = o.Id,
                        Status = o.Status,
                        UserId = o.UserId,
                    })
                    .FirstAsync());
            }
        }
        /// <summary>
        /// Used for creating TODO
        /// </summary>
        /// <param name="todoDTO">The data which will be uploaded to the database</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        
        public async Task<IActionResult> CreateTodo([FromBody] NewTodoDTO todoDTO)
        {
            if (todoDTO != null)
            {
                todoDbContext.Todos.Add(new Todo
                {
                    UserId = todoDTO.UserId,
                    Date = todoDTO.Date,
                    Description = todoDTO.Description,
                    Status = todoDTO.Status
                });
                await todoDbContext.SaveChangesAsync();
                return Ok("Todo sucessfully created");
            }
            else
            {
                return BadRequest("Wrong todo format");
            }
        }
        /// <summary>
        /// Used for updating TODO
        /// </summary>
        /// <param idname="id">id of the todo which is to be changed</param>
        /// <param name="todoDTO">new data which will be sent</param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> UpdateTodo(int id, [FromBody] NewTodoDTO todoDTO)
        {
            var ret = await todoDbContext.Todos.FindAsync(id);
            if (ret != null && todoDTO != null)
            {
                ret.UserId = todoDTO.UserId;
                ret.Date = todoDTO.Date;
                ret.Description = todoDTO.Description;
                ret.Status = todoDTO.Status;
                todoDbContext.Todos.Update(ret);
                await todoDbContext.SaveChangesAsync();
                return Ok("Todo sucessfully updated");
            }
            else
            {
                return BadRequest("Wrong todo format");
            }
        }

        /// <summary>
        /// Used for deleting TODO
        /// </summary>
        /// <param name="id">The id of todo which will be deleted from the database</param>
        /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> DeleteTodo(int id)
        {
            var todo = await todoDbContext.Todos.FindAsync(id);
            if (todo != null)
            {
                todoDbContext.Todos.Remove(todo);
                await todoDbContext.SaveChangesAsync();
                return Ok("Todo sucessfully deleted");
            }
            else
            {
                return BadRequest("No todo found");
            }
        }
    }
}
