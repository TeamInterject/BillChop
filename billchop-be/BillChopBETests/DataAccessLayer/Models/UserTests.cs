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
    class UserTests
    {
        [Test]
        public void User_WhenUserInfoIsPassed_ShouldReturnUserObject()
        {
            //Arrange    
            var id = Guid.NewGuid();
            var name = "test";
            var email = "test@test.com";
            var password = "password";
            var groups = A.Fake<List<Group>>();
            var loans = A.Fake<List<Loan>>();
            var bills = A.Fake<List<Bill>>();
            var paymentsMade = A.Fake<List<Payment>>();
            var paymentsReceived = A.Fake<List<Payment>>();


            //Act
            var payment = new User()
            {
                Id = id,
                Name = name,
                Email = email,
                Password = password,
                Groups = groups,
                Loans = loans,
                Bills = bills,
                PaymentsMade = paymentsMade,
                PaymentsReceived = paymentsReceived,
            };

            //Assert
            payment.Id.ShouldBe(id);
            payment.Name.ShouldBe(name);
            payment.Email.ShouldBe(email);
            payment.Password.ShouldBe(password);
            payment.Groups.ShouldBe(groups);
            payment.Loans.ShouldBe(loans);
            payment.Bills.ShouldBe(bills);
            payment.PaymentsMade.ShouldBe(paymentsMade);
            payment.PaymentsReceived.ShouldBe(paymentsReceived);
        }
    }
}
