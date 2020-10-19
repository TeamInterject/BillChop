using BillChopBE.DataAccessLayer.Models.Interfaces;
using BillChopBE.DataAccessLayer.Models.Validation;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BillChopBE.DataAccessLayer.Repositories.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class, IValidatableModel, IDbModel
    {
        public Task<IList<TEntity>> GetAllAsync();

        public Task<TEntity?> GetByIdAsync(Guid id);

        public Task<TEntity> AddAsync(TEntity user);

        public Task<TEntity?> ModifyAsync(Guid id, Action<TEntity> modifyCallback);

        public Task<TEntity?> DeleteById(Guid id);
    }
}
