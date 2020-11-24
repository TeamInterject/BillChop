using BillChopBE.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BillChopBE.DataAccessLayer.Repositories.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<IList<User>> SearchNameAndEmailAsync(string keyword, Guid? exclusionGroupId, int top);
        Task<User?> GetByEmailAndPassword(string email, string password);
    }
}
