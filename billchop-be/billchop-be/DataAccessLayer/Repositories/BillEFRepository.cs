using BillChopBE.DataAccessLayer.Models;
using BillChopBE.DataAccessLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BillChopBE.DataAccessLayer.Repositories
{
    public class BillEFRepository : AbstractEFRepository<Bill>, IBillRepository
    {
        private readonly BillChopContext context;
        protected override DbContext DbContext => context;
        protected override DbSet<Bill> DbSet => context.Bills;

        public BillEFRepository(BillChopContext context)
        {
            this.context = context;
        }
    }
}
