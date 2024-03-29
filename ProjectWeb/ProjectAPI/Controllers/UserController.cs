﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectAPI.Model;
using ProjectAPI.Model.Data;

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
        /// <param name="name">Username of user</param>
        /// /// <param name="password">password of the user</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUser(string name, string password)
        {
            if (name == null || password == null)
            {
                return BadRequest("Wrong id input");
            }
            var user = await todoDbContext.Users
                .Where(o => (o.Username == name || o.Email == name) && o.Password == password)
                .Select(o => new UserDTO
                {
                    Id = o.Id,
                    Forename = o.Forename,
                    Gender = o.Gender,
                    IsAdmin = o.IsAdmin,
                    Surname = o.Surname,
                    Password = o.Password,
                    Username = o.Username,
                    Email = o.Email,
                })
                .FirstOrDefaultAsync();
            if (user == null)
            {
                return NotFound("User not found");
            }
            else
            {
                return Ok(user);
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
        public async Task<IActionResult> CreateUser([FromBody] NewUserDTO user)
        {
            var isExisting =  todoDbContext.Users.Where(o => o.Username == user.Username || o.Email == user.Email).Any();
            if (user == null || isExisting == true)
            {
                return BadRequest("Cannot create user because the user already exists or the input doesn't exist");
            }
            else
            {
                todoDbContext.Users.Add(new User
                {
                    Forename = user.Forename,
                    Surname = user.Surname,
                    IsAdmin = false,
                    Gender = user.Gender,
                    Password = user.Password,
                    Username = user.Username,
                    Email = user.Email,
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
                var isExisting = todoDbContext.Users.Where(o => (o.Username == userDTO.Username || o.Email == userDTO.Email) && o.Id != id).Any();
                if (isExisting == true)
                {
                    return BadRequest("Other user is already using this email or username");
                }

                ret.Username = userDTO.Username;
                ret.Surname = userDTO.Surname;
                ret.Forename = userDTO.Forename;
                ret.Gender = userDTO.Gender;
                ret.Password = userDTO.Password;
                ret.Email = userDTO.Email;
                todoDbContext.Users.Update(ret);
                await todoDbContext.SaveChangesAsync();
                return Ok("Todo sucessfully updated");
            }
            else
            {
                return BadRequest("Wrong todo format");
            }
        }

        /// <summary>
        /// Used for deleting the user
        /// </summary>
        /// <param name="id">The id of the user which will be deleted from the database</param>
        /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> DeleteTodo(int id)
        {
            var user = await todoDbContext.Users.FindAsync(id);
            if (user != null)
            {
                todoDbContext.Users.Remove(user);
                await todoDbContext.SaveChangesAsync();
                return Ok("User sucessfully deleted");
            }
            else
            {
                return BadRequest("No user found");
            }
        }
    }
}
