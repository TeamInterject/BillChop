using NUnit.Framework;
using FakeItEasy;
using Shouldly;
using System;
using BillChopBE.DataAccessLayer.Models;
using System.Collections.Generic;
using System.Linq;
using BillChopBE.DataAccessLayer.Filters;

namespace BillChopBETests.DataAccessLayer.Filters
{
    class BillDbFilterTests
    {
        protected class BillDbFilterSutBuilder : ISutBuilder<BillDbFilter>
        {
            internal BillFilterInfo BillFilterInfo { get; set; } = new BillFilterInfo();

            public BillDbFilter CreateSut()
            {
                return new BillDbFilter(BillFilterInfo);
            }

            public List<Bill> Setup()
            {
                var loanerId = Guid.NewGuid();
                var laoner = new User();
                var groupId = Guid.NewGuid();
                var group = new Group();
                var bills = new List<Bill>()
                {
                    new Bill()
                    {
                        Id = Guid.NewGuid(),
                        Name = "Shopping",
                        Total = 90,
                        LoanerId = loanerId,
                        Loaner = laoner,
                        GroupContextId = groupId,
                        GroupContext = group,
                        CreationTime = DateTime.Now.AddMilliseconds(2000)
                    },
                    new Bill()
                    {
                        Id = Guid.NewGuid(),
                        Name = "Shopping2",
                        Total = 140,
                        LoanerId = loanerId,
                        Loaner = laoner,
                        GroupContextId = groupId,
                        GroupContext = group,
                        CreationTime = DateTime.Now.AddMilliseconds(-2000)
                    },
                    new Bill()
                    {
                        Id = Guid.NewGuid(),
                        Name = "Shopping3",
                        Total = 200,
                        LoanerId = Guid.NewGuid(),
                        Loaner = new User(),
                        GroupContextId = Guid.NewGuid(),
                        GroupContext = new Group(),
                        CreationTime = DateTime.Now.AddMilliseconds(2000)
                    },
            };           
            return bills;                 
            }
        }

        [Test]
        public void BillDbFilter_WhenLoanerHasBills_ShouldReturnOnlyLoanersBills()
        {
            //Arrange           
            var sutBuilder = new BillDbFilterSutBuilder();
            var bills = sutBuilder.Setup();
            var filterInfo = new BillFilterInfo
            {
                LoanerId = bills[0].LoanerId,
            };
            var filter = new BillDbFilter(filterInfo);

            var queryableBills = bills.AsQueryable();
            bills = bills.Where(u => u.LoanerId == bills[0].LoanerId).ToList();

            //Act
            var result = filter.ApplyFilter(queryableBills).ToList();

            //Assert
            result.ShouldBe(bills);
        }

        [Test]
        public void BillDbFilter_WhenGroupHasBills_ShouldReturnOnlyGroupsBills()
        {
            //Arrange           
            var sutBuilder = new BillDbFilterSutBuilder();
            var bills = sutBuilder.Setup();
            var filterInfo = new BillFilterInfo
            {
                GroupId = bills[0].GroupContextId,
            };
            var filter = new BillDbFilter(filterInfo);

            var queryableBills = bills.AsQueryable();
            bills = bills.Where(u => u.GroupContextId == bills[0].GroupContextId).ToList();

            //Act
            var result = filter.ApplyFilter(queryableBills).ToList();

            //Assert
            result.ShouldBe(bills);
        }
      
        [Test]
        public void LoanDbFilter_WhenThereAreBillsWereCreatedAfterCertainTime_ShouldReturnAllBillsCreatedAfterCertainTime()
        {
            //Arrange           
            var sutBuilder = new BillDbFilterSutBuilder();
            var bills = sutBuilder.Setup();
            var filterInfo = new BillFilterInfo
            {
                StartTime = DateTime.Now.AddMilliseconds(-1000),
            };
            var filter = new BillDbFilter(filterInfo);

            var queryableBills = bills.AsQueryable();
            bills = bills.Where(u => u.CreationTime > filterInfo.StartTime).ToList();

            //Act
            var result = filter.ApplyFilter(queryableBills).ToList();

            //Assert
            result.ShouldBe(bills);
        }

        [Test]
        public void LoanDbFilter_WhenThereAreBillsWereCreatedBeforeCertainTime_ShouldReturnAllBillsCreatedBeforeCertainTime()
        {
            //Arrange           
            var sutBuilder = new BillDbFilterSutBuilder();
            var bills = sutBuilder.Setup();
            var filterInfo = new BillFilterInfo
            {
                EndTime = DateTime.Now.AddMilliseconds(1000),
            };
            var filter = new BillDbFilter(filterInfo);

            var queryableBills = bills.AsQueryable();
            bills = bills.Where(u => u.CreationTime < filterInfo.EndTime).ToList();

            //Act
            var result = filter.ApplyFilter(queryableBills).ToList();

            //Assert
            result.ShouldBe(bills);
        }
    }
}
