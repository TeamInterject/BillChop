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
    class PaymentDbFilterTests
    {
        protected class PaymentDbFilterSutBuilder : ISutBuilder<PaymentDbFilter>
        {
            internal PaymentFilterInfo PaymentFilterInfo { get; set; } = new PaymentFilterInfo();

            public PaymentDbFilter CreateSut()
            {
                return new PaymentDbFilter(PaymentFilterInfo);
            }

            public List<Payment> Setup()
            {
                var receiver = new User();
                var payer = new User();
                var group = new Group();
                var payments = new List<Payment>()
                {
                    new Payment()
                    {
                        Id = Guid.NewGuid(),
                        Amount = 225,
                        Receiver = receiver,
                        ReceiverId = receiver.Id,
                        Payer = new User(),
                        PayerId = Guid.NewGuid(),
                        GroupContextId = group.Id,
                        GroupContext = group,
                        CreationTime = DateTime.Now.AddMilliseconds(2000),
                    },
                    new Payment()
                    {
                        Id = Guid.NewGuid(),
                        Amount = 100,
                        Receiver = receiver,
                        ReceiverId = receiver.Id,
                        Payer = payer,
                        PayerId = payer.Id,
                        GroupContextId = Guid.NewGuid(),
                        GroupContext = new Group(),
                        CreationTime = DateTime.Now.AddMilliseconds(-2000),
                    },
                    new Payment()
                    {
                        Id = Guid.NewGuid(),
                        Amount = 45,
                        Receiver = new User(),
                        ReceiverId = Guid.NewGuid(),
                        Payer = payer,
                        PayerId = payer.Id,
                        GroupContextId = group.Id,
                        GroupContext = group,
                        CreationTime = DateTime.Now.AddMilliseconds(2000),
                    },
                };           
            return payments;                 
            }
        }

        [Test]
        public void PaymentDbFilter_WhenPayerHasPayments_ShouldReturnOnlyPayersPayments()
        {
            //Arrange           
            var sutBuilder = new PaymentDbFilterSutBuilder();
            var payments = sutBuilder.Setup();
            var filterInfo = new PaymentFilterInfo
            {
                PayerId = payments[0].PayerId,
            };
            var filter = new PaymentDbFilter(filterInfo);

            var queryablePayments = payments.AsQueryable();
            payments = payments.Where(u => u.PayerId == filterInfo.PayerId).ToList();

            //Act
            var result = filter.ApplyFilter(queryablePayments).ToList();

            //Assert
            result.ShouldBe(payments);
        }

        [Test]
        public void PaymentDbFilter_WhenReceiverHasPayments_ShouldReturnOnlyReceiversPayments()
        {
            //Arrange           
            var sutBuilder = new PaymentDbFilterSutBuilder();
            var payments = sutBuilder.Setup();
            var filterInfo = new PaymentFilterInfo
            {
                ReceiverId = payments[0].ReceiverId,
            };
            var filter = new PaymentDbFilter(filterInfo);

            var queryablePayments = payments.AsQueryable();
            payments = payments.Where(u => u.ReceiverId == filterInfo.ReceiverId).ToList();

            //Act
            var result = filter.ApplyFilter(queryablePayments).ToList();

            //Assert
            result.ShouldBe(payments);
        }

        [Test]
        public void PaymentDbFilter_WhenGroupHasPayments_ShouldReturnOnlyGroupsPayments()
        {
            //Arrange           
            var sutBuilder = new PaymentDbFilterSutBuilder();
            var payments = sutBuilder.Setup();
            var filterInfo = new PaymentFilterInfo
            {
                GroupId = payments[0].GroupContextId,
            };
            var filter = new PaymentDbFilter(filterInfo);

            var queryablePayments = payments.AsQueryable();
            payments = payments.Where(u => u.GroupContextId == payments[0].GroupContextId).ToList();

            //Act
            var result = filter.ApplyFilter(queryablePayments).ToList();

            //Assert
            result.ShouldBe(payments);
        }
      
        [Test]
        public void PaymentDbFilter_WhenThereArePaymentsCreatedAfterCertainTime_ShouldReturnAllPaymentsCreatedAfterThatTime()
        {
            //Arrange           
            var sutBuilder = new PaymentDbFilterSutBuilder();
            var payments = sutBuilder.Setup();
            var filterInfo = new PaymentFilterInfo
            {
                StartTime = DateTime.Now.AddMilliseconds(-1000),
            };
            var filter = new PaymentDbFilter(filterInfo);

            var queryablePayments = payments.AsQueryable();
            payments = payments.Where(u => u.CreationTime > filterInfo.StartTime).ToList();

            //Act
            var result = filter.ApplyFilter(queryablePayments).ToList();

            //Assert
            result.ShouldBe(payments);
        }

        [Test]
        public void PaymentDbFilter_WhenThereArePaymentsCreatedBeforeCertainTime_ShouldReturnAllPaymentsCreatedBeforeThatTime()
        {
            //Arrange           
            var sutBuilder = new PaymentDbFilterSutBuilder();
            var payments = sutBuilder.Setup();
            var filterInfo = new PaymentFilterInfo
            {
                EndTime = DateTime.Now.AddMilliseconds(1000),
            };
            var filter = new PaymentDbFilter(filterInfo);

            var queryablePayments = payments.AsQueryable();
            payments = payments.Where(u => u.CreationTime < filterInfo.EndTime).ToList();

            //Act
            var result = filter.ApplyFilter(queryablePayments).ToList();

            //Assert
            result.ShouldBe(payments);
        }
    }
}
