using BillChopBE.DataAccessLayer.Models;
using BillChopBE.DataAccessLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<IList<User>> SearchNameAndEmailAsync(string keyword, Guid? exclusionGroupId, int top)
        {
            keyword = keyword.ToLower();
            var users = DbSet
                .Where(g => g.Email.ToLower().Contains(keyword) || g.Name.ToLower().Contains(keyword));

            if (exclusionGroupId.HasValue)
                users = users.Where(u => u.Groups.All(g => g.Id != exclusionGroupId.Value));

            return await users
                .Take(top)
                .ToListAsync();
        }
        
        public async Task<User> GetByEmailAndPassword(string email, string password)
        {
            return await DbSet.FirstOrDefaultAsync(u => u.Email == email && u.Password == password);
        }
    }
}
