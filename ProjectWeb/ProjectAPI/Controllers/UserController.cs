﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectAPI.Model;
using ProjectWeb.Model;
using ProjectWeb.Model.Data;

namespace ProjectAPI.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly TodoDbContext todoDbContext;
        public UserController(TodoDbContext todoDbContext)
        {
            this.todoDbContext = todoDbContext;
        }
        /// <summary>
        /// Used for getting the user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUser(int id)
        {
            if (id == 0)
            {
                return BadRequest("Wrong id input");
            }
            else if(todoDbContext.Users.Where(o => o.Id == id) == null)
            {
                return NotFound("User not found");
            }
            else
            {
                return Ok(await todoDbContext.Users.Where(o => o.Id == id)
                    .Select(o => new UserDTO
                    {
                        Id = o.Id,
                        Forename = o.Forename,
                        Gender = o.Gender,
                        IsAdmin = o.IsAdmin,
                        Surname = o.Surname,
                        Password = o.Password,
                        Username = o.Username,
                    })
                    .FirstOrDefaultAsync());
            }
        }
        /// <summary>
        /// Used for creating a user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateUser(UserDTO user)
        {
            var isExisting = await todoDbContext.Users.FindAsync(user.Username);
            if (user == null && isExisting == null)
            {
                return BadRequest("Cannot create user because the user already exists or the input doesn't exist");
            }
            else
            {
                todoDbContext.Users.Add(new User
                {
                    Forename = user.Forename,
                    Surname = user.Surname,
                    IsAdmin = user.IsAdmin,
                    Gender = user.Gender,
                    Password = user.Password,
                    Username = user.Username
                });
                await todoDbContext.SaveChangesAsync();
                return Ok("User added");
            }
        }

        /// <summary>
        /// Used for updating a user
        /// </summary>
        /// <param idname="id">id of the user which is to be changed</param>
        /// <param name="userDTO">new data which will be sent</param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> UpdateUser(int id, [FromBody] NewUserDTO userDTO)
        {
            var ret = await todoDbContext.Users.FindAsync(id);
            if (ret != null && userDTO != null)
            {
                ret.Username = userDTO.Username;
                ret.Surname = userDTO.Surname;
                ret.Forename = userDTO.Forename;
                ret.Gender = userDTO.Gender;
                ret.Password = userDTO.Password;
                todoDbContext.Users.Update(ret);
                await todoDbContext.SaveChangesAsync();
                return Ok("Todo sucessfully updated");
            }
            else
            {
                return BadRequest("Wrong todo format");
            }
        }
    }
}
