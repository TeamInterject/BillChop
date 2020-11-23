using BillChopBE.Services;
using BillChopBE.DataAccessLayer.Filters;
using BillChopBE.DataAccessLayer.Filters.Factories;
using BillChopBE.Exceptions;
using NUnit.Framework;
using FakeItEasy;
using Shouldly;
using BillChopBE.DataAccessLayer.Repositories.Interfaces;
using BillChopBE.Services.Models;
using System;
using System.Threading.Tasks;
using BillChopBE.DataAccessLayer.Models;
using ProjectPortableTools.Extensions;
using Bogus;
using System.Collections.Generic;
using System.Linq;
using ValidationException = System.ComponentModel.DataAnnotations.ValidationException;

namespace BillChopBETests
{
    public class UserServiceTests
    {
        protected class UserServiceSutBuilder : ISutBuilder<UserService>
        {
            internal IBillRepository BillRepository { get; set; } = A.Fake<IBillRepository>();
            internal IUserRepository UserRepository { get; set; } = A.Fake<IUserRepository>();
            internal IGroupRepository GroupRepository { get; set; } = A.Fake<IGroupRepository>();
            internal IBillDbFilterFactory BillDbFilterFactory { get; set; } = A.Fake<IBillDbFilterFactory>();

            public UserService CreateSut()
            {
                return new UserService(UserRepository);
            }

            public CreateNewBill CreateNewBill(string name, decimal total, Guid? groupContextId = null, Guid? loanerId = null) 
            {
                return new CreateNewBill()
                {
                    Name = name,
                    Total = total,
                    GroupContextId = groupContextId ?? Guid.NewGuid(),
                    LoanerId = loanerId ?? Guid.NewGuid(),
                };
            }

            public Group CreateGroupWithUsers(string name, int userCount, Guid? groupId = null)
            {
                var group = new Group()
                {
                    Id = groupId ?? Guid.NewGuid(),
                    Name = name,
                };

                group.Users = userCount.Select((_) => CreateUser(group)).ToList();
                return group;
            }

            public User CreateUser(Group? group = null, string? name = null, string? email = null)
            {
                var faker = new Faker();
                var user = new User()
                {
                    Name = name ?? faker.Person.FullName,
                    Groups = new List<Group>(),
                    Id = Guid.NewGuid(),
                    Email = email ?? faker.Person.Email
                };

                if (group != null)
                    user.Groups.Add(group);

                return user;
            }
            public LoginDetails CreateLoginDetails(string? email = null)
            {
                var faker = new Faker();
                var loginDetails = new LoginDetails()
                {
                    Email = email
                };

                return loginDetails;
            }
        }

        [Test]
        [TestCase("Test", "test@test.com")]
        [TestCase("John K", "john@gmail.com")]
        [TestCase("Alice", "alice@yahoo.com")]
        public void LoginAsync_WhenLoginDetailsAreCorrect_ShouldLoginUser(string name, string email)
        {
            //Arrange
            var sutBuilder = new UserServiceSutBuilder();
            var user = sutBuilder.CreateUser(name: name, email: email);
            var loginDetails = sutBuilder.CreateLoginDetails(email: email);
            var userService = sutBuilder.CreateSut();

            A.CallTo(() => sutBuilder.UserRepository.GetByIdAsync(user.Id))
                .Returns(user);

            A.CallTo(() => sutBuilder.UserRepository.GetByEmailAsync(loginDetails.Email))
                .Returns(user);

            //Act
            var resultLogin = userService.LoginAsync(loginDetails);

            //Assert
            resultLogin.Result.Name.ShouldBe(name);
            resultLogin.Result.Email.ShouldBe(email);
        }

        [Test]
        [TestCase("Test", "test@test.com")]
        [TestCase("John K", "john@gmail.com")]
        [TestCase("Alice", "alice@yahoo.com")]
        public void LoginAsync_WhenUserDoesNotExist_ShouldThrow(string name, string email)
        {
            //Arrange
            var sutBuilder = new UserServiceSutBuilder();
            var user = sutBuilder.CreateUser(name: name, email: "wrong@email.com");
            var loginDetails = sutBuilder.CreateLoginDetails(email: email);
            var userService = sutBuilder.CreateSut();

            A.CallTo(() => sutBuilder.UserRepository.GetByIdAsync(user.Id))
               .Returns<User?>(null);

            A.CallTo(() => sutBuilder.UserRepository.GetByEmailAsync(loginDetails.Email))
               .Returns<User?>(null);

            //Act & Assert
            var exception = Assert.ThrowsAsync<NotFoundException>(async () => await userService.LoginAsync(loginDetails));
            exception.Message.ShouldBe($"User with email ({loginDetails.Email}) does not exist");
        }

        [Test]
        [TestCase("Test", "@test.com")]
        [TestCase("John K", "john@gmailom")]
        [TestCase("Alice", "alicyahoo.com")]
        public void LoginAsync_WhenLoginDetailsAreIncorrect_ShouldThrow(string name, string email)
        {
            //Arrange
            var sutBuilder = new UserServiceSutBuilder();
            var user = sutBuilder.CreateUser(name: name, email: email);
            var loginDetails = sutBuilder.CreateLoginDetails(email: email);
            var userService = sutBuilder.CreateSut();

           /* A.CallTo(() => sutBuilder.UserRepository.GetByIdAsync(user.Id))
               .Returns(user);

            A.CallTo(() => sutBuilder.UserRepository.GetByEmailAsync(loginDetails.Email))
                .Returns(user);*/

            //Act & Assert
            var exception = Assert.ThrowsAsync<ValidationException>(async () => await userService.LoginAsync(loginDetails));
        }

        /* public async Task<User> GetUserAsync(Guid id)
        {
            var user = await userRepository.GetByIdAsync(id);
            return user ?? throw new NotFoundException($"User with id ({id}) does not exist");
        }*/

        [Test]
        [TestCase("Test", "test@test.com")]
        [TestCase("John K", "john@gmail.com")]
        [TestCase("Alice", "alice@yahoo.com")]
        public void GetUserAsync_WhenUserExists_ShouldReturnUser(string name, string email)
        {
            //Arrange
            var sutBuilder = new UserServiceSutBuilder();
            var user = sutBuilder.CreateUser(name: name, email: email);
            var userService = sutBuilder.CreateSut();
            
            A.CallTo(() => sutBuilder.UserRepository.GetByIdAsync(user.Id))
               .Returns(user);

            //Act
            var resultLogin = userService.GetUserAsync(user.Id);

            //Assert
            resultLogin.Result.Name.ShouldBe(name);
            resultLogin.Result.Email.ShouldBe(email);
        }

        [Test]
        [TestCase("Test", "test@test.com")]
        public void GetUserAsync_WhenUserDoesNotExists_ShouldThrow(string name, string email)
        {
            //Arrange
            var sutBuilder = new UserServiceSutBuilder();
            var user = sutBuilder.CreateUser(name: name, email: email);
            var userService = sutBuilder.CreateSut();
            var userId = Guid.NewGuid();

            A.CallTo(() => sutBuilder.UserRepository.GetByIdAsync(userId))
               .Returns<User?>(null);

            //Act & Assert
            var exception = Assert.ThrowsAsync<NotFoundException>(async () => await userService.GetUserAsync(userId));
            exception.Message.ShouldBe($"User with id ({userId}) does not exist");
        }

        /*public Task<User> AddUserAsync(CreateNewUser newUser)
        {
            newUser.Validate(); //TODO: Silent validate and throw appropriate HttpException
            var user = newUser.ToUser();

            return userRepository.AddAsync(user);
        }*/
        [Test]
        [TestCase("Test", "@test.com")]
        [TestCase("John K", "john@gmailom")]
        [TestCase("Alice", "alicyahoo.com")]
        public void AddUserAsync_WhenEmailIsWrong_ShouldThrow(string name, string email)
        {
            //Arrange
            var sutBuilder = new UserServiceSutBuilder();
            //var user = new CreateNewUser(name , email);
            var user = sutBuilder.CreateUser(name: name, email: email);
            var user2 = new CreateNewUser();
            user2.Email = email;
            user2.Name = name;
            var userService = sutBuilder.CreateSut();

           /* A.CallTo(() => sutBuilder.UserRepository.AddAsync(user))
               .Returns(user);*/

            //Act & Assert
            var exception = Assert.ThrowsAsync<ValidationException>(async () => await userService.AddUserAsync(user2));
        }

    }
}