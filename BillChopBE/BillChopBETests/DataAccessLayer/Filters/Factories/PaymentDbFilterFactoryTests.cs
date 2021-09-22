using BillChopBE.DataAccessLayer.Filters;
using BillChopBE.DataAccessLayer.Filters.Factories;
using NUnit.Framework;
using Shouldly;
using System;


namespace BillChopBETests.DataAccessLayer.Filters.Factories
{
    class PaymentDbFilterFactoryTests
    {
        [Test]
        public void Create_WhenFilterInfoIsPassed_ShouldReturnPaymentDbFilter()
        {
            //Arrange           
            var paymentDbFilterFactory = new PaymentDbFilterFactory();
            var filterInfo = new PaymentFilterInfo
            {
                GroupId = Guid.NewGuid(),
                StartTime = DateTime.Now.AddMilliseconds(-1000),
                EndTime = DateTime.Now.AddMilliseconds(1000),
                PayerId = Guid.NewGuid(),
                ReceiverId = Guid.NewGuid(),
            };

            //Act
            var result = paymentDbFilterFactory.Create(filterInfo);

            //Assert
            result.GetType().ShouldBe(typeof(PaymentDbFilter));
        }
    }
}
