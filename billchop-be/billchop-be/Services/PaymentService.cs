using BillChopBE.DataAccessLayer.Filters;
using BillChopBE.DataAccessLayer.Filters.Factories;
using BillChopBE.DataAccessLayer.Models;
using BillChopBE.DataAccessLayer.Repositories.Interfaces;
using BillChopBE.Exceptions;
using BillChopBE.Services.Models;
using ProjectPortableTools.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BillChopBE.Services
{
    public interface IPaymentService
    {
        Task<Payment> AddPaymentAsync(CreateNewPayment newPaymentData);
        Task<IList<Payment>> GetExpectedPaymentsForUserAsync(Guid userId, Guid? groupId);
    }

    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository paymentRepository;
        private readonly ILoanRepository loanRepository;
        private readonly ILoanDbFilterFactory loanDbFilterFactory;
        private readonly IPaymentDbFilterFactory paymentDbFilterFactory;
        private readonly IUserRepository userRepository;

        public PaymentService(IPaymentRepository paymentRepository, 
            ILoanRepository loanRepository, 
            ILoanDbFilterFactory loanDbFilterFactory,
            IPaymentDbFilterFactory paymentDbFilterFactory,
            IUserRepository userRepository)
        {
            this.loanDbFilterFactory = loanDbFilterFactory;
            this.paymentDbFilterFactory = paymentDbFilterFactory;
            this.userRepository = userRepository;
            this.loanRepository = loanRepository;
            this.paymentRepository = paymentRepository;
        }

        public async Task<IList<Payment>> GetExpectedPaymentsForUserAsync(Guid userId, Guid? groupId)
        {
            var user = await userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new NotFoundException("User with given id does not exist");

            var otherUsers = await GetUsersForPaymentsAsync(user, groupId);

            var expectedPayments = new List<Payment>();
            foreach(var otherUser in otherUsers) 
            {
                var expectedPayment = await GetExpectedPaymentBetweenUsers(
                    userA: user, 
                    userB: otherUser, 
                    groupContextId: groupId);
                
                if (expectedPayment != null)
                    expectedPayments.Add(expectedPayment);
            }
            
            return expectedPayments;
        }

        private async Task<IList<User>> GetUsersForPaymentsAsync(User user, Guid? groupId) 
        {
            Task<IList<User>> getContextUsers()
            {
                if (groupId.HasValue)
                    return userRepository.GetByGroupIdAsync(groupId.Value);

                return userRepository.GetAllAsync();
            }

            return (await getContextUsers())
                .Where(u => u.Id != user.Id)
                .ToList();
        }


        public async Task<Payment> AddPaymentAsync(CreateNewPayment newPaymentData) 
        {
            newPaymentData.Validate();

            var payer = await userRepository.GetByIdAsync(newPaymentData.PayerId);
            if (payer == null)
                throw new NotFoundException("User with given id does not exist");

            var receiver = await userRepository.GetByIdAsync(newPaymentData.ReceiverId);
            if (receiver == null)
                throw new NotFoundException("User with given id does not exist");

            var expectedPayment = await GetExpectedPaymentBetweenUsers(
                userA: payer,
                userB: receiver,
                groupContextId: newPaymentData.GroupContextId
            );

            if (expectedPayment == null || 
                payer.Id != expectedPayment.PayerId || 
                receiver.Id != expectedPayment.ReceiverId) 
            {
                throw new BadRequestException("No payment expected.");
            }

            if (newPaymentData.Amount > expectedPayment.Amount)
                throw new BadRequestException("Trying to pay back more than owned.");

            var paymentToAdd = newPaymentData.ToPayment();

            return await paymentRepository.AddAsync(paymentToAdd);
        }

        public async Task<Payment?> GetExpectedPaymentBetweenUsers(User userA, User userB, Guid? groupContextId) 
        {
            var bOwesAmount = await GetLoaneeOwnedSum(
                loaner: userA, 
                loanee: userB, 
                groupContextId: groupContextId
            );

            var aOwesAmount = await GetLoaneeOwnedSum(
                loaner: userB, 
                loanee: userA, 
                groupContextId: groupContextId
            );

            if (aOwesAmount > bOwesAmount) 
            {
                return new Payment() 
                {
                    Id = Guid.NewGuid(),
                    Amount = aOwesAmount - bOwesAmount,
                    PayerId = userA.Id,
                    Payer = userA,
                    ReceiverId = userB.Id,
                    Receiver = userB,
                    GroupContextId = groupContextId ?? Guid.Empty, //TODO.AZ: Fix this by grouping payments
                };
            }

            if (bOwesAmount > aOwesAmount) {
                return new Payment() 
                {
                    Id = Guid.NewGuid(),
                    Amount = bOwesAmount - aOwesAmount,
                    PayerId = userB.Id,
                    Payer = userB,
                    ReceiverId = userA.Id,
                    Receiver = userA,
                    GroupContextId = groupContextId ?? Guid.Empty, //TODO.AZ: Fix this by grouping payments
                };
            }

            return null;
        }

        private async Task<decimal> GetLoaneeOwnedSum(User loaner, User loanee, Guid? groupContextId) {
            var loansAToB = await GetLoansBetweenUsers(
                loanerId: loaner.Id,
                loaneeId: loanee.Id,
                groupContextId: groupContextId
            );

            var bOwesAmount = loansAToB.Sum((loan) => loan.Amount);

            var paymentsBToA = await GetPaymentsBetweenUsers(
                payerId: loanee.Id,
                receiverId: loaner.Id,
                groupContextId: groupContextId
            );

            var bPayedBackAmount = paymentsBToA.Sum((payment) => payment.Amount);

            return bOwesAmount - bPayedBackAmount;
        }

        private Task<IList<Loan>> GetLoansBetweenUsers(Guid loanerId, Guid loaneeId, Guid? groupContextId) 
        {
            var filterData = new LoanFilterInfo() 
            {
                LoaneeId = loaneeId,
                LoanerId = loanerId,
                GroupId = groupContextId,
            };

            var dbFilter = loanDbFilterFactory.Create(filterData);
            return loanRepository.GetAllAsync(dbFilter);
        }

        private Task<IList<Payment>> GetPaymentsBetweenUsers(Guid payerId, Guid receiverId, Guid? groupContextId) 
        {
            var filterData = new PaymentFilterInfo() 
            {
                PayerId = payerId,
                ReceiverId = receiverId,
                GroupId = groupContextId,
            };

            var dbFilter = paymentDbFilterFactory.Create(filterData);
            return paymentRepository.GetAllAsync(dbFilter);
        }
    }
}
