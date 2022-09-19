using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        public DbContext DbContext { get; set; }
        public UnitOfWork(IDbContextFactory<CaroDbContext> dbContextFactory)
        {
            DbContext = dbContextFactory.CreateDbContext();
        }

        public void Commit()
        {
            DbContext.SaveChanges();
        }

        public IDbContextTransaction BeginTransaction()
        {
            return DbContext.Database.BeginTransaction();
        }
    }
}
