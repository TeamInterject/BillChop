using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BillChopBE.Controllers.Models;
using BillChopBE.Services;
using BillChopBE.Services.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BillChopBE.Controllers
{
    [Authorize]
    [ApiController]
    [Produces("application/json")]
    [Route("api/groups")]
    public class GroupsController : ControllerBase
    {
        private readonly IGroupService groupService;
        private readonly IMapper mapper;

        public GroupsController(IGroupService groupService, IMapper mapper)
        {
            this.mapper = mapper;
            this.groupService = groupService;
        }

        [HttpGet]
        public async Task<ActionResult<IList<ApiGroup>>> GetGroups(Guid? userId)
        {
            var groups = await groupService.GetGroupsAsync(userId);
            return mapper.Map<List<ApiGroup>>(groups);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiGroup>> GetGroup(Guid id)
        {
            var group = await groupService.GetGroupAsync(id);
            return mapper.Map<ApiGroup>(group);
        }

        [HttpPost]
        public async Task<ActionResult<ApiGroup>> CreateGroup([FromBody] CreateNewGroup newGroupData)
        {
            var group = await groupService.AddGroupAsync(newGroupData);
            return mapper.Map<ApiGroup>(group);
        }

        [HttpPost("{groupId}/add-user/{userId}")]
        public async Task<ActionResult<ApiGroup>> AddUserToGroup(Guid groupId, Guid userId)
        {
            var group = await groupService.AddUserToGroupAsync(groupId, userId);
            return mapper.Map<ApiGroup>(group);
        }
    }
}
