using Microsoft.EntityFrameworkCore;
using Model.DbModels;

namespace Data.Repositories;

public class UserResultRepository : GenericRepository<UserResult>, IUserResultRepository
{
    public UserResultRepository(DbContext context) : base(context)
    {
    }
}