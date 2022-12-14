using Model.DbModels;

namespace Data.Repositories;

public class ResultRepository : GenericRepository<Result>, IResultRepository
{
    public ResultRepository(CaroDbContext context) : base(context)
    {
    }
}