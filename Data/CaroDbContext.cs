﻿using Data.Configurations;
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
        public DbSet<Result> Results { get; set; }
        public DbSet<UserResult> UserResults { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration<Result>(new ResultConfiguration());
            builder.ApplyConfiguration<UserResult>(new UserResultConfiguration());
        }
    }
}
