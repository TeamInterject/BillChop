using BillChopBE.DataAccessLayer.Models;
using BillChopBE.DataAccessLayer.Repositories.Interfaces;
using BillChopBE.Exceptions;
using BillChopBE.Services.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BillChopBE.Services
{
    public interface IUserService
    {
        Task<User> AddUserAsync(CreateNewUser newUser);
        Task<User> GetUserAsync(Guid id);
        Task<IList<User>> GetUsersAsync();
    }

    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;
        public UserService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task<User> GetUserAsync(Guid id)
        {
            var user = await userRepository.GetByIdAsync(id);
            return user ?? throw new NotFoundException($"User with id ({id}) does not exist");
        }

        public Task<IList<User>> GetUsersAsync()
        {
            return userRepository.GetAllAsync();
        }

        public Task<IList<User>> SearchForUsersAsync(string keyword)
        {
            return userRepository.SearchUsersAsync();
        }

        public Task<User> AddUserAsync(CreateNewUser newUser)
        {
            newUser.Validate(); //TODO: Silent validate and throw appropriate HttpException
            var user = newUser.ToUser();

            return userRepository.AddAsync(user);
        }
    }
}
