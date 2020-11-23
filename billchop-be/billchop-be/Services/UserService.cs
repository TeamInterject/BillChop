using BillChopBE.DataAccessLayer;
using BillChopBE.DataAccessLayer.Models;
using BillChopBE.DataAccessLayer.Repositories.Interfaces;
using BillChopBE.Exceptions;
using BillChopBE.Services.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BillChopBE.Services
{
    public interface IUserService
    {
        Task<User> AddUserAsync(CreateNewUser newUser);
        Task<User> GetUserAsync(Guid id);
        Task<IList<User>> GetUsersAsync();
        Task<IList<User>> SearchForUsersAsync(string keyword, Guid? exclusionGroupId, int top);
        Task<User> LoginAsync(LoginDetails loginDetails);
    }

    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;
        private readonly IConfiguration configuration;
        private readonly BillChopContext context;
        protected DbContext DbContext => context;
        protected DbSet<User> DbSet => context.Users;
        public UserService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task<User> GetUserAsync(Guid id)
        {
            var user = await userRepository.GetByIdAsync(id);
            return user ?? throw new NotFoundException($"User with id ({id}) does not exist");
        }

        public async Task<User> LoginAsync(LoginDetails loginDetails)
        {
            loginDetails.Validate();
            var user = await userRepository.GetByEmailAsync(loginDetails.Email);

            return user ?? throw new NotFoundException($"User with email ({loginDetails.Email}) does not exist");
        }

        public Task<IList<User>> GetUsersAsync()
        {
            return userRepository.GetAllAsync();
        }

        public Task<IList<User>> SearchForUsersAsync(string keyword, Guid? exclusionGroupId, int top)
        {
            return userRepository.SearchNameAndEmailAsync(keyword, exclusionGroupId, top);
        }

        public Task<User> AddUserAsync(CreateNewUser newUser)
        {
            newUser.Validate(); //TODO: Silent validate and throw appropriate HttpException
            var user = newUser.ToUser();

            return userRepository.AddAsync(user);
        }
    }
}
