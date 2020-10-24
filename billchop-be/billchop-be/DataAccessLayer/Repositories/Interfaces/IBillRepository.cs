using BillChopBE.DataAccessLayer.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;

namespace BillChopBE.DataAccessLayer.Repositories.Interfaces
{
    public interface IBillRepository : IRepository<Bill>
    {
        Task<IList<Bill>> GetBillsByGroupId(Guid groupId);
    }
}
