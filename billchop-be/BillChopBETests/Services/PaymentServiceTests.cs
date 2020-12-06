using BillChopBE.Services;
using BillChopBE.DataAccessLayer.Filters.Factories;
using NUnit.Framework;
using FakeItEasy;
using Shouldly;
using BillChopBE.DataAccessLayer.Repositories.Interfaces;
using BillChopBE.Services.Models;
using System;
using System.Threading.Tasks;
using BillChopBE.DataAccessLayer.Models;
using ProjectPortableTools.Extensions;
using Bogus;
using System.Collections.Generic;
using System.Linq;
using BillChopBE.DataAccessLayer.Filters;
using BillChopBE.Exceptions;

namespace BillChopBETests
{
    public class PaymentServiceTests
    {
        public class PaymentServiceTestData 
        {
            public CreateNewPayment NewPaymentData { get; } 
            public Group Group { get; }
            public User Payer { get; }
            public User Receiver { get; }
            public List<Loan> PayerLoansToReceiver { get; set; } = new List<Loan>();
            public List<Loan> ReceiverLoansToPayer { get; set; } = new List<Loan>();
            public List<Payment> PayerPaymentsToReceiver { get; set; } = new List<Payment>();
            public List<Payment> ReceiverPaymentsToPayer { get; set; } = new List<Payment>();
            public decimal PayerOwesReceiver { get; private set; } = 0;

            public PaymentServiceTestData(decimal amount, string groupName = "Test group")
            {
                Group = CreateGroupWithUsers(groupName, 2);
                Payer = Group.Users[0];
                Receiver = Group.Users[1];

                NewPaymentData = new CreateNewPayment() 
                {
                    PayerId = Payer.Id,
                    ReceiverId = Receiver.Id,
                    GroupContextId = Group.Id,
                    Amount = amount,
                };
            }

            public PaymentServiceTestData AddPayerLoanToReceiver(decimal amount) 
            {
                var loan = new Loan() 
                {
                    Id = Guid.NewGuid(),
                    LoaneeId = Receiver.Id,
                    Loanee = Receiver,
                    Bill = new Bill()  
                    {
                        LoanerId = Payer.Id,
                        Loaner = Payer,
                        GroupContext = Group,
                        GroupContextId = Group.Id
                    },
                    Amount = amount,
                };

                PayerLoansToReceiver.Add(loan);

                PayerOwesReceiver -= amount;
                
                return this;
            }

            public PaymentServiceTestData AddReceiverLoanToPayer(decimal amount) 
            {
                var loan = new Loan() 
                {
                    Id = Guid.NewGuid(),
                    LoaneeId = Payer.Id,
                    Loanee = Payer,
                    Bill = new Bill()  
                    {
                        LoanerId = Receiver.Id,
                        Loaner = Receiver,
                        GroupContext = Group,
                        GroupContextId = Group.Id
                    },
                    Amount = amount,
                };

                ReceiverLoansToPayer.Add(loan);

                PayerOwesReceiver += amount;
                
                return this;
            }

            public PaymentServiceTestData AddPayerPaymentToReceiver(decimal amount) 
            {
                var payment = new Payment() 
                {
                    Id = Guid.NewGuid(),
                    ReceiverId = Receiver.Id,
                    Receiver = Receiver,
                    PayerId = Payer.Id,
                    Payer = Payer,
                    Amount = amount,
                    GroupContext = Group,
                    GroupContextId = Group.Id,
                };

                PayerPaymentsToReceiver.Add(payment);

                PayerOwesReceiver -= amount;
                
                return this;
            }

            public PaymentServiceTestData AddReceiverPaymentToPayer(decimal amount) 
            {
                var payment = new Payment() 
                {
                    Id = Guid.NewGuid(),
                    ReceiverId = Payer.Id,
                    Receiver = Payer,
                    PayerId = Receiver.Id,
                    Payer = Receiver,
                    Amount = amount,
                    GroupContext = Group,
                    GroupContextId = Group.Id,
                };

                ReceiverPaymentsToPayer.Add(payment);

                PayerOwesReceiver += amount;
                
                return this;
            }

            public static Group CreateGroupWithUsers(string name, int userCount, Guid? groupId = null)
            {
                var group = new Group()
                {
                    Id = groupId ?? Guid.NewGuid(),
                    Name = name,
                };

                group.Users = userCount.Select((_) => CreateUser(group)).ToList();
                return group;
            }


            public static User CreateUser(Group? group = null)
            {
                var faker = new Faker();
                var user = new User()
                {
                    Id = Guid.NewGuid(),
                    Name = faker.Person.FullName,
                    Groups = new List<Group>(),
                };

                if (group != null)
                    user.Groups.Add(group);

                return user;
            }
        }

        protected class PaymentServiceSutBuilder : ISutBuilder<PaymentService>
        {
            internal IPaymentRepository PaymentRepository { get; set; } = A.Fake<IPaymentRepository>();
            internal ILoanRepository LoanRepository { get; set; } = A.Fake<ILoanRepository>();
            internal ILoanDbFilterFactory LoanDbFilterFactory { get; set; } = A.Fake<ILoanDbFilterFactory>();
            internal IPaymentDbFilterFactory PaymentDbFilterFactory { get; set; } = A.Fake<IPaymentDbFilterFactory>();
            internal IUserRepository UserRepository { get; set; } = A.Fake<IUserRepository>();

            public PaymentService CreateSut()
            {
                return new PaymentService(PaymentRepository, LoanRepository, LoanDbFilterFactory, PaymentDbFilterFactory, UserRepository);
            }

            public static Group CreateGroupWithUsers(string name, int userCount, Guid? groupId = null)
            {
                var group = new Group()
                {
                    Id = groupId ?? Guid.NewGuid(),
                    Name = name,
                };

                group.Users = userCount.Select((_) => CreateUser(group)).ToList();
                return group;
            }

            public static User CreateUser(Group? group = null)
            {
                var faker = new Faker();
                var user = new User()
                {
                    Id = Guid.NewGuid(),
                    Name = faker.Person.FullName,
                    Groups = new List<Group>(),
                };

                if (group != null)
                    user.Groups.Add(group);

                return user;
            }

            public void SetupLoans(List<Loan> loans, Guid loanerId, Guid loaneeId, Guid? groupId)
            {
                var filter = A.Fake<LoanDbFilter>();
                A.CallTo(() => LoanDbFilterFactory.Create(A<LoanFilterInfo>.That.Matches(passedInfo => 
                    passedInfo.GroupId == groupId &&
                    passedInfo.LoanerId == loanerId &&
                    passedInfo.LoaneeId == loaneeId
                ))).Returns(filter);

                A.CallTo(() => LoanRepository.GetAllAsync(filter))
                    .Returns(loans);
            }

            public void SetupPayments(List<Payment> payments, Guid payerId, Guid receiverId, Guid? groupId)
            {
                var filter = A.Fake<PaymentDbFilter>();
                A.CallTo(() => PaymentDbFilterFactory.Create(A<PaymentFilterInfo>.That.Matches(passedInfo => 
                    passedInfo.GroupId == groupId &&
                    passedInfo.PayerId == payerId &&
                    passedInfo.ReceiverId == receiverId 
                ))).Returns(filter);

                A.CallTo(() => PaymentRepository.GetAllAsync(filter))
                    .Returns(payments);
            }

            public void SetupByIdUser(Guid id, User? user) 
            {
                A.CallTo(() => UserRepository.GetByIdAsync(id))
                    .Returns(user);
            }

            public void SetupByGroupUser(Group group, bool nullGroup = false) 
            {
                if (nullGroup)
                {
                    A.CallTo(() => UserRepository.GetAllAsync(null))
                        .Returns(group.Users);

                    return;
                }

                A.CallTo(() => UserRepository.GetByGroupIdAsync(group.Id))
                    .Returns(group.Users);
            }

            public Payment SetupAddPayment(CreateNewPayment newPaymentData) 
            {
                var expectedPayment = new Payment();
                A.CallTo(() => PaymentRepository.AddAsync(A<Payment>.That.Matches((passedPayment) =>
                    passedPayment.Amount == newPaymentData.Amount &&
                    passedPayment.PayerId == newPaymentData.PayerId &&
                    passedPayment.ReceiverId == newPaymentData.ReceiverId &&
                    passedPayment.GroupContextId == newPaymentData.GroupContextId
                ))).Returns(expectedPayment);

                return expectedPayment;
            }

            public Payment SetupFromTestData(PaymentServiceTestData testData, bool nullGroup = false)
            {
                SetupByIdUser(testData.Payer.Id, testData.Payer);
                SetupByIdUser(testData.Receiver.Id, testData.Receiver);
                SetupByGroupUser(testData.Group, nullGroup);

                SetupLoans(
                    testData.PayerLoansToReceiver,
                    loanerId: testData.Payer.Id,
                    loaneeId: testData.Receiver.Id,
                    nullGroup ? null : testData.Group.Id
                );

                SetupLoans(
                    testData.ReceiverLoansToPayer,
                    loanerId: testData.Receiver.Id,
                    loaneeId: testData.Payer.Id,
                    nullGroup ? null : testData.Group.Id
                );

                SetupPayments(
                    testData.PayerPaymentsToReceiver,
                    payerId: testData.Payer.Id,
                    receiverId: testData.Receiver.Id,
                    nullGroup ? null : testData.Group.Id
                );

                SetupPayments(
                    testData.ReceiverPaymentsToPayer,
                    payerId: testData.Receiver.Id,
                    receiverId: testData.Payer.Id,
                    nullGroup ? null : testData.Group.Id
                );

                return SetupAddPayment(testData.NewPaymentData);
            }
        }

        [Test]
        public async Task GetFilteredPaymentsAsync_WhenFilterInfoPassed_ShouldCreateFilterAndReturnExpectedResult()
        {
            //Arrange
            var sutBuilder = new PaymentServiceSutBuilder();

            var filterInfo = new PaymentFilterInfo();

            var filter = A.Fake<PaymentDbFilter>();
            A.CallTo(() => sutBuilder.PaymentDbFilterFactory.Create(filterInfo))
                .Returns(filter);

            var expectedList = new List<Payment>();
            A.CallTo(() => sutBuilder.PaymentRepository.GetAllAsync(filter))
                .Returns(expectedList);

            var paymentService = sutBuilder.CreateSut();

            //Act
            var results = await paymentService.GetFilteredPaymentsAsync(filterInfo);

            //Assert
            results.ShouldBe(expectedList);
        }

        [Test]
        public void AddPaymentAsync_WhenReceiverDoesNotExist_ShouldThrowNotFound()
        {
            //Arrange
            var sutBuilder = new PaymentServiceSutBuilder();
            var testData = new PaymentServiceTestData(40)
                    .AddReceiverLoanToPayer(50);

            sutBuilder.SetupFromTestData(testData);
            sutBuilder.SetupByIdUser(testData.Receiver.Id, null);

            var paymentService = sutBuilder.CreateSut();

            //Act & Assert
            var exception = Assert.ThrowsAsync<NotFoundException>(async () => await paymentService.AddPaymentAsync(testData.NewPaymentData));
            exception.Message.ShouldBe("User with given id does not exist.");
        }

        [Test]
        public void AddPaymentAsync_WhenPayerDoesNotExist_ShouldThrowNotFound()
        {
            //Arrange
            var sutBuilder = new PaymentServiceSutBuilder();
            var testData = new PaymentServiceTestData(40)
                    .AddReceiverLoanToPayer(50);

            sutBuilder.SetupFromTestData(testData);
            sutBuilder.SetupByIdUser(testData.Payer.Id, null);

            var paymentService = sutBuilder.CreateSut();

            //Act & Assert
            var exception = Assert.ThrowsAsync<NotFoundException>(async () => await paymentService.AddPaymentAsync(testData.NewPaymentData));
            exception.Message.ShouldBe("User with given id does not exist.");
        }

        static IEnumerable<PaymentServiceTestData> NoPaymentsExpectedTestData() 
        {
            return new List<PaymentServiceTestData>() 
            {
                new PaymentServiceTestData(50),
                new PaymentServiceTestData(50)
                    .AddPayerLoanToReceiver(25)
                    .AddReceiverPaymentToPayer(25),
                new PaymentServiceTestData(10)
                    .AddPayerLoanToReceiver(25)
                    .AddReceiverLoanToPayer(25),
                new PaymentServiceTestData(5)
                    .AddReceiverLoanToPayer(25)
                    .AddPayerPaymentToReceiver(25),
                new PaymentServiceTestData(200)
                    .AddReceiverLoanToPayer(100)
                    .AddReceiverLoanToPayer(150)
                    .AddPayerLoanToReceiver(50)
                    .AddPayerPaymentToReceiver(200),
            };
        } 

        [Test]
        [TestCaseSource(nameof(NoPaymentsExpectedTestData))]
        public void AddPaymentAsync_WhenNoPaymentsAreExpected_ShouldThrowNoPaymentsExpected(PaymentServiceTestData testData)
        {
            //Arrange
            var sutBuilder = new PaymentServiceSutBuilder();

            sutBuilder.SetupFromTestData(testData);

            var paymentService = sutBuilder.CreateSut();

            //Act & Assert
            var exception = Assert.ThrowsAsync<BadRequestException>(async () => await paymentService.AddPaymentAsync(testData.NewPaymentData));
            exception.Message.ShouldBe("No payment expected.");
        }

        static IEnumerable<PaymentServiceTestData> ReceiverOwesPayerTestData() 
        {
            return new List<PaymentServiceTestData>() 
            {
                new PaymentServiceTestData(50)
                    .AddPayerLoanToReceiver(50),
                new PaymentServiceTestData(10)
                    .AddPayerLoanToReceiver(25)
                    .AddReceiverLoanToPayer(20),
                new PaymentServiceTestData(5)
                    .AddPayerLoanToReceiver(50)
                    .AddReceiverPaymentToPayer(48),
                new PaymentServiceTestData(200)
                    .AddPayerLoanToReceiver(100)
                    .AddReceiverLoanToPayer(140)
                    .AddPayerPaymentToReceiver(20)
                    .AddPayerPaymentToReceiver(20)
                    .AddPayerLoanToReceiver(50)
                    .AddReceiverPaymentToPayer(20),
            };
        } 

        [Test]
        [TestCaseSource(nameof(ReceiverOwesPayerTestData))]
        public void AddPaymentAsync_WhenReceiverOwesPayer_ShouldThrowAlreadyOwedMoney(PaymentServiceTestData testData)
        {
            //Arrange
            var sutBuilder = new PaymentServiceSutBuilder();
            sutBuilder.SetupFromTestData(testData);

            var paymentService = sutBuilder.CreateSut();

            //Act & Assert
            var exception = Assert.ThrowsAsync<BadRequestException>(async () => await paymentService.AddPaymentAsync(testData.NewPaymentData));
            exception.Message.ShouldBe($"User {testData.Receiver.Email} already owes you money.");
        }

        static IEnumerable<PaymentServiceTestData> OverpayTestData() 
        {
            return new List<PaymentServiceTestData>() 
            {
                new PaymentServiceTestData(100)
                    .AddReceiverLoanToPayer(50),
                new PaymentServiceTestData(10)
                    .AddReceiverLoanToPayer(4)
                    .AddReceiverLoanToPayer(5.99M),
                new PaymentServiceTestData(30)
                    .AddReceiverLoanToPayer(50)
                    .AddPayerLoanToReceiver(25),
                new PaymentServiceTestData(50)
                    .AddReceiverLoanToPayer(100)
                    .AddPayerPaymentToReceiver(80),
                new PaymentServiceTestData(200)
                    .AddReceiverLoanToPayer(500)
                    .AddPayerLoanToReceiver(200)
                    .AddPayerPaymentToReceiver(50)
                    .AddPayerLoanToReceiver(80),
            };
        } 

        [Test]
        [TestCaseSource(nameof(OverpayTestData))]
        public void AddPaymentAsync_WhenTryingToOverpay_ShouldThrowOverpay(PaymentServiceTestData testData)
        {
            //Arrange
            var sutBuilder = new PaymentServiceSutBuilder();

            sutBuilder.SetupFromTestData(testData);

            var paymentService = sutBuilder.CreateSut();

            //Act & Assert
            var exception = Assert.ThrowsAsync<BadRequestException>(async () => await paymentService.AddPaymentAsync(testData.NewPaymentData));
            exception.Message.ShouldBe("Trying to pay back more than owned.");
        }

        static IEnumerable<PaymentServiceTestData> CorrectPaymentTestData() 
        {
            return new List<PaymentServiceTestData>() 
            {
                new PaymentServiceTestData(40)
                    .AddReceiverLoanToPayer(50),
                new PaymentServiceTestData(8)
                    .AddReceiverLoanToPayer(4)
                    .AddReceiverLoanToPayer(5.99M),
                new PaymentServiceTestData(25)
                    .AddReceiverLoanToPayer(50)
                    .AddPayerLoanToReceiver(25),
                new PaymentServiceTestData(10)
                    .AddReceiverLoanToPayer(100)
                    .AddPayerPaymentToReceiver(80),
                new PaymentServiceTestData(0.01M)
                    .AddReceiverLoanToPayer(500)
                    .AddPayerLoanToReceiver(200)
                    .AddPayerPaymentToReceiver(50)
                    .AddPayerLoanToReceiver(80),
                new PaymentServiceTestData(170)
                    .AddReceiverLoanToPayer(500)
                    .AddPayerLoanToReceiver(200)
                    .AddPayerPaymentToReceiver(50)
                    .AddPayerLoanToReceiver(80),
                new PaymentServiceTestData(50)
                    .AddReceiverLoanToPayer(500)
                    .AddPayerLoanToReceiver(600)
                    .AddReceiverPaymentToPayer(100)
                    .AddReceiverLoanToPayer(100)
                    .AddPayerLoanToReceiver(50),
            };
        } 

        [Test]
        [TestCaseSource(nameof(CorrectPaymentTestData))]
        public async Task AddPaymentAsync_WhenPaymentCorrect_ShouldReturnExpectedPayment(PaymentServiceTestData testData)
        {
            //Arrange
            var sutBuilder = new PaymentServiceSutBuilder();
            var expectedPayment = sutBuilder.SetupFromTestData(testData);

            var paymentService = sutBuilder.CreateSut();

            //Act
            var resultPayment = await paymentService.AddPaymentAsync(testData.NewPaymentData);

            //Assert
            resultPayment.ShouldBe(expectedPayment);
        }


        static IEnumerable<PaymentServiceTestData> NullGroupExpectedPaymentsTestData() 
        {
            return new List<PaymentServiceTestData>() 
            {
                new PaymentServiceTestData(40)
                    .AddReceiverLoanToPayer(50),
                new PaymentServiceTestData(8)
                    .AddReceiverLoanToPayer(4)
                    .AddReceiverLoanToPayer(5.99M),
                new PaymentServiceTestData(25)
                    .AddReceiverLoanToPayer(50)
                    .AddPayerLoanToReceiver(25),
                new PaymentServiceTestData(10)
                    .AddReceiverLoanToPayer(100)
                    .AddPayerPaymentToReceiver(80),
                new PaymentServiceTestData(0.01M)
                    .AddReceiverLoanToPayer(500)
                    .AddPayerLoanToReceiver(200)
                    .AddPayerPaymentToReceiver(50)
                    .AddPayerLoanToReceiver(80),
                new PaymentServiceTestData(170)
                    .AddReceiverLoanToPayer(500)
                    .AddPayerLoanToReceiver(200)
                    .AddPayerPaymentToReceiver(50)
                    .AddPayerLoanToReceiver(80),
                new PaymentServiceTestData(50)
                    .AddReceiverLoanToPayer(500)
                    .AddPayerLoanToReceiver(600)
                    .AddReceiverPaymentToPayer(100)
                    .AddReceiverLoanToPayer(100)
                    .AddPayerLoanToReceiver(50),
            };
        } 

        [Test]
        [TestCaseSource(nameof(NullGroupExpectedPaymentsTestData))]
        public async Task GetExpectedPaymentsForUserAsync_WhenCalledWithNullGroup_ShouldReturnExpectedPayments(PaymentServiceTestData testData)
        {
            //Arrange
            var sutBuilder = new PaymentServiceSutBuilder();
            sutBuilder.SetupFromTestData(testData, nullGroup: true);

            var paymentService = sutBuilder.CreateSut();

            //Act
            var payerPayments = await paymentService.GetExpectedPaymentsForUserAsync(testData.Payer.Id, null);
            var receiverPayments = await paymentService.GetExpectedPaymentsForUserAsync(testData.Receiver.Id, null);

            //Assert
            payerPayments.Count.ShouldBe(1);

            var payerPayment = payerPayments.Single();
            payerPayment.Amount.ShouldBe(testData.PayerOwesReceiver);
            payerPayment.Payer.ShouldBe(testData.Payer);
            payerPayment.Receiver.ShouldBe(testData.Receiver);

            receiverPayments.Count.ShouldBe(1);
            var receiverPayment = payerPayments.Single();
            receiverPayment.Amount.ShouldBe(testData.PayerOwesReceiver);
            receiverPayment.Payer.ShouldBe(testData.Payer);
            receiverPayment.Receiver.ShouldBe(testData.Receiver);
        }

        static IEnumerable<PaymentServiceTestData> GroupExpectedPaymentsTestData() 
        {
            return new List<PaymentServiceTestData>() 
            {
                new PaymentServiceTestData(40)
                    .AddReceiverLoanToPayer(50),
                new PaymentServiceTestData(8)
                    .AddReceiverLoanToPayer(4)
                    .AddReceiverLoanToPayer(5.99M),
                new PaymentServiceTestData(25)
                    .AddReceiverLoanToPayer(50)
                    .AddPayerLoanToReceiver(25),
                new PaymentServiceTestData(10)
                    .AddReceiverLoanToPayer(100)
                    .AddPayerPaymentToReceiver(80),
                new PaymentServiceTestData(0.01M)
                    .AddReceiverLoanToPayer(500)
                    .AddPayerLoanToReceiver(200)
                    .AddPayerPaymentToReceiver(50)
                    .AddPayerLoanToReceiver(80),
                new PaymentServiceTestData(170)
                    .AddReceiverLoanToPayer(500)
                    .AddPayerLoanToReceiver(200)
                    .AddPayerPaymentToReceiver(50)
                    .AddPayerLoanToReceiver(80),
                new PaymentServiceTestData(50)
                    .AddReceiverLoanToPayer(500)
                    .AddPayerLoanToReceiver(600)
                    .AddReceiverPaymentToPayer(100)
                    .AddReceiverLoanToPayer(100)
                    .AddPayerLoanToReceiver(50),
            };
        } 

        [Test]
        [TestCaseSource(nameof(GroupExpectedPaymentsTestData))]
        public async Task GetExpectedPaymentsForUserAsync_WhenCalledWithGroup_ShouldReturnExpectedPayments(PaymentServiceTestData testData)
        {
            //Arrange
            var sutBuilder = new PaymentServiceSutBuilder();
            sutBuilder.SetupFromTestData(testData);

            var paymentService = sutBuilder.CreateSut();

            //Act
            var payerPayments = await paymentService.GetExpectedPaymentsForUserAsync(testData.Payer.Id, testData.Group.Id);
            var receiverPayments = await paymentService.GetExpectedPaymentsForUserAsync(testData.Receiver.Id, testData.Group.Id);

            //Assert
            payerPayments.Count.ShouldBe(1);

            var payerPayment = payerPayments.Single();
            payerPayment.Amount.ShouldBe(testData.PayerOwesReceiver);
            payerPayment.Payer.ShouldBe(testData.Payer);
            payerPayment.Receiver.ShouldBe(testData.Receiver);

            receiverPayments.Count.ShouldBe(1);
            var receiverPayment = payerPayments.Single();
            receiverPayment.Amount.ShouldBe(testData.PayerOwesReceiver);
            receiverPayment.Payer.ShouldBe(testData.Payer);
            receiverPayment.Receiver.ShouldBe(testData.Receiver);
        }
    }
}