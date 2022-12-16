using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System;
using Microsoft.EntityFrameworkCore;

namespace FriendstagramApi.Data.Repository.Interfaces
{
    public interface IGenericRepository<TEntity>
    {
        public IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "");

        public void Insert(TEntity entity);
        public TEntity GetById(object id);
        public void Delete(object id);
        public void Delete(TEntity entityToDelete);

    }
}
