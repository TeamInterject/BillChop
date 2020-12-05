using BillChopBE.Services;
using BillChopBE.Exceptions;
using NUnit.Framework;
using FakeItEasy;
using Shouldly;
using BillChopBE.DataAccessLayer.Repositories.Interfaces;
using BillChopBE.Services.Models;
using System;
using System.Threading.Tasks;
using BillChopBE.DataAccessLayer.Models;
using Bogus;
using System.Collections.Generic;
using ValidationException = System.ComponentModel.DataAnnotations.ValidationException;
using BillChopBE.Services.Configurations;
using ProjectPortableTools.Extensions;
using System.Linq;

namespace BillChopBETests
{
    public class UserServiceTests
    {
        protected class UserServiceSutBuilder : ISutBuilder<UserService>
        {
            internal IUserRepository UserRepository { get; set; } = A.Fake<IUserRepository>();

            internal JwtConfig Config { get; set; } = new JwtConfig() 
            {
                Key = Guid.NewGuid().ToString(),
                Issuer = "SomeIssuer",
                Audience = "SomeAudience",
                Subject = "SomeSubject"
            };

            public UserService CreateSut()
            {
                return new UserService(UserRepository, Config);
            }

            public User CreateUser(Group? group = null, string? name = null, string? email = null, string? password = null)
            {
                var faker = new Faker();
                var user = new User()
                {
                    Name = name ?? faker.Person.FullName,
                    Groups = new List<Group>(),
                    Id = Guid.NewGuid(),
                    Email = email ?? faker.Person.Email,
                    Password = password ?? faker.Person.Email
                };

                if (group != null)
                    user.Groups.Add(group);

                return user;
            }

            public List<UserWithoutPassword> CovertUsersToUsersWithoutPassword(List<User> users)
            {
                return users.Select(u => new UserWithoutPassword(u)).ToList();
            }

            public List<User> CreateUsers(int userCount)
            {
                return userCount.Select((_) => CreateUser()).ToList();
            }

            public LoginDetails CreateLoginDetails(string? email = null, string? password = null)
            {
                var faker = new Faker();
                var loginDetails = new LoginDetails()
                {
                    Email = email ?? faker.Person.Email,
                    Password = password ?? faker.Person.Email
                };

                return loginDetails;
            }
        }

        [Test]
        [TestCase("Test", "test@test.com", "test123")]
        [TestCase("John K", "john@gmail.com", "test123")]
        [TestCase("Alice", "alice@yahoo.com", "test123")]
        public async Task LoginAsync_WhenLoginDetailsAreCorrect_ShouldLoginUser(string name, string email, string password)
        {
            //Arrange
            var sutBuilder = new UserServiceSutBuilder();
            var user = sutBuilder.CreateUser(name: name, email: email, password: password);
            var loginDetails = sutBuilder.CreateLoginDetails(email: email, password: password);
            var userService = sutBuilder.CreateSut();

            A.CallTo(() => sutBuilder.UserRepository.GetByEmailAndPasswordAsync(email, A<string>._))
                .Returns(user);

            //Act
            var resultLogin = await userService.LoginAsync(loginDetails);

            //Assert
            resultLogin.Name.ShouldBe(name);
            resultLogin.Email.ShouldBe(email);
        }

        [Test]
        [TestCase("Test", "test@test.com", "test123")]
        [TestCase("John K", "john@gmail.com", "test123")]
        [TestCase("Alice", "alice@yahoo.com", "test123")]
        public void LoginAsync_WhenUserDoesNotExist_ShouldThrow(string name, string email, string password)
        {
            //Arrange
            var sutBuilder = new UserServiceSutBuilder();
            var user = sutBuilder.CreateUser(name: name, email: "wrong@email.com", password: password);
            var loginDetails = sutBuilder.CreateLoginDetails(email: email, password: password);
            var userService = sutBuilder.CreateSut();

            A.CallTo(() => sutBuilder.UserRepository.GetByIdAsync(user.Id))
               .Returns<User?>(null);

            A.CallTo(() => sutBuilder.UserRepository.GetByEmailAndPasswordAsync(email, A<string>._))
               .Returns<User?>(null);

            //Act & Assert
            var exception = Assert.ThrowsAsync<UnauthorizedException>(async() => await userService.LoginAsync(loginDetails));
            exception.Message.ShouldBe($"Username or password is incorrect");
        }

        [Test]
        [TestCase("Test", "@test.com", "test123")]
        [TestCase("John K", "john@gmailom", "test123")]
        [TestCase("Alice", "alicyahoo.com", "test123")]
        public void LoginAsync_WhenLoginDetailsAreIncorrect_ShouldThrow(string name, string email, string password)
        {
            //Arrange
            var sutBuilder = new UserServiceSutBuilder();
            var user = sutBuilder.CreateUser(name: name, email: email, password: password);
            var loginDetails = sutBuilder.CreateLoginDetails(email: email, password: password);
            var userService = sutBuilder.CreateSut();

            //Act & Assert
            var exception = Assert.ThrowsAsync<ValidationException>(async () => await userService.LoginAsync(loginDetails));
        }


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

        [Test]
        [TestCase("Test", "@test.com")]
        [TestCase("John K", "john@gmailom")]
        [TestCase("Alice", "alicyahoo.com")]
        public void AddUserAsync_WhenEmailIsWrong_ShouldThrow(string name, string email)
        {
            //Arrange
            var sutBuilder = new UserServiceSutBuilder();
            var user = sutBuilder.CreateUser(name: name, email: email);
            var user2 = new CreateNewUser
            {
                Email = email,
                Name = name
            };
            var userService = sutBuilder.CreateSut();

            //Act & Assert
            var exception = Assert.ThrowsAsync<ValidationException>(async () => await userService.AddUserAsync(user2));
        }

        [Test]
        [TestCase("Test", "test@test.com", "test123!")]
        [TestCase("John K", "john@gmail.com", "test123!")]
        [TestCase("Alice", "alice@yahoo.com", "test123!")]
        public async Task AddUserAsync_WhenAllInformationIsCorrect_ShouldReturnUserWithToken(string name, string email, string password)
        {
            //Arrange
            /*var sutBuilder = new UserServiceSutBuilder();
            var user = sutBuilder.CreateUser(name: name, email: email);
            var user2 = new CreateNewUser
            {
                Email = email,
                Name = name
            };
            var userService = sutBuilder.CreateSut();*/
            var sutBuilder = new UserServiceSutBuilder();
            
            var loginDetails = sutBuilder.CreateLoginDetails(email: email, password: password);
            var user2 = new CreateNewUser
            {
                Email = email,
                Name = name,
                Password = password,
            };
            var user = user2.ToUser();
            var userService = sutBuilder.CreateSut();

            A.CallTo(() => sutBuilder.UserRepository.GetByEmailAndPasswordAsync(email, Hasher.GetHashed(password)))
                .Returns(user);

            A.CallTo(() => sutBuilder.UserRepository.AddAsync(A<User>.That.Matches(passedUser => passedUser.Name == user.Name && 
            passedUser.Password == Hasher.GetHashed(user.Password) && 
            passedUser.Email == user.Email)))
                .Returns(user);

            //Act
            var resultLogin = await userService.AddUserAsync(user2);

            //Assert
            resultLogin.Name.ShouldBe(name);
            resultLogin.Email.ShouldBe(email);

        }

        [Test]
        public async Task GetUsersAsync_WhenUsersExists_ShouldReturnAllUsers()
        {
            //Arrange
            var sutBuilder = new UserServiceSutBuilder();   
            List<User> users = sutBuilder.CreateUsers(5);
            var usersWithoutPassword = sutBuilder.CovertUsersToUsersWithoutPassword(users);
            var userService = sutBuilder.CreateSut();

            A.CallTo(() => sutBuilder.UserRepository.GetAllAsync(null))
               .Returns(users);

            //Act
            IList<UserWithoutPassword> resultLogin = await userService.GetUsersAsync();

            //Assert
            resultLogin.ShouldBe(usersWithoutPassword);
        }

        [Test]
        public async Task SearchFurUsersAsync_WhenUsersMatchKeyword_ShouldReturnMatchedUsers()
        {
            //Arrange
            var sutBuilder = new UserServiceSutBuilder();
            List<User> returnedUsers = new List<User>
            {
                sutBuilder.CreateUser(name: "Jack", email: "jack@gg.com", password: "password"),
                sutBuilder.CreateUser(name: "James", email: "james@gg.com", password: "password"),
            };
            var usersWithoutPassword = sutBuilder.CovertUsersToUsersWithoutPassword(returnedUsers);
            var userService = sutBuilder.CreateSut();

            A.CallTo(() => sutBuilder.UserRepository.SearchNameAndEmailAsync("ja", null, 5))
               .Returns(returnedUsers);

            //Act
            IList<UserWithoutPassword> resultLogin = await userService.SearchForUsersAsync("ja", null, 5);

            //Assert
            resultLogin.ShouldBe(usersWithoutPassword);
        }
    }
}