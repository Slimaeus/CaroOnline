using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.UnitOfWork
{
    public interface IUnitOfWork
    {
        CaroDbContext DbContext { get; set; }
        int Commit();
        IDbContextTransaction BeginTransaction();
    }
}
