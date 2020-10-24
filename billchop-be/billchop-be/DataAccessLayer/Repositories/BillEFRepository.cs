using BillChopBE.DataAccessLayer.Models;
using BillChopBE.DataAccessLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public async Task<IList<Bill>> GetBillsByGroupId(Guid groupId)
        {
            return await DbSet.Where(b => b.GroupContext.Id == groupId).ToListAsync();
        }
    }
}
