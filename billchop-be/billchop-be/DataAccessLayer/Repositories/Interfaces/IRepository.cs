using BillChopBE.DataAccessLayer.Filters;
using BillChopBE.DataAccessLayer.Models.Interfaces;
using BillChopBE.DataAccessLayer.Models.Validation;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BillChopBE.DataAccessLayer.Repositories.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class, IValidatableModel, IDbModel
    {
        Task<IList<TEntity>> GetAllAsync(IDbFilter<TEntity>? dbFilter = null);

        Task<TEntity?> GetByIdAsync(Guid id);

        Task<TEntity> AddAsync(TEntity entity);

        Task<TEntity?> ModifyAsync(Guid id, Action<TEntity> modifyCallback);

        Task<TEntity?> DeleteById(Guid id);
        
        Task<int> SaveChangesAsync();
    }
}
