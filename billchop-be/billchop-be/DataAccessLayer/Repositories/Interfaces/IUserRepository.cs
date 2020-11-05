using BillChopBE.DataAccessLayer.Models;
using System.Threading.Tasks;

namespace BillChopBE.DataAccessLayer.Repositories.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
         Task<User> GetByEmailAsync(string email);
    }
}
