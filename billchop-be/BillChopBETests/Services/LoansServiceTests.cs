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
using BillChopBE.DataAccessLayer.Filters.Factories;
using BillChopBE.DataAccessLayer.Filters;

namespace BillChopBETests.Services
{
    class LoansServiceTests
    {
        protected class LoansServiceSutBuilder : ISutBuilder<LoanService>
        {
            internal ILoanRepository LoanRepository { get; set; } = A.Fake<ILoanRepository>();
            internal ILoanDbFilterFactory LoanDbFilterFactory { get; set; } = A.Fake<ILoanDbFilterFactory>();

            public LoanService CreateSut()
            {
                return new LoanService(LoanRepository, LoanDbFilterFactory);
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
            public CreateNewBill CreateNewBill(string name, decimal total, Guid? groupContextId = null, Guid? loanerId = null, DateTime? creationTime = null)
            {
                return new CreateNewBill()
                {
                    Name = name,
                    Total = total,
                    GroupContextId = groupContextId ?? Guid.NewGuid(),
                    LoanerId = loanerId ?? Guid.NewGuid(),
                    CreationTime = creationTime ?? DateTime.Now,
                };
            }
            public List<Loan> Setup()
            {
                var sutBuilder = new LoansServiceSutBuilder();
                var group = sutBuilder.CreateGroupWithUsers("Test group", 0);
                var loaner = sutBuilder.CreateUser(name: "loaner", group: group);
                var loanee = sutBuilder.CreateUser(name: "loanee", group: group);
                var user1 = sutBuilder.CreateUser(name: "user1", group: group);
                var user2 = sutBuilder.CreateUser(name: "user2", group: group);
                var groupUsers = new List<User>
                {
                    loaner,
                    loanee,
                    user1,
                    user2
                };
                var billToCreate = sutBuilder.CreateNewBill("Shopping", 120, group.Id, loaner.Id);
                var bill = new Bill()
                {
                    Id = Guid.NewGuid(),
                    Name = billToCreate.Name,
                    Total = billToCreate.Total,
                    LoanerId = loaner.Id,
                    Loaner = loaner,
                    GroupContextId = group.Id,
                    GroupContext = group,
                };
                var loans = groupUsers
                    .Select((user, index) => new Loan()
                    {
                        BillId = bill.Id,
                        Bill = bill,
                        LoaneeId = user.Id,
                        Loanee = user,
                        Amount = 30
                    }).ToList();
                return loans;
            }
        }

        [Test]
        public async Task GetProvidedLoansAsync_WhenProvidedLoansExist_ShouldReturnProvidedLoans()
        {
            //Arrange
            var sutBuilder = new LoansServiceSutBuilder();
            var loans = sutBuilder.Setup();
            List<Loan> filteredLoans = loans.Where(u => u.LoaneeId != u.Bill.LoanerId).ToList();
            var loanService = sutBuilder.CreateSut();

            var loanerId = loans[0].Bill.LoanerId;
            var groupId = loans[0].Loanee.Groups[0].Id;
            var startTime = DateTime.Now.AddMilliseconds(-1000);
            var endTime = DateTime.Now.AddMilliseconds(1000);

            var expectedFilter = A.Fake<LoanDbFilter>();
            A.CallTo(() => sutBuilder.LoanDbFilterFactory.Create(A<LoanFilterInfo>.That.Matches(passedInfo =>
                passedInfo.LoanerId == loanerId &&
                passedInfo.GroupId == groupId &&
                passedInfo.StartTime == startTime &&
                passedInfo.EndTime == endTime &&
                passedInfo.LoaneeId == null
            ))).Returns(expectedFilter);

            A.CallTo(() => sutBuilder.LoanRepository.GetAllAsync(expectedFilter))
                .Returns(filteredLoans);

            //Act
            var resultLoans = await loanService.GetProvidedLoansAsync(loanerId, groupId, startTime, endTime);

            //Assert
            resultLoans.ShouldBe(filteredLoans);
        }

        [Test]
        public async Task GetReceivedLoansAsync_WhenReceivedLoansExist_ShouldReturnReceivedLoans()
        {
            //Arrange
            var sutBuilder = new LoansServiceSutBuilder();
            var loans = sutBuilder.Setup();
            List<Loan> filteredLoans = loans.Where(u => u.LoaneeId == loans[1].LoaneeId).ToList();
            var loanService = sutBuilder.CreateSut();

            var loaneeId = loans[1].LoaneeId;
            var groupId = loans[0].Loanee.Groups[0].Id;
            var startTime = DateTime.Now.AddMilliseconds(-1000);
            var endTime = DateTime.Now.AddMilliseconds(1000);

            var expectedFilter = A.Fake<LoanDbFilter>();
            A.CallTo(() => sutBuilder.LoanDbFilterFactory.Create(A<LoanFilterInfo>.That.Matches(passedInfo =>
                passedInfo.LoanerId == null &&
                passedInfo.GroupId == groupId &&
                passedInfo.StartTime == startTime &&
                passedInfo.EndTime == endTime &&
                passedInfo.LoaneeId == loaneeId
            ))).Returns(expectedFilter);

            A.CallTo(() => sutBuilder.LoanRepository.GetAllAsync(expectedFilter))
                .Returns(filteredLoans);

            //Act
            var resultLoans = await loanService.GetReceivedLoansAsync(loaneeId, groupId, startTime, endTime);

            //Assert
            resultLoans.ShouldBe(filteredLoans);
        }

        [Test]
        public async Task GetSelfLoansAsync_WhenSelfLoansExist_ShouldReturnSelfLoans()
        {
            //Arrange
            var sutBuilder = new LoansServiceSutBuilder();
            var loans = sutBuilder.Setup();
            List<Loan> filteredLoans = loans.Where(u => u.LoaneeId == u.Bill.LoanerId).ToList();
            var loanService = sutBuilder.CreateSut();

            var loanerId = loans[0].Bill.LoanerId;
            var loaneeId = loans[0].Bill.LoanerId;
            var groupId = loans[0].Loanee.Groups[0].Id;
            var startTime = DateTime.Now.AddMilliseconds(-1000);
            var endTime = DateTime.Now.AddMilliseconds(1000);

            var expectedFilter = A.Fake<LoanDbFilter>();
            A.CallTo(() => sutBuilder.LoanDbFilterFactory.Create(A<LoanFilterInfo>.That.Matches(passedInfo =>
                passedInfo.LoanerId == loanerId &&
                passedInfo.GroupId == groupId &&
                passedInfo.StartTime == startTime &&
                passedInfo.EndTime == endTime &&
                passedInfo.LoaneeId == loaneeId
            ))).Returns(expectedFilter);

            A.CallTo(() => sutBuilder.LoanRepository.GetAllAsync(expectedFilter))
                .Returns(filteredLoans);

            //Act
            var resultLoans = await loanService.GetSelfLoansAsync(loaneeId, groupId, startTime, endTime);

            //Assert
            resultLoans.ShouldBe(filteredLoans);
        }
    }
}
