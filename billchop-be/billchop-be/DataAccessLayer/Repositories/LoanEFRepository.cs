using BillChopBE.DataAccessLayer.Models;
using BillChopBE.DataAccessLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

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
    }
}
