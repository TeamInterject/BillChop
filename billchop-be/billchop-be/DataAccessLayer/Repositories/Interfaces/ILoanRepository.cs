using BillChopBE.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BillChopBE.DataAccessLayer.Repositories.Interfaces
{
    public interface ILoanRepository : IRepository<Loan>
    {
        Task<IList<Loan>> GetByUserIdAsync(Guid userId);
        Task<IList<Loan>> GetByBillIdAsync(Guid billId);
        Task<IList<Loan>> GetByLoaneeAndGroupAsync(Guid userId, Guid groupId);
        Task<IList<Loan>> GetByLoanerAndGroupAsync(Guid loanerId, Guid groupId);
    }
}
