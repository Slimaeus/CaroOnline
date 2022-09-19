using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class GenericRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly DbContext _context;
        private readonly DbSet<TEntity> _dbSet;

        public GenericRepository(DbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }
        public virtual void Add(TEntity entity)
        {
            _dbSet.Add(entity);
        }

        public virtual bool Any(Expression<Func<TEntity, bool>> filter)
        {
            return _dbSet.Any(filter);
        }

        public virtual int Count(Expression<Func<TEntity, bool>> filter)
        {
            if (filter == null)
                return _dbSet.Count();
            return _dbSet.Count(filter);
        }

        public virtual void Delete(TEntity entity)
        {
            _dbSet.Remove(entity);
        }

        public virtual void Delete(Expression<Func<TEntity, bool>> filter)
        {
            IEnumerable<TEntity> entities = GetList(filter);
            _dbSet.RemoveRange(entities);
        }

        public void Delete(object id)
        {
            TEntity entity = GetById(id);
            Delete(entity);
        }

        public virtual TEntity GetById(object id)
        {
            return _dbSet.Find(id)!;
        }

        public virtual IEnumerable<TEntity> GetList(
            Expression<Func<TEntity, bool>> filter = null!, 
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null!, 
            string includeProperties = "", 
            int skip = 0, 
            int take = 0)
        {
            IQueryable<TEntity> query = _dbSet.AsQueryable();
            if (filter != null)
                query = query.Where(filter);
            foreach (string includeProperty in includeProperties.Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
            {
                query = query.Include(includeProperty);
            }
            if (skip > 0)
                query = query.Skip(skip);
            if (take > 0)
                query = query.Take(take);
            if (orderBy != null)
                query = orderBy(query);
            return query.ToList();
        }
        public virtual void Update(TEntity entity)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }
    }
}
