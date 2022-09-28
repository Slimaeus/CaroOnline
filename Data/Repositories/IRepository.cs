using System.Linq.Expressions;

namespace Data.Repositories;

public interface IRepository<TEntity> where TEntity : class
{
    void Add(TEntity entity);
    void Update(TEntity entity);
    void Delete(TEntity entity);
    void Delete(object id);
    void Delete(Expression<Func<TEntity, bool>> filter);
    int Count(Expression<Func<TEntity, bool>> filter);
    IEnumerable<TEntity> GetList(
        Expression<Func<TEntity, bool>> filter = null!,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null!,
        string includeProperties = "",
        int skip = 0,
        int take = 0
    );
    bool Any(Expression<Func<TEntity, bool>> filter);
    TEntity GetById(object id);
    Task<TEntity> GetByIdAsync(object id);
}