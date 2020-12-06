using NUnit.Framework;
using FakeItEasy;
using Shouldly;
using BillChopBE.Services.Models;
using System;
using BillChopBE.DataAccessLayer.Models;
using System.Collections.Generic;
using System.Linq;
using BillChopBE.DataAccessLayer.Filters;

namespace BillChopBETests.DataAccessLayer.Filters
{
    class LoanDbFilterTests
    {
        protected class LoanDbFilterSutBuilder : ISutBuilder<LoanDbFilter>
        {
            internal LoanFilterInfo LoanFilterInfo { get; set; } = new LoanFilterInfo();

            public LoanDbFilter CreateSut()
            {
                return new LoanDbFilter(LoanFilterInfo);
            }

            public List<Loan> Setup()
            {
                var sutBuilder = new LoanDbFilterSutBuilder();
                var bill = new Bill()
                {
                    Id = Guid.NewGuid(),
                    Name = "Shopping",
                    Total = 90,
                    LoanerId = Guid.NewGuid(),
                    Loaner = new User(),
                    GroupContextId = Guid.NewGuid(),
                    GroupContext = new Group(),
                    CreationTime = DateTime.Now.AddMilliseconds(2000)
                };
                var bill2 = new Bill()
                {
                    Id = Guid.NewGuid(),
                    Name = "Shopping2",
                    Total = 140,
                    LoanerId = bill.LoanerId,
                    Loaner = bill.Loaner,
                    GroupContextId = Guid.NewGuid(),
                    GroupContext = new Group(),
                    CreationTime = DateTime.Now.AddMilliseconds(-2000)
                };
                var bill3 = new Bill()
                {
                    Id = Guid.NewGuid(),
                    Name = "Shopping3",
                    Total = 200,
                    LoanerId = Guid.NewGuid(),
                    Loaner = new User(),
                    GroupContextId = Guid.NewGuid(),
                    GroupContext = new Group(),
                    CreationTime = DateTime.Now.AddMilliseconds(2000)
                };
                var loans = new List<Loan>()
                {
                    new Loan()
                    {
                        BillId = bill.Id,
                        Bill = bill,
                        LoaneeId = bill.LoanerId,
                        Loanee = bill.Loaner,
                        Amount = 30
                    },
                    new Loan()
                    {
                        BillId = bill.Id,
                        Bill = bill,
                        LoaneeId = Guid.NewGuid(),
                        Loanee = new User(),
                        Amount = 30
                    },
                    new Loan()
                    {
                        BillId = bill.Id,
                        Bill = bill,
                        LoaneeId = Guid.NewGuid(),
                        Loanee = new User(),
                        Amount = 40
                    },
                    new Loan()
                    {
                        BillId = bill2.Id,
                        Bill = bill2,
                        LoaneeId = bill2.LoanerId,
                        Loanee = bill2.Loaner,
                        Amount = 60
                    },
                    new Loan()
                    {
                        BillId = bill2.Id,
                        Bill = bill2,
                        LoaneeId = Guid.NewGuid(),
                        Loanee = new User(),
                        Amount = 80
                    },
                    new Loan()
                    {
                        BillId = bill3.Id,
                        Bill = bill3,
                        LoaneeId = bill3.LoanerId,
                        Loanee = bill3.Loaner,
                        Amount = 100
                    },
                    new Loan()
                    {
                        BillId = bill3.Id,
                        Bill = bill3,
                        LoaneeId = Guid.NewGuid(),
                        Loanee = new User(),
                        Amount = 100
                    },
                };
                return loans;                 
            }
        }

        [Test]
        public void LoanDbFilter_WhenLoanerHasProvidedLoans_ShouldReturnOnlyProvidedLoans()
        {
            //Arrange           
            var sutBuilder = new LoanDbFilterSutBuilder();
            var loans = sutBuilder.Setup();
            var filterInfo = new LoanFilterInfo
            {
                LoanerId = loans[0].LoaneeId,
            };
            var filter = new LoanDbFilter(filterInfo);

            var queryableLoans = loans.AsQueryable();
            loans = loans.Where(u => u.Bill.LoanerId == loans[0].Bill.LoanerId).ToList();

            //Act
            var result = filter.ApplyFilter(queryableLoans).ToList();

            //Assert
            result.ShouldBe(loans);
        }
        [Test]
        public void LoanDbFilter_WhenLoaneeHasRecievedLoans_ShouldReturnOnlyReceivedLoans()
        {
            //Arrange           
            var sutBuilder = new LoanDbFilterSutBuilder();
            var loans = sutBuilder.Setup();
            var filterInfo = new LoanFilterInfo
            {
                LoaneeId = loans[1].LoaneeId,
            };
            var filter = new LoanDbFilter(filterInfo);

            var queryableLoans = loans.AsQueryable();
            loans = loans.Where(u => u.LoaneeId == loans[1].LoaneeId).ToList();

            //Act
            var result = filter.ApplyFilter(queryableLoans).ToList();

            //Assert
            result.ShouldBe(loans);
        }

        [Test]
        public void LoanDbFilter_WhenGroupHasBills_ShouldReturnAllGroupLoans()
        {
            //Arrange           
            var sutBuilder = new LoanDbFilterSutBuilder();
            var loans = sutBuilder.Setup();
            var filterInfo = new LoanFilterInfo
            {
                GroupId = loans[0].Bill.GroupContextId,
            };
            var filter = new LoanDbFilter(filterInfo);

            var queryableLoans = loans.AsQueryable();
            loans = loans.Where(u => u.Bill.GroupContextId == loans[0].Bill.GroupContextId).ToList();

            //Act
            var result = filter.ApplyFilter(queryableLoans).ToList();

            //Assert
            result.ShouldBe(loans);
        }

        [Test]
        public void LoanDbFilter_WhenThereAreLoansWereCreatedAfterCertainTime_ShouldReturnAllLoansCreatedAfterCertainTime()
        {
            //Arrange           
            var sutBuilder = new LoanDbFilterSutBuilder();
            var loans = sutBuilder.Setup();
            var filterInfo = new LoanFilterInfo
            {
                StartTime = DateTime.Now.AddMilliseconds(-1000),
            };
            var filter = new LoanDbFilter(filterInfo);

            var queryableLoans = loans.AsQueryable();
            loans = loans.Where(u => u.Bill.CreationTime > filterInfo.StartTime).ToList();

            //Act
            var result = filter.ApplyFilter(queryableLoans).ToList();

            //Assert
            result.ShouldBe(loans);
        }

        [Test]
        public void LoanDbFilter_WhenThereAreLoansWereCreatedBeforeCertainTime_ShouldReturnAllLoansCreatedBeforeCertainTime()
        {
            //Arrange           
            var sutBuilder = new LoanDbFilterSutBuilder();
            var loans = sutBuilder.Setup();
            var filterInfo = new LoanFilterInfo
            {
                EndTime = DateTime.Now.AddMilliseconds(1000),
            };
            var filter = new LoanDbFilter(filterInfo);

            var queryableLoans = loans.AsQueryable();
            loans = loans.Where(u => u.Bill.CreationTime < filterInfo.EndTime).ToList();

            //Act
            var result = filter.ApplyFilter(queryableLoans).ToList();

            //Assert
            result.ShouldBe(loans);
        }
    }
}
