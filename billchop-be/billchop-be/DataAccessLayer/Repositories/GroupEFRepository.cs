using BillChopBE.DataAccessLayer.Models;
using BillChopBE.DataAccessLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

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
    }
}
