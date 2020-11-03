using BillChopBE.Services;
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

namespace BillChopBETests
{
    public class BillServiceTests
    {
        protected class BillServiceSutBuilder : ISutBuilder<BillService>
        {
            internal IBillRepository BillRepository { get; set; } = A.Fake<IBillRepository>();
            internal ILoanRepository LoanRepository { get; set; } = A.Fake<ILoanRepository>();
            internal IGroupRepository GroupRepository { get; set; } = A.Fake<IGroupRepository>();
            internal IUserRepository UserRepository { get; set; } = A.Fake<IUserRepository>();

            public BillService CreateSut()
            {
                return new BillService(BillRepository, LoanRepository, GroupRepository, UserRepository);
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

                var faker = new Faker();
                group.Users = userCount.Select((_) => new User()
                {
                    Name = faker.Person.FullName,
                    Groups = new List<Group>() { group },
                    Id = Guid.NewGuid(),
                }).ToList();

                return group;
            }
        }

        [Test]
        [TestCase(100.05, 1)]
        [TestCase(50.30, 2)]
        [TestCase(99.99, 3)]
        [TestCase(99.01, 3)]
        [TestCase(8.07, 8)]
        public async Task CreateAndSplitBillAsync_WhenBillIsSplitInGroup_ShouldCreateBillWithEqualLoans(decimal total, int userCount)
        {
            //Arrange
            var sutBuilder = new BillServiceSutBuilder();
            var group = sutBuilder.CreateGroupWithUsers("Test group", userCount);
            var loaner = group.Users.First();

            A.CallTo(() => sutBuilder.GroupRepository.GetByIdAsync(group.Id))
                .Returns(group);

            A.CallTo(() => sutBuilder.UserRepository.GetByIdAsync(loaner.Id))
                .Returns(loaner);

            A.CallTo(() => sutBuilder.BillRepository.AddAsync(A<Bill>._))
                .ReturnsLazily((Bill bill) => bill);

            var createNewBill = sutBuilder.CreateNewBill("Test bill", total, group.Id, loaner.Id);
            var billService = sutBuilder.CreateSut();

            //Act
            var resultBill = await billService.CreateAndSplitBillAsync(createNewBill);

            //Assert
            resultBill.Total.ShouldBe(total);
            resultBill.Name.ShouldBe(createNewBill.Name);
            resultBill.Loaner.ShouldBe(loaner);
            resultBill.Loans.Count.ShouldBe(group.Users.Count);

            var billLoanees = resultBill.Loans.Select(loan => loan.Loanee);
            billLoanees.ShouldBe(group.Users);

            var loanTotal = resultBill.Loans.Sum(loan => loan.Amount);
            loanTotal.ShouldBe(total);

            //Explanation of ToZero: https://docs.microsoft.com/en-us/dotnet/api/system.midpointrounding?view=netcore-3.1#directed-rounding
            var expectedAmount = Math.Round(total / userCount, 2, MidpointRounding.ToZero);
            resultBill.Loans.ForEach((loan) =>
            {
                // Allow error of 1 cent
                loan.Amount.ShouldBeInRange(expectedAmount, expectedAmount + 0.01M);
            });
        }
    }
}