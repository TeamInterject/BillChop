using BillChopBE.DataAccessLayer.Models;
using BillChopBE.DataAccessLayer.Repositories.Interfaces;
using BillChopBE.Exceptions;
using BillChopBE.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BillChopBE.Services
{
    public interface IGroupService
    {
        Task<IList<Group>> GetGroupsAsync(Guid? userId);
        Task<Group> GetGroupAsync(Guid id);
        Task<Group> AddGroupAsync(CreateNewGroup newGroup);
        Task<Group> AddUserToGroupAsync(Guid groupId, Guid userId);
    }

    public class GroupService : IGroupService
    {
        private readonly IGroupRepository groupRepository;
        private readonly IUserRepository userRepository;

        public GroupService(IGroupRepository groupRepository, IUserRepository userRepository)
        {
            this.groupRepository = groupRepository;
            this.userRepository = userRepository;
        }

        public Task<IList<Group>> GetGroupsAsync(Guid? userId)
        {
            if (userId.HasValue)
                return GetGroupsOfUserAsync(userId.Value);

            return groupRepository.GetAllAsync();
        }

        private async Task<IList<Group>> GetGroupsOfUserAsync(Guid userId)
        {
            var user = await userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new NotFoundException($"User with id {userId} does not exist.");

            return await groupRepository.GetUserGroups(userId);
        }

        public async Task<Group> GetGroupAsync(Guid id)
        {
            var group = await groupRepository.GetByIdAsync(id);
            return group ?? throw new NotFoundException($"Group with id ({id}) does not exist");
        }       

        public Task<Group> AddGroupAsync(CreateNewGroup newGroup)
        {
            newGroup.Validate(); //TODO: Silent validate and throw appropriate HttpException
            var group = newGroup.ToGroup();

            return groupRepository.AddAsync(group);
        }

        public async Task<Group> AddUserToGroupAsync(Guid groupId, Guid userId)
        {
            var group = await GetGroupAsync(groupId);
            if (group == null)
                throw new NotFoundException($"Group with id {groupId} does not exist.");

            var user = await userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new NotFoundException($"User with id {userId} does not exist.");

            group.Users.Add(user);
            await groupRepository.SaveChangesAsync();

            return group;
        }
    }  
}
