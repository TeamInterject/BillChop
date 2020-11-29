﻿using BillChopBE.DataAccessLayer.Repositories.Interfaces;
using BillChopBE.Exceptions;
using BillChopBE.Services.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Linq;
using BillChopBE.Services.Configurations;

namespace BillChopBE.Services
{
  public interface IUserService
    {
        Task<UserWithToken> LoginAsync(LoginDetails loginDetails);
        Task<IList<UserWithoutPassword>> GetUsersAsync();
        Task<IList<UserWithoutPassword>> SearchForUsersAsync(string keyword, Guid? exclusionGroupId, int top);
        Task<UserWithoutPassword> GetUserAsync(Guid id);
        Task<UserWithToken> AddUserAsync(CreateNewUser newUser);
    }

    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;
        private readonly JwtConfig config;

        public UserService(IUserRepository userRepository, JwtConfig config)
        {
            this.userRepository = userRepository;
            this.config = config;
        }

        public async Task<UserWithoutPassword> GetUserAsync(Guid id)
        {
            var user = await userRepository.GetByIdAsync(id);
            if (user == null) 
                throw new NotFoundException($"User with id ({id}) does not exist");

            return new UserWithoutPassword(user);
        }

        public async Task<UserWithToken> LoginAsync(LoginDetails loginDetails)
        {
            loginDetails.Validate();
            var hashed = Hasher.GetHashed(loginDetails.Password);
            var user = await userRepository.GetByEmailAndPassword(loginDetails.Email, hashed);
            if (user == null)
                throw new UnauthorizedException($"Username or password is incorrect");

            var claims = new[] 
            {
                new Claim(JwtRegisteredClaimNames.Sub, config.Subject),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                new Claim("Id", user.Id.ToString()),
                new Claim("Email", user.Email)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.Key));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var securityToken = new JwtSecurityToken(config.Issuer, config.Audience, claims, expires: DateTime.UtcNow.AddDays(1), signingCredentials: signIn);
            var token = new JwtSecurityTokenHandler().WriteToken(securityToken);

            return new UserWithToken(user, token);
        }

        public async Task<IList<UserWithoutPassword>> GetUsersAsync()
        {
            var users = await userRepository.GetAllAsync();
            return users
                .Select(u => new UserWithoutPassword(u))
                .ToList();
        }

        public async Task<IList<UserWithoutPassword>> SearchForUsersAsync(string keyword, Guid? exclusionGroupId, int top)
        {
            var users = await userRepository.SearchNameAndEmailAsync(keyword, exclusionGroupId, top);
            return users
                .Select(u => new UserWithoutPassword(u))
                .ToList();
        }

        public async Task<UserWithToken> AddUserAsync(CreateNewUser newUser)
        {
            newUser.Validate();
            var user = newUser.ToUser();

            user.Password = Hasher.GetHashed(user.Password);

            var addedUser = await userRepository.AddAsync(user);
            return await LoginAsync(new LoginDetails() { Email = addedUser.Email, Password = newUser.Password });
        }
    }
}
