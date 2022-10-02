using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Data.Repositories;

public class GenericRepository<TEntity> : IRepository<TEntity> where TEntity : class
{
    protected readonly DbContext _context;
    protected readonly DbSet<TEntity> _dbSet;

    public GenericRepository(DbContext context)
    {
        _context = context;
        _dbSet = context.Set<TEntity>();
    }
    public virtual void Add(TEntity entity)
    {
        _dbSet.Add(entity);
    }

    public async Task AddAsync(TEntity entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public virtual bool Any(Expression<Func<TEntity, bool>> filter)
    {
        return _dbSet.Any(filter);
    }

    public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> filter)
    {
        return await _dbSet.AnyAsync(filter);
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

    public async Task<TEntity> GetByIdAsync(object id)
    {
        return (await _dbSet.FindAsync(id))!;
    }

    public virtual IEnumerable<TEntity> GetList(
        Expression<Func<TEntity, bool>> filter = null!, 
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null!, 
        string includeProperties = "", 
        int skip = 0, 
        int take = 0)
    {
        var query = _dbSet.AsQueryable();
        foreach (string includeProperty in includeProperties.Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
            query = query.Include(includeProperty);
        if (filter != null)
            query = query.Where(filter);
        if (skip > 0)
            query = query.Skip(skip);
        if (take > 0)
            query = query.Take(take);
        if (orderBy != null)
            query = orderBy(query);
        return query.ToList();
    }

    public async Task<IEnumerable<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> filter = null!, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null!, string includeProperties = "", int skip = 0, int take = 0)
    {
        var query = _dbSet.AsQueryable();
        foreach (string includeProperty in includeProperties.Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
            query = query.Include(includeProperty);
        if (filter != null)
            query = query.Where(filter);
        if (skip > 0)
            query = query.Skip(skip);
        if (take > 0)
            query = query.Take(take);
        if (orderBy != null)
            query = orderBy(query);
        return await query.ToListAsync();
    }

    public virtual void Update(TEntity entity)
    {
        _dbSet.Attach(entity);
        _context.Entry(entity).State = EntityState.Modified;
    }
}