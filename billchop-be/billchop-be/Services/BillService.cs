using BillChopBE.DataAccessLayer.Filters;
using BillChopBE.DataAccessLayer.Filters.Factories;
using BillChopBE.DataAccessLayer.Models;
using BillChopBE.DataAccessLayer.Repositories.Interfaces;
using BillChopBE.Exceptions;
using ProjectPortableTools.Extensions;
using BillChopBE.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BillChopBE.Services
{
    public interface IBillService
    {
        Task<Bill> CreateAndSplitBillAsync(CreateNewBill newBillData);
        Task<IList<Bill>> GetBillsAsync(Guid? groupId, DateTime? startTime, DateTime? endTime);
        Task<IList<Bill>> GetFilteredBillsAsync(BillFilterInfo billFilterInfo);
    }

    public class BillService : IBillService
    {
        private readonly IBillRepository billRepository;
        private readonly IGroupRepository groupRepository;
        private readonly IBillDbFilterFactory billDbFilterFactory;
        
        public BillService(IBillRepository billRepository,
            IGroupRepository groupRepository,
            IBillDbFilterFactory billDbFilterFactory)
        {
            this.billRepository = billRepository;
            this.groupRepository = groupRepository;
            this.billDbFilterFactory = billDbFilterFactory;
        }

        public async Task<IList<Bill>> GetBillsAsync(Guid? groupId, DateTime? startTime, DateTime? endTime)
        {
            var filterInfo = new BillFilterInfo()
            {
                GroupId = groupId,
                StartTime = startTime,
                EndTime = endTime,
            };

            return await GetFilteredBillsAsync(filterInfo);
        }

        public Task<IList<Bill>> GetFilteredBillsAsync(BillFilterInfo billFilterInfo)
        {
            var filter = billDbFilterFactory.Create(billFilterInfo);
            return billRepository.GetAllAsync(filter);
        }

        public async Task<Bill> CreateAndSplitBillAsync(CreateNewBill newBillData)
        {
            newBillData.Validate();

            var group = await groupRepository.GetByIdAsync(newBillData.GroupContextId);
            if (group == null)
                throw new NotFoundException($"Group with id {newBillData.GroupContextId} does not exist.");

            var loaner = group.Users.FirstOrDefault(user => user.Id == newBillData.LoanerId);
            if (loaner == null)
                throw new NotFoundException($"Payee with id {newBillData.LoanerId} does not exist in group.");

            var bill = new Bill()
            {
                Name = newBillData.Name,
                Total = newBillData.Total,
                LoanerId = loaner.Id,
                Loaner = loaner,
                GroupContextId = group.Id,
                GroupContext = group,
            };

            bill = await billRepository.AddAsync(bill);

            var loans = SplitBill(bill);
            loans.ForEach(loan => bill.Loans.Add(loan));

            await billRepository.SaveChangesAsync();

            return bill;
        }

        private static List<Loan> SplitBill(Bill bill)
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

            return loans;
        }
    }
}