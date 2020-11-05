using BillChopBE.DataAccessLayer.Models;
using BillChopBE.DataAccessLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BillChopBE.DataAccessLayer.Repositories
{
    public class UserEFRepository : AbstractEFRepository<User>, IUserRepository
    {
        private readonly BillChopContext context;
        protected override DbContext DbContext => context;
        protected override DbSet<User> DbSet => context.Users;

        public UserEFRepository(BillChopContext context) 
        {
            this.context = context;
        }

        public async Task<IList<User>> SearchUsersAsync(string keyword)
        {
            return await DbSet.Where(g => ).ToListAsync();
        }
    }
}
