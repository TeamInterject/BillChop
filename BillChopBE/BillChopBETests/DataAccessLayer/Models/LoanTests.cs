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
    class LoanTests
    {
        [Test]
        public void Loan_WhenLoanInfoIsPassed_ShouldReturnLoanObject()
        {
            //Arrange    
            var id = Guid.NewGuid();
            var amount = 100;
            var loaneeId = Guid.NewGuid();
            var loanee = A.Fake<User>();
            var billId = Guid.NewGuid();
            var bill = A.Fake<Bill>();

            //Act
            var loan = new Loan()
            {
                Id = id,
                Amount = amount,
                LoaneeId = loaneeId,
                Loanee = loanee,
                BillId = billId,
                Bill = bill,
            };

            //Assert
            loan.Id.ShouldBe(id);
            loan.Amount.ShouldBe(amount);
            loan.LoaneeId.ShouldBe(loaneeId);
            loan.Loanee.ShouldBe(loanee);
            loan.BillId.ShouldBe(billId);
            loan.Bill.ShouldBe(bill);
            loan.Loaner.ShouldBe(bill.Loaner);
        }
    }
}
