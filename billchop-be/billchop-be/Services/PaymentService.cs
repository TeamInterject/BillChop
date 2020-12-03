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
            var otherUsers = await GetUsersForPaymentsAsync(userId, groupId);
            var expectedPayments = await otherUsers
                .Select(otherUser => GetExpectedPaymentBetweenUsers(
                    userA: userId, 
                    userB: otherUser.Id, 
                    groupContextId: groupId))
                .WhenAll();
            
            return expectedPayments
                .NotNull()
                .ToList();
        }

        private async Task<IList<User>> GetUsersForPaymentsAsync(Guid userId, Guid? groupId) 
        {
            Task<IList<User>> getContextUsers()
            {
                if (groupId.HasValue)
                    return userRepository.GetByGroupIdAsync(groupId.Value);

                return userRepository.GetAllAsync();
            }

            return (await getContextUsers())
                .Where(u => u.Id != userId)
                .ToList();
        }


        public async Task<Payment> AddPaymentAsync(CreateNewPayment newPaymentData) 
        {
            newPaymentData.Validate();
            var expectedPayment = await GetExpectedPaymentBetweenUsers(
                userA: newPaymentData.PayerId,
                userB: newPaymentData.ReceiverId,
                groupContextId: newPaymentData.GroupContextId
            );

            if (expectedPayment == null || 
                newPaymentData.PayerId != expectedPayment.PayerId || 
                newPaymentData.ReceiverId != expectedPayment.ReceiverId) 
            {
                throw new BadRequestException("No payment expected.");
            }

            if (newPaymentData.Amount > expectedPayment.Amount)
                throw new BadRequestException("Trying to pay back more than owned.");

            var paymentToAdd = newPaymentData.ToPayment();

            return await paymentRepository.AddAsync(paymentToAdd);
        }

        public async Task<Payment?> GetExpectedPaymentBetweenUsers(Guid userA, Guid userB, Guid? groupContextId) 
        {
            var bOwesAmount = await GetLoaneeOwnedSum(
                loanerId: userA, 
                loaneeId: userB, 
                groupContextId: groupContextId
            );

            var aOwesAmount = await GetLoaneeOwnedSum(
                loanerId: userB, 
                loaneeId: userA, 
                groupContextId: groupContextId
            );

            if (aOwesAmount > bOwesAmount) 
            {
                return new Payment() 
                {
                    Id = Guid.NewGuid(),
                    Amount = aOwesAmount - bOwesAmount,
                    PayerId = userA,
                    ReceiverId = userB,
                };
            }

            if (bOwesAmount > aOwesAmount) {
                return new Payment() 
                {
                    Id = Guid.NewGuid(),
                    Amount = bOwesAmount - aOwesAmount,
                    PayerId = userB,
                    ReceiverId = userA,
                };
            }

            return null;
        }

        private async Task<decimal> GetLoaneeOwnedSum(Guid loanerId, Guid loaneeId, Guid? groupContextId) {
            var loansAToB = await GetLoansBetweenUsers(
                loanerId: loanerId,
                loaneeId: loaneeId,
                groupContextId: groupContextId
            );

            var bOwesAmount = loansAToB.Sum((loan) => loan.Amount);

            var paymentsBToA = await GetPaymentsBetweenUsers(
                payerId: loaneeId,
                receiverId: loanerId,
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
