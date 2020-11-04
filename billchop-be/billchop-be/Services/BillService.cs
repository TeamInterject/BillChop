using BillChopBE.DataAccessLayer.Models;
using BillChopBE.DataAccessLayer.Repositories.Interfaces;
using BillChopBE.Exceptions;
using ProjectPortableTools.Extensions;
using BillChopBE.Services.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BillChopBE.Services
{
    public interface IBillService
    {
        Task<Bill> CreateAndSplitBillAsync(CreateNewBill newBill);
        Task<IList<Bill>> GetBillsAsync(Guid? groupId);
    }

    public class BillService : IBillService
    {
        private readonly IBillRepository billRepository;
        private readonly IGroupRepository groupRepository;

        public BillService(IBillRepository billRepository,
            IGroupRepository groupRepository)
        {
            this.billRepository = billRepository;
            this.groupRepository = groupRepository;
        }

        public Task<IList<Bill>> GetBillsAsync(Guid? groupId)
        {
            if (groupId.HasValue)
                return GetGroupBillsAsync(groupId.Value);

            return billRepository.GetAllAsync();
        }

        private async Task<IList<Bill>> GetGroupBillsAsync(Guid groupId)
        {
            var group = await groupRepository.GetByIdAsync(groupId);
            if (group == null)
                throw new NotFoundException($"Group with id {groupId} does not exist");

            return await billRepository.GetBillsByGroupId(groupId);
        }

        // TODO.AZ: Investigate if it's possible to consistenly separate the logic from the "piping"
        // TODO.AZ: In other words, avoiding having logic mixed with getting stuff from db, as it complicates testing.
        public async Task<Bill> CreateAndSplitBillAsync(CreateNewBill newBill)
        {
            newBill.Validate();

            var group = await groupRepository.GetByIdAsync(newBill.GroupContextId);
            if (group == null)
                throw new NotFoundException($"Group with id {newBill.GroupContextId} does not exist.");

            var loaner = group.Users.FirstOrDefault(user => user.Id == newBill.LoanerId);
            if (loaner == null)
                throw new NotFoundException($"Payee with id {newBill.LoanerId} does not exist in group.");

            var bill = new Bill()
            {
                Name = newBill.Name,
                Total = newBill.Total,
                LoanerId = loaner.Id,
                Loaner = loaner,
                GroupContextId = group.Id,
                GroupContext = group,
            };

            bill = await billRepository.AddAsync(bill);
            SplitBillAsync(bill);

            await billRepository.SaveChangesAsync();

            return bill;
        }

        private IList<Loan> SplitBillAsync(Bill bill)
        {
            var payingUsers = bill.GroupContext.Users.ToList();
            var amounts = bill.Total.SplitEqually(payingUsers.Count).ToList();

            var loans = payingUsers
                .Select((user, index) => new Loan()
                {
                    BillId = bill.Id,
                    Bill = bill,
                    LoaneeId = user.Id,
                    Loanee = user,
                    Amount = amounts[index]
                }).ToList();
                
            loans.ForEach(loan => bill.Loans.Add(loan));

            return loans;
        }
    }
}
