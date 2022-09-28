using Microsoft.EntityFrameworkCore;
using Model.DbModels;

namespace Data.Repositories;

public class ResultRepository : GenericRepository<Result>, IResultRepository
{
    public ResultRepository(DbContext context) : base(context)
    {
    }
}