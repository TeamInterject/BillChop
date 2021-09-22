using BillChopBE.DataAccessLayer.Filters;
using BillChopBE.DataAccessLayer.Filters.Factories;
using NUnit.Framework;
using Shouldly;
using System;

namespace BillChopBETests.DataAccessLayer.Filters.Factories
{
    class BillDbFilterFactoryTests
    {
        [Test]
        public void Create_WhenFilterInfoIsPassed_ShouldReturnBillDbFilter()
        {
            //Arrange           
            var billDbFilterFactory = new BillDbFilterFactory();
            var filterInfo = new BillFilterInfo
            {
                StartTime = DateTime.Now.AddMilliseconds(-1000),
                EndTime = DateTime.Now.AddMilliseconds(1000),
                LoanerId = Guid.NewGuid(),
                GroupId = Guid.NewGuid(),   
            };

            //Act
            var result = billDbFilterFactory.Create(filterInfo);

            //Assert
            result.GetType().ShouldBe(typeof(BillDbFilter));
        }
    }
}
