using BillChopBE.DataAccessLayer.Filters;
using BillChopBE.DataAccessLayer.Filters.Factories;
using NUnit.Framework;
using Shouldly;
using System;


namespace BillChopBETests.DataAccessLayer.Filters.Factories
{
    class LoanDbFilterFactoryTests
    {
        [Test]
        public void Create_WhenFilterInfoIsPassed_ShouldReturnLoanDbFilter()
        {
            //Arrange           
            var loanDbFilterFactory = new LoanDbFilterFactory();
            var filterInfo = new LoanFilterInfo
            {
                LoanerId = Guid.NewGuid(),
                GroupId = Guid.NewGuid(),
                StartTime = DateTime.Now.AddMilliseconds(-1000),
                EndTime = DateTime.Now.AddMilliseconds(1000),
                LoaneeId = Guid.NewGuid(),
            };

            //Act
            var result = loanDbFilterFactory.Create(filterInfo);

            //Assert
            result.GetType().ShouldBe(typeof(LoanDbFilter));
        }
    }
}
