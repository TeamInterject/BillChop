using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BillChopBE.DataAccessLayer.Models;
using BillChopBE.Services;
using BillChopBE.Services.Models;
using Microsoft.AspNetCore.Mvc;

namespace BillChopBE.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService userService;

        public UsersController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<IList<User>>> GetUsers() 
        {
            return Ok(await userService.GetUsersAsync());
        }

        [HttpPost("login")]
        public async Task<ActionResult<IList<User>>> Login([FromBody] LoginDetails loginDetails)
        {
            return Ok(await userService.LoginAsync(loginDetails));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(Guid id)
        {
            return Ok(await userService.GetUserAsync(id));
        }

        
        /// <summary>
        /// Searches for users by matching partial keyword to usernames and emails
        /// </summary>
        /// <param name="keyword">Substring of username or email</param>
        /// <param name="top">Amount of returned results</param>
        /// <returns></returns>
        [HttpGet("search/{keyword}")]
        public async Task<ActionResult<User>> SearchForUsers(string keyword, int top = 10)
        {
            return Ok(await userService.SearchForUsersAsync(keyword, top));
        }

        [HttpPost]
        public async Task<ActionResult<User>> CreateUser([FromBody] CreateNewUser newUser)
        {
            return Ok(await userService.AddUserAsync(newUser));
        }
    }
}
