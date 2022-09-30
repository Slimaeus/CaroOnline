using Microsoft.EntityFrameworkCore.Storage;

namespace Data.UnitOfWork;

public interface IUnitOfWork
{
    CaroDbContext DbContext { get; set; }
    int Commit();
    Task<int> CommitAsync();
    IDbContextTransaction BeginTransaction();
}