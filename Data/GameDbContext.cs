﻿using Microsoft.EntityFrameworkCore;
using Model.GameModels;

namespace Data
{
    public class GameDbContext : DbContext
    {
        public GameDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<GameUser> GameUsers { get; set; } = default!;
        public DbSet<Connection> Connections { get; set; } = default!;
        public DbSet<PlayRoom> Rooms { get; set; } = default!;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
