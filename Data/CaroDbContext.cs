using Data.Configurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Model.DbModels;

namespace Data;

public class CaroDbContext : IdentityDbContext<AppUser, AppRole, Guid>
{
    public CaroDbContext(DbContextOptions<CaroDbContext> options) : base(options)
    {
    }
    public DbSet<Result> Results { get; set; }
    public DbSet<UserResult> UserResults { get; set; }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfiguration(new ResultConfiguration());
        builder.ApplyConfiguration(new UserResultConfiguration());

        builder.Entity<AppRole>().HasData(
            new AppRole { Id = Guid.NewGuid(), Name = "Admin", NormalizedName = "ADMIN", Description = "Admin Role" }, 
            new AppRole { Id = Guid.NewGuid(), Name = "Manager", NormalizedName = "MANAGER", Description = "Manager Role" }
        );
    }
}