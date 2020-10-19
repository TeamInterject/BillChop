using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BillChopBE.DataAccessLayer.Models.Interfaces
{
    public interface IDbModel
    {
        Guid Id { get; set; }
    }
}
