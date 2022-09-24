using Data.Configurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Model.DbModels;

namespace Data
{
    public class CaroDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        public CaroDbContext(DbContextOptions<CaroDbContext> options, DbSet<Result> results, DbSet<UserResult> userResults) : base(options)
        {
            Results = results;
            UserResults = userResults;
        }
        public DbSet<Result> Results { get; }
        public DbSet<UserResult> UserResults { get; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration<Result>(new ResultConfiguration());
            builder.ApplyConfiguration<UserResult>(new UserResultConfiguration());
        }
    }
}
