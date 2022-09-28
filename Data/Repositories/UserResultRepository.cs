using Model.DbModels;

namespace Data.Repositories;

public class UserResultRepository : GenericRepository<UserResult>, IUserResultRepository
{
    public UserResultRepository(CaroDbContext context) : base(context)
    {
    }
}