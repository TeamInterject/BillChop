using BillChopBE.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BillChopBE.DataAccessLayer.Repositories.Interfaces
{
    public interface IGroupRepository : IRepository<Group>
    {
        Task<IList<Group>> GetUserGroups(Guid userId);
    }
}
