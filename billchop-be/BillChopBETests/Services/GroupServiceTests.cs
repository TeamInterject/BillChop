using BillChopBE.Services;
using BillChopBE.Exceptions;
using NUnit.Framework;
using FakeItEasy;
using Shouldly;
using BillChopBE.DataAccessLayer.Repositories.Interfaces;
using System;
using System.Threading.Tasks;
using BillChopBE.DataAccessLayer.Models;
using ProjectPortableTools.Extensions;
using Bogus;
using System.Collections.Generic;
using System.Linq;

namespace BillChopBETests
{
  public class GroupServiceTests
    {
        protected class GroupServiceSutBuilder : ISutBuilder<GroupService>
        {
            internal IGroupRepository GroupRepository { get; set; } = A.Fake<IGroupRepository>();
            internal IUserRepository UserRepository { get; set; } = A.Fake<IUserRepository>();

            public GroupService CreateSut()
            {
                return new GroupService(GroupRepository, UserRepository);
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
        }

        [Test]
        public async Task GetGroupsAsync_WhenUserExists_ShouldReturnUserGroups()
        {
            //Arrange
            var sutBuilder = new GroupServiceSutBuilder();
            var group = sutBuilder.CreateGroupWithUsers("Test", 6);
            var user = sutBuilder.CreateUser(group: group, name: "Test", email: "test@email.com");
            var groupService = sutBuilder.CreateSut();
            IList<Group> groupList = new List<Group>();
            groupList.Add(group);

            A.CallTo(() => sutBuilder.GroupRepository.GetByIdAsync(group.Id))
                .Returns(group);

            A.CallTo(() => sutBuilder.UserRepository.GetByIdAsync(user.Id))
                .Returns(user);

            A.CallTo(() => sutBuilder.GroupRepository.GetUserGroups(user.Id))
                .Returns(groupList);

            //Act
            IList<Group> resultGroup = await groupService.GetGroupsAsync(user.Id);

            //Assert
            resultGroup.ShouldBe(groupList);
        }

        [Test]
        public async Task GetGroupsAsync_WhenUserIdIsNotPassed_ShouldReturnGroups()
        {
            //Arrange
            var sutBuilder = new GroupServiceSutBuilder();
            var group = sutBuilder.CreateGroupWithUsers("Test", 6);
            var user = sutBuilder.CreateUser(name: "Test", email: "test@email.com");
            var groupService = sutBuilder.CreateSut();
            IList<Group> groupList = new List<Group>();
            groupList.Add(group);


            A.CallTo(() => sutBuilder.GroupRepository.GetAllAsync(null))
                .Returns(groupList);

            //Act
            var resultGroup = await groupService.GetGroupsAsync(null);

            //Assert
            resultGroup.ShouldBe(groupList);
        }

        [Test]
        public void GetGroupsAsync_WhenUserDoesNotExist_ShouldThrow()
        {
            //Arrange
            var sutBuilder = new GroupServiceSutBuilder();
            var group = sutBuilder.CreateGroupWithUsers("Test", 6);
            var groupService = sutBuilder.CreateSut();
            IList<Group> groupList = new List<Group>();
            groupList.Add(group);
            var userid = Guid.NewGuid();

            A.CallTo(() => sutBuilder.GroupRepository.GetByIdAsync(group.Id))
                .Returns(group);

            A.CallTo(() => sutBuilder.UserRepository.GetByIdAsync(userid))
                .Returns<User?>(null);

            //Act & Assert
            var exception = Assert.ThrowsAsync<NotFoundException>(async () => await groupService.GetGroupsAsync(userid));
            exception.Message.ShouldBe($"User with id {userid} does not exist.");
        }

        [Test]
        public async Task GetGroupAsync_WhenGroupExists_ShouldReturnGroup()
        {
            //Arrange
            var sutBuilder = new GroupServiceSutBuilder();
            var group = sutBuilder.CreateGroupWithUsers("Test", 6);
            var groupService = sutBuilder.CreateSut();

            A.CallTo(() => sutBuilder.GroupRepository.GetByIdAsync(group.Id))
                .Returns(group);

            //Act
            var resultGroup = await groupService.GetGroupAsync(group.Id);

            //Assert
            resultGroup.ShouldBe(group);
        }

        [Test]
        public void GetGroupAsync_WhenGroupDoesNotExist_ShouldThrow()
        {
            //Arrange
            var sutBuilder = new GroupServiceSutBuilder();
            var group = sutBuilder.CreateGroupWithUsers("Test", 6);
            var groupService = sutBuilder.CreateSut();

            A.CallTo(() => sutBuilder.GroupRepository.GetByIdAsync(group.Id))
                .Returns<Group?>(null);

            //Act & Assert
            var exception = Assert.ThrowsAsync<NotFoundException>(async () => await groupService.GetGroupAsync(group.Id));
            exception.Message.ShouldBe($"Group with id ({group.Id}) does not exist");
        }


        [Test]
        public async Task AddUserToGroupAsync_WhenUserAndGroupExists_ShouldReturnGroup()
        {
            //Arrange
            var sutBuilder = new GroupServiceSutBuilder();
            var group = sutBuilder.CreateGroupWithUsers("Test", 1);
            var user = sutBuilder.CreateUser(name: "Test", email: "test@email.com");
            var groupService = sutBuilder.CreateSut();

            A.CallTo(() => sutBuilder.GroupRepository.GetByIdAsync(group.Id))
                .Returns(group);

            A.CallTo(() => sutBuilder.UserRepository.GetByIdAsync(user.Id))
                .Returns(user);

            //Act
            var resultGroup = await groupService.AddUserToGroupAsync(group.Id, user.Id);

            //Assert
            resultGroup.ShouldBe(group);
        }
        [Test]
        public void AddUserToGroupAsync_WhenGroupDoesNotExist_ShouldThrow()
        {
            //Arrange
            var sutBuilder = new GroupServiceSutBuilder();
            var group = sutBuilder.CreateGroupWithUsers("Test", 1);
            var user = sutBuilder.CreateUser(name: "Test", email: "test@email.com");
            var groupService = sutBuilder.CreateSut();

            A.CallTo(() => sutBuilder.GroupRepository.GetByIdAsync(group.Id))
                .Returns<Group?>(null);

            A.CallTo(() => sutBuilder.UserRepository.GetByIdAsync(user.Id))
                .Returns(user);

            //Act & Assert
            var exception = Assert.ThrowsAsync<NotFoundException>(async () => await groupService.AddUserToGroupAsync(group.Id, user.Id));
            exception.Message.ShouldBe($"Group with id ({group.Id}) does not exist");
        }

        [Test]
        public void AddUserToGroupAsync_WhenUserDoesNotExist_ShouldThrow()
        {
            //Arrange
            var sutBuilder = new GroupServiceSutBuilder();
            var group = sutBuilder.CreateGroupWithUsers("Test", 1);
            var user = sutBuilder.CreateUser(name: "Test", email: "test@email.com");
            var groupService = sutBuilder.CreateSut();

            A.CallTo(() => sutBuilder.GroupRepository.GetByIdAsync(Guid.NewGuid()))
                .Returns<Group?>(group);

            A.CallTo(() => sutBuilder.UserRepository.GetByIdAsync(user.Id))
                .Returns<User?>(null);

            //Act & Assert
            var exception = Assert.ThrowsAsync<NotFoundException>(async () => await groupService.AddUserToGroupAsync(group.Id, user.Id));
            exception.Message.ShouldBe($"User with id {user.Id} does not exist.");
        }
    }
}