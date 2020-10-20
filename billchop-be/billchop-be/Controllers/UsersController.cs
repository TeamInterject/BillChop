using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BillChopBE.DataAccessLayer.Models;
using BillChopBE.DataAccessLayer.Repositories.Interfaces;
using BillChopBE.Services;
using BillChopBE.Services.Models;
using Microsoft.AspNetCore.Mvc;

namespace BillChopBE.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/[controller]")]
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
