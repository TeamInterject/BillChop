using BillChopBE.DataAccessLayer.Models;
using BillChopBE.DataAccessLayer.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BillChopBE.Services
{
    public interface ILoanService
    {
        Task<IList<Loan>> GetBillLoans(Guid billId);
        Task<IList<Loan>> GetLentUserLoansInGroup(Guid loanerId, Guid groupId);
        Task<IList<Loan>> GetTakenUserLoans(Guid userId);
        Task<IList<Loan>> GetTakenUserLoansInGroup(Guid loaneeId, Guid groupId);
    }

    public class LoanService : ILoanService
    {
        private readonly ILoanRepository loanRepository;

        public LoanService(ILoanRepository loanRepository)
        {
            this.loanRepository = loanRepository;
        }

        /// <summary>
        /// Method for getting all loans where user borrowed money.
        /// Note: Useful for calculating total loan sums for a user.
        /// </summary>
        /// <param name="userId">Id of user who borrowed money</param>
        /// <returns>List of loan instances where user borrowed money.</returns>
        public Task<IList<Loan>> GetTakenUserLoans(Guid userId)
        {
            return loanRepository.GetByUserIdAsync(userId);
        }

        /// <summary>
        /// Method for getting all loans where user borrowed money in a certain group.
        /// Note: Useful for calculating total loan sums for a user in a group.
        /// </summary>
        /// <param name="loaneeId">Id of user who borrowed money</param>
        /// <param name="groupId">Id of context group</param>
        /// <returns>List of loan instances where user borrowed money in a certain group.</returns>
        public Task<IList<Loan>> GetTakenUserLoansInGroup(Guid loaneeId, Guid groupId)
        {
            return loanRepository.GetByLoaneeAndGroupAsync(loaneeId, groupId);
        }

        /// <summary>
        /// Get all loans associated with a bill
        /// </summary>
        /// <param name="billId">Bill to get loans for</param>
        /// <returns>List of loans associated with a specific bill</returns>
        public Task<IList<Loan>> GetBillLoans(Guid billId)
        {
            return loanRepository.GetByBillIdAsync(billId);
        }

        /// <summary>
        /// Get all loans loaned by a specific user in a specific group.
        /// </summary>
        /// <param name="loanerId">Loaner user who is owed money</param>
        /// <param name="groupId">Loan group context</param>
        /// <returns>List of loans owed to a user in a specific group context</returns>
        public Task<IList<Loan>> GetLentUserLoansInGroup(Guid loanerId, Guid groupId)
        {
            return loanRepository.GetByLoanerAndGroupAsync(loanerId, groupId);
        }
    }
}
