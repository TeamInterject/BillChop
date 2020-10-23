using BillChopBE.DataAccessLayer.Models;
using BillChopBE.DataAccessLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BillChopBE.DataAccessLayer.Repositories
{
    public class LoanEFRepository : AbstractEFRepository<Loan>, ILoanRepository
    {
        private readonly BillChopContext context;
        protected override DbContext DbContext => context;
        protected override DbSet<Loan> DbSet => context.Loans;

        public LoanEFRepository(BillChopContext context)
        {
            this.context = context;
        }

        public async Task<IList<Loan>> GetByUserIdAsync(Guid userId)
        {
            return await DbSet
                .Where(e => e.LoaneeId == userId)
                .ToListAsync();
        }

        public async Task<IList<Loan>> GetByBillIdAsync(Guid billId)
        {
            return await DbSet
                .Where(e => e.BillId == billId)
                .ToListAsync();
        }

        public async Task<IList<Loan>> GetByLoaneeAndGroupAsync(Guid userId, Guid groupId)
        {
            return await DbSet
                .Where(e => e.LoaneeId == userId)
                .Where(e => e.Bill.GroupContextId == groupId)
                .ToListAsync();
        }

        public async Task<IList<Loan>> GetByLoanerAndGroupAsync(Guid loanerId, Guid groupId)
        {
            return await DbSet
                .Where(e => e.Bill.LoanerId == loanerId)
                .Where(e => e.Bill.GroupContextId == groupId)
                .ToListAsync();
        }
    }
}
