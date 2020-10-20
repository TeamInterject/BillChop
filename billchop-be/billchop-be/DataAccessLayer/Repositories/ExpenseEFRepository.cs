using BillChopBE.DataAccessLayer.Models;
using BillChopBE.DataAccessLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BillChopBE.DataAccessLayer.Repositories
{
    public class ExpenseEFRepository : AbstractEFRepository<Expense>, IExpenseRepository
    {
        private readonly BillChopContext context;
        protected override DbContext DbContext => context;
        protected override DbSet<Expense> DbSet => context.Expenses;

        public ExpenseEFRepository(BillChopContext context)
        {
            this.context = context;
        }
    }
}
