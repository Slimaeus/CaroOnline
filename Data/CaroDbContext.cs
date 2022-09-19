using Data.Configurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Model.DbModels;

namespace Data
{
    public class CaroDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        public CaroDbContext(DbContextOptions<CaroDbContext> options) : base(options)
        {
        }
        DbSet<Result> Results { get; set; } = default!;
        DbSet<UserResult> UserResults { get; set; } = default!;
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration<Result>(new ResultConfiguration());
            builder.ApplyConfiguration<UserResult>(new UserResultConfiguration());
        }
    }
}
