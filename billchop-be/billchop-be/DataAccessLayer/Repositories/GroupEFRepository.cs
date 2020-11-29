using BillChopBE.DataAccessLayer.Models;
using BillChopBE.DataAccessLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BillChopBE.DataAccessLayer.Repositories
{
  public class GroupEFRepository : AbstractEFRepository<Group>, IGroupRepository
    {
        private readonly BillChopContext context;
        protected override DbContext DbContext => context;
        protected override DbSet<Group> DbSet => context.Groups;

        public GroupEFRepository(BillChopContext context)
        {
            this.context = context;
        }

        public async Task<IList<Group>> GetUserGroups(Guid userId) 
        {
            return await DbSet
                .Where(g => g.Users.Any(u => u.Id == userId))
                .ToListAsync();
        }
    }
}
