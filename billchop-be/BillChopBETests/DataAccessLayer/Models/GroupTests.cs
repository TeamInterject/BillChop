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
    class GroupTests
    {
        [Test]
        public void Bill_WhenBillInfoIsPassed_ShouldReturnBillObject()
        {
            //Arrange    
            var id = Guid.NewGuid();
            var name = "Group one";
            var users = A.Fake<List<User>>();
            var bills = A.Fake<List<Bill>>();

            //Act
            var group = new Group()
            {
                Id = id,
                Name = name,
                Users = users,
                Bills = bills,
            };

            //Assert
            group.Id.ShouldBe(id);
            group.Name.ShouldBe(name);
            group.Users.ShouldBe(users);
            group.Bills.ShouldBe(bills);
        }
    }
}
