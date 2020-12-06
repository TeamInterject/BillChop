using BillChopBE.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using FakeItEasy;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shouldly;
using NUnit.Framework;

namespace BillChopBETests.DataAccessLayer.Models
{
    class PaymentTests
    {
        [Test]
        public void Payment_WhenPaymentInfoIsPassed_ShouldReturnPaymentObject()
        {
            //Arrange    
            var id = Guid.NewGuid();
            var amount = 100;
            var creationTime = DateTime.Now;
            var payerId = Guid.NewGuid();
            var payer = A.Fake<User>();
            var receiverId = Guid.NewGuid();
            var receiver = A.Fake<User>();
            var groupContextId = Guid.NewGuid();
            var groupContext = A.Fake<Group>();

            //Act
            var payment = new Payment()
            {
                Id = id,
                Amount = amount,
                CreationTime = creationTime,
                PayerId = payerId,
                Payer = payer,
                ReceiverId = receiverId,
                Receiver = receiver,
                GroupContextId = groupContextId,
                GroupContext = groupContext,
            };

            //Assert
            payment.Id.ShouldBe(id);
            payment.Amount.ShouldBe(amount);
            payment.CreationTime.ShouldBe(creationTime);
            payment.PayerId.ShouldBe(payerId);
            payment.Payer.ShouldBe(payer);
            payment.ReceiverId.ShouldBe(receiverId);
            payment.Receiver.ShouldBe(receiver);
            payment.GroupContextId.ShouldBe(groupContextId);
            payment.GroupContext.ShouldBe(groupContext);
        }
    }
}
