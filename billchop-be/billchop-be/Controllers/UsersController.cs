﻿using System;
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

        [HttpGet("login/{email}")]
        public async Task<ActionResult<IList<User>>> Login([FromBody] FindUser findUser)
        {
            return Ok(await userService.GetUserAsync(findUser));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(Guid id)
        {
            return Ok(await userService.GetUserAsync(id));
        }

        [HttpPost]
        public async Task<ActionResult<User>> CreateUser([FromBody] CreateNewUser newUser)
        {
            return Ok(await userService.AddUserAsync(newUser));
        }
    }
}
