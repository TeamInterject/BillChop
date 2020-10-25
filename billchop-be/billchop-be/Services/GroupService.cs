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
        Task<Group> AddGroupAsync(CreateNewGroup newGroup);
        Task<Group> GetGroupAsync(Guid id);
        Task<IList<Group>> GetGroupsAsync();
        Task<Group> AddUserToGroupAsync(Guid groupId, Guid userId);
        Task<IList<Group>> GetGroupsOfUserAsync(Guid id);
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

        public Task<IList<Group>> GetGroupsAsync()
        {
            return groupRepository.GetAllAsync();
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
        
        public async Task<IList<Group>> GetGroupsOfUserAsync(Guid id)
        {
            var user = await userRepository.GetByIdAsync(id);
            if (user == null)
                throw new NotFoundException($"User with id {id} does not exist.");

            return user.Groups.ToList();
        }
    }  
}
