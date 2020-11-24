using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BillChopBE.DataAccessLayer.Models;
using BillChopBE.Services;
using BillChopBE.Services.Models;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IList<UserWithoutPassword>>> GetUsers() 
        {
            return Ok(await userService.GetUsersAsync());
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserWithToken>> Login([FromBody] LoginDetails loginDetails)
        {
            return Ok(await userService.LoginAsync(loginDetails));
        }

        [Authorize]
        [HttpGet("current")]
        public async Task<ActionResult<UserWithoutPassword>> GetCurrentUser()
        {
            var id = Guid.Parse(User.FindFirst("Id").Value);
            var currentUser = await userService.GetUserAsync(id);

            System.Diagnostics.Debug.WriteLine($"Currently logged in user: {currentUser.Id} {currentUser.Email}");

            return Ok(currentUser);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<UserWithoutPassword>> GetUser(Guid id)
        {
            return Ok(await userService.GetUserAsync(id));
        }

        /// <summary>
        /// Searches for users by matching partial keyword to usernames and emails
        /// </summary>
        /// <param name="keyword">Substring of username or email</param>
        /// <param name="exclusionGroupId">Optional paramater for excluding users in passed group</param>
        /// <param name="top">Amount of returned results</param>
        /// <returns>List of matching users</returns>
        [Authorize]
        [HttpGet("search/{keyword}")]
        public async Task<ActionResult<IList<UserWithoutPassword>>> SearchForUsers(string keyword, Guid? exclusionGroupId, int top = 10)
        {
            return Ok(await userService.SearchForUsersAsync(keyword, exclusionGroupId, top));
        }

        [HttpPost]
        public async Task<ActionResult<UserWithoutPassword>> CreateUser([FromBody] CreateNewUser newUser)
        {
            return Ok(await userService.AddUserAsync(newUser));
        }
    }
}
