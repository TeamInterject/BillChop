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
    class BillTests
    {
        [Test]
        public void Bill_WhenBillInfoIsPassed_ShouldReturnBillObject()
        {
            //Arrange    
            var id = Guid.NewGuid();
            var name = "Shopping";
            var total = 100;
            var creationTime = DateTime.Now;
            var loanerId = Guid.NewGuid();
            var loaner = A.Fake<User>();
            var loans = A.Fake<List<Loan>>();
            var groupContextId = Guid.NewGuid();
            var groupContext = A.Fake<Group>();

            //Act
            var bill = new Bill()
            {
                Id = id,
                Name = name,
                Total = total,
                CreationTime = creationTime,
                LoanerId = loanerId,
                Loaner = loaner,
                Loans = loans,
                GroupContextId = groupContextId,
                GroupContext = groupContext
            };

            //Assert
            bill.Id.ShouldBe(id);
            bill.Name.ShouldBe(name);
            bill.Total.ShouldBe(total);
            bill.CreationTime.ShouldBe(creationTime);
            bill.LoanerId.ShouldBe(loanerId);
            bill.Loaner.ShouldBe(loaner);
            bill.Loans.ShouldBe(loans);
            bill.GroupContextId.ShouldBe(groupContextId);
            bill.GroupContext.ShouldBe(groupContext);
        }
    }
}
