using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BillChopBE.Controllers.Models;
using BillChopBE.Exceptions;
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
        private readonly IMapper mapper;

        public UsersController(IUserService userService, IMapper mapper)
        {
            this.mapper = mapper;
            this.userService = userService;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IList<ApiUser>>> GetUsers() 
        {
            var users = await userService.GetUsersAsync();
            return mapper.Map<List<ApiUser>>(users);
        }

        [HttpPost("login")]
        public async Task<ActionResult<ApiUserWithToken>> Login([FromBody] LoginDetails loginDetails)
        {
            var user = await userService.LoginAsync(loginDetails);
            return mapper.Map<ApiUserWithToken>(user);
        }

        [Authorize]
        [HttpGet("current")]
        public async Task<ActionResult<ApiUser>> GetCurrentUser()
        {
            var userId = User.FindFirst("Id");
            if (userId == null)
                throw new UnauthorizedException();

            var id = Guid.Parse(userId.Value);
            var currentUser = await userService.GetUserAsync(id);

            System.Diagnostics.Debug.WriteLine($"Currently logged in user: {currentUser.Id} {currentUser.Email}");

            return mapper.Map<ApiUser>(currentUser);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiUser>> GetUser(Guid id)
        {
            var user = await userService.GetUserAsync(id);
            return mapper.Map<ApiUser>(user);
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
        public async Task<ActionResult<IList<ApiUser>>> SearchForUsers(string keyword, Guid? exclusionGroupId, int top = 10)
        {
            var users = await userService.SearchForUsersAsync(keyword, exclusionGroupId, top);
            return mapper.Map<List<ApiUser>>(users);
        }

        [HttpPost]
        public async Task<ActionResult<ApiUser>> CreateUser([FromBody] CreateNewUser newUserData)
        {
            var user = await userService.AddUserAsync(newUserData);
            return mapper.Map<ApiUser>(user);
        }
    }
}
