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
using MoreLinq.Extensions;

namespace BillChopBE.Services
{
    public interface IPaymentService
    {
        Task<Payment> AddPaymentAsync(CreateNewPayment newPaymentData);
        Task<IList<Payment>> GetExpectedPaymentsForUserAsync(Guid userId, Guid? groupId);
        Task<IList<Payment>> GetFilteredPaymentsAsync(PaymentFilterInfo paymentFilterInfo);
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

        public Task<IList<Payment>> GetFilteredPaymentsAsync(PaymentFilterInfo paymentFilterInfo)
        {
            var filter = paymentDbFilterFactory.Create(paymentFilterInfo);
            return paymentRepository.GetAllAsync(filter);
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
                var userToUserPayments = await GetExpectedPaymentsBetweenUsers(
                    userA: user, 
                    userB: otherUser, 
                    groupContextId: groupId);
                
                expectedPayments.AddRange(userToUserPayments);
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

            var expectedPayments = await GetExpectedPaymentsBetweenUsers(
                userA: payer,
                userB: receiver,
                groupContextId: newPaymentData.GroupContextId
            );

            if (expectedPayments.Count < 1)
                throw new BadRequestException("No payment expected.");

            var expectedPayment = expectedPayments.Single();
            if (payer.Id != expectedPayment.PayerId || receiver.Id != expectedPayment.ReceiverId) 
                throw new BadRequestException($"User {receiver.Email} already owes you money.");

            if (newPaymentData.Amount > expectedPayment.Amount)
                throw new BadRequestException("Trying to pay back more than owned.");

            var paymentToAdd = newPaymentData.ToPayment();

            return await paymentRepository.AddAsync(paymentToAdd);
        }

        private static Payment CreatePayment(decimal total, User payer, User receiver, Guid groupContextId) 
        {
            return new Payment() 
            {
                Id = Guid.NewGuid(),
                Amount = total,
                PayerId = payer.Id,
                Payer = payer,
                ReceiverId = receiver.Id,
                Receiver = receiver,
                GroupContextId = groupContextId,
            };
        }

        private async Task<List<Payment>> GetExpectedPaymentsBetweenUsers(User userA, User userB, Guid? groupContextId) 
        {
            var bOwesTotals = await GetLoaneeOwnedTotals(
                loaner: userA, 
                loanee: userB, 
                groupContextId: groupContextId
            );

            var aOwesTotals = await GetLoaneeOwnedTotals(
                loaner: userB, 
                loanee: userA, 
                groupContextId: groupContextId
            );

            var expectedPayments = aOwesTotals.FullJoin(
                bOwesTotals,
                (aOwes) => aOwes.GroupContextId,
                (aOwes) => aOwes.Total != 0 ? CreatePayment(aOwes.Total, payer: userA, receiver: userB, aOwes.GroupContextId) : null,
                (bOwes) => bOwes.Total != 0 ? CreatePayment(bOwes.Total, payer: userB, receiver: userA, bOwes.GroupContextId) : null,
                (aOwes, bOwes) => 
                {
                    if (aOwes.Total > bOwes.Total)
                        return CreatePayment(aOwes.Total - bOwes.Total, payer: userA, receiver: userB, aOwes.GroupContextId);

                    if (bOwes.Total > aOwes.Total)
                        return CreatePayment(bOwes.Total - aOwes.Total, payer: userB, receiver: userA, bOwes.GroupContextId);

                    return null;
                }
            );

            return expectedPayments
                .NotNull()
                .ToList();
        }

        private class GroupedTotals 
        {
            public Guid GroupContextId { get;set; }
            public decimal Total { get; }

            public GroupedTotals(Guid groupContextId, decimal total) 
            {
                GroupContextId = groupContextId;
                Total = total;
            }
        }

        private async Task<List<GroupedTotals>> GetLoaneeOwnedTotals(User loaner, User loanee, Guid? groupContextId) {
            var loansToLoanee = await GetLoansBetweenUsers(
                loanerId: loaner.Id,
                loaneeId: loanee.Id,
                groupContextId: groupContextId
            );

            var loaneeOwesTotals = loansToLoanee
                .GroupBy(loan => loan.Bill.GroupContextId)
                .Select(loanGroup => 
                    new GroupedTotals(loanGroup.Key, loanGroup.Sum(loan => loan.Amount))
                ).ToList();

            var paymentsToLoaner = await GetPaymentsBetweenUsers(
                payerId: loanee.Id,
                receiverId: loaner.Id,
                groupContextId: groupContextId
            );

            var loaneePayedBackTotals = paymentsToLoaner
                .GroupBy(payment => payment.GroupContextId)
                .Select(paymentGroup => 
                    new GroupedTotals(paymentGroup.Key, paymentGroup.Sum(payment => payment.Amount))
                ).ToList();

            var totals = loaneeOwesTotals
                .LeftJoin(
                    loaneePayedBackTotals,
                    (owes) => owes.GroupContextId,
                    (owes) => new GroupedTotals(owes.GroupContextId, owes.Total),
                    (owes, payedBack) => new GroupedTotals(owes.GroupContextId, owes.Total - payedBack.Total)
                ).ToList();

            return totals;
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
