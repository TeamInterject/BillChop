using BillChopBE.DataAccessLayer.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BillChopBE.DataAccessLayer.Filters
{
    public interface IDbFilter<TEntity> where TEntity : class, IDbModel
    {
        List<Expression<Func<TEntity, bool>>> Filters { get; }

        IQueryable<TEntity> ApplyFilter(IQueryable<TEntity> dbSet);
    }

    public class AbstractDbFilter<TEntity> : IDbFilter<TEntity> where TEntity : class, IDbModel
    {
        public List<Expression<Func<TEntity, bool>>> Filters { get; } = new List<Expression<Func<TEntity, bool>>>();

        public IQueryable<TEntity> ApplyFilter(IQueryable<TEntity> dbSet)
        {
            Filters.ForEach(filter => dbSet = dbSet.Where(filter));
            return dbSet;
        }
    }
}
