using BillChopBE.DataAccessLayer.Filters;
using BillChopBE.DataAccessLayer.Filters.Factories;
using BillChopBE.DataAccessLayer.Models;
using BillChopBE.DataAccessLayer.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BillChopBE.Services
{
    public interface ILoanService
    {
        Task<IList<Loan>> GetProvidedLoansAsync(Guid loanerId, Guid? groupId, DateTime? startTime, DateTime? endTime);
        Task<IList<Loan>> GetReceivedLoansAsync(Guid loaneeId, Guid? groupId, DateTime? startTime, DateTime? endTime);
        Task<IList<Loan>> GetSelfLoansAsync(Guid loanerAndLoaneeId, Guid? groupId, DateTime? startTime, DateTime? endTime);
        Task<IList<Loan>> GetFilteredLoansAsync(LoanFilterInfo loanFilterInfo);
    }

    public class LoanService : ILoanService
    {
        private readonly ILoanRepository loanRepository;
        private readonly ILoanDbFilterFactory loanDbFilterFactory;

        public LoanService(ILoanRepository loanRepository, ILoanDbFilterFactory loanDbFilterFactory)
        {
            this.loanRepository = loanRepository;
            this.loanDbFilterFactory = loanDbFilterFactory;
        }

        public async Task<IList<Loan>> GetProvidedLoansAsync(Guid loanerId, Guid? groupId, DateTime? startTime, DateTime? endTime)
        {
            var filterInfo = new LoanFilterInfo()
            {
                LoanerId = loanerId,
                GroupId = groupId,
                StartTime = startTime,
                EndTime = endTime,
            };

            return await GetFilteredLoansAsync(filterInfo);
        }

        public async Task<IList<Loan>> GetReceivedLoansAsync(Guid loaneeId, Guid? groupId, DateTime? startTime, DateTime? endTime)
        {
            var filterInfo = new LoanFilterInfo()
            {
                LoaneeId = loaneeId,
                GroupId = groupId,
                StartTime = startTime,
                EndTime = endTime,
            };

            return await GetFilteredLoansAsync(filterInfo);
        }

        public async Task<IList<Loan>> GetSelfLoansAsync(Guid loanerAndLoaneeId, Guid? groupId, DateTime? startTime, DateTime? endTime)
        {
            var filterInfo = new LoanFilterInfo()
            {
                LoaneeId = loanerAndLoaneeId,
                LoanerId = loanerAndLoaneeId,
                GroupId = groupId,
                StartTime = startTime,
                EndTime = endTime,
            };

            return await GetFilteredLoansAsync(filterInfo);
        }

        public Task<IList<Loan>> GetFilteredLoansAsync(LoanFilterInfo loanFilterInfo)
        {
            var filter = loanDbFilterFactory.Create(loanFilterInfo);
            return loanRepository.GetAllAsync(filter);
        }
    }
}
