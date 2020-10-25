using BillChopBE.DataAccessLayer.Filters;
using BillChopBE.DataAccessLayer.Models;
using BillChopBE.DataAccessLayer.Models.Interfaces;
using BillChopBE.DataAccessLayer.Models.Validation;
using BillChopBE.DataAccessLayer.Repositories.Interfaces;
using BillChopBE.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BillChopBE.DataAccessLayer.Repositories
{
    public abstract class AbstractEFRepository<TEntity> : IRepository<TEntity> where TEntity : class, IValidatableModel, IDbModel
    {
        protected abstract DbSet<TEntity> DbSet { get; }
        protected abstract DbContext DbContext { get; }

        public async Task<IList<TEntity>> GetAllAsync(IDbFilter<TEntity>? dbFilter = null)
        {
            if (dbFilter == null)
                return await DbSet.ToListAsync();

            return await dbFilter.ApplyFilter(DbSet).ToListAsync();
        }

        public async Task<TEntity?> GetByIdAsync(Guid id)
        {
            return await DbSet.FindAsync(id);
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            entity.Validate();

            var newEntity = await DbSet.AddAsync(entity);
            await DbContext.SaveChangesAsync();

            return newEntity.Entity;
        }

        public async Task<TEntity?> ModifyAsync(Guid id, Action<TEntity> modifyCallback)
        {
            var entityToModify = await GetByIdAsync(id);
            if (entityToModify == null)
                return null;

            modifyCallback(entityToModify);
            if (entityToModify.Id != id)
                throw new DbException("Modification of entity id is not allowed");

            entityToModify.Validate();
            await DbContext.SaveChangesAsync();

            return entityToModify;
        }

        public async Task<TEntity?> DeleteById(Guid id)
        {
            var entity = await GetByIdAsync(id);
            if (entity == null)
                return null;

            var removedEntity = DbSet.Remove(entity);
            await DbContext.SaveChangesAsync();

            return removedEntity.Entity;
        }

        public Task<int> SaveChangesAsync()
        {
            return DbContext.SaveChangesAsync();
        }
    }
}
