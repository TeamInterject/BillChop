using System;
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
    public class GroupsController : ControllerBase
    {
        private readonly IGroupService groupService;

        public GroupsController(IGroupService groupService)
        {
            this.groupService = groupService;
        }
        [HttpGet]
        public async Task<ActionResult<IList<Group>>> GetGroups()
        {
            return Ok(await groupService.GetGroupsAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Group>> GetGroup(Guid id)
        {
            return Ok(await groupService.GetGroupAsync(id));
        }

        [HttpPost]
        public async Task<ActionResult<Group>> CreateGroup([FromBody] CreateNewGroup newGroup)
        {
            return Ok(await groupService.AddGroupAsync(newGroup));
        }

        [HttpPost("{groupId}/add_user/{userId}")]
        public async Task<ActionResult<Group>> AddUserToGroup(Guid groupId, Guid userId)
        {
            return Ok(await groupService.AddUserToGroupAsync(groupId, userId));
        }
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<Group>> GetGroupsOfUser(Guid userId)
        {
            return Ok(await groupService.GetGroupsOfUserAsync(userId));
        }
    }

}
