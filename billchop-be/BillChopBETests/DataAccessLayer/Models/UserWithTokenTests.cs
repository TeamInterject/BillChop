using BillChopBE.DataAccessLayer.Models;
using BillChopBE.Services.Models;
using FakeItEasy;
using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillChopBETests.DataAccessLayer.Models
{
    class UserWithTokenTests
    {
        [Test]
        public void UserWithToken_WhenUserWithTokenInfoIsPassed_ShouldReturnUserWithTokenObject()
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
            var token = "token";

            //Act
            var user = new UserWithToken(new User(), token)
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
                Token = token,
            };

            //Assert
            user.Id.ShouldBe(id);
            user.Name.ShouldBe(name);
            user.Email.ShouldBe(email);
            user.Password.ShouldBe(password);
            user.Groups.ShouldBe(groups);
            user.Loans.ShouldBe(loans);
            user.Bills.ShouldBe(bills);
            user.PaymentsMade.ShouldBe(paymentsMade);
            user.PaymentsReceived.ShouldBe(paymentsReceived);
            user.Token.ShouldBe(token);
        }
    }
}