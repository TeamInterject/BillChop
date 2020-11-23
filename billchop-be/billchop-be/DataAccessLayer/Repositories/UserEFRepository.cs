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
            IQueryable<User> usersQuery = DbSet
                .Where(g => g.Email.ToLower().Contains(keyword) || g.Name.ToLower().Contains(keyword));

            if (exclusionGroupId.HasValue)
                usersQuery = usersQuery.Where(u => u.Groups.All(g => g.Id != exclusionGroupId.Value));

            IEnumerable<User> users = await usersQuery.ToListAsync();
            if (!keyword.Contains("@"))
            {
                users = users.Where(g => g.Email.Split("@", StringSplitOptions.None)[0].ToLower().Contains(keyword) ||
                        g.Name.ToLower().Contains(keyword));
            }

            return users
                .Take(top)
                .ToList();
        }
        
        public async Task<User> GetByEmailAndPassword(string email, string password)
        {
            return await DbSet.FirstOrDefaultAsync(u => u.Email == email && u.Password == password);
        }
    }
}
