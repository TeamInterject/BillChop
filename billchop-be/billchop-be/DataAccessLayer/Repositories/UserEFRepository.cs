using BillChopBE.DataAccessLayer.Models;
using BillChopBE.DataAccessLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

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
    }
}
