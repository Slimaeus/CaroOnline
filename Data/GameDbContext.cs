using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    public class GameUser
    {
        [Key]
        public string UserName { get; set; } = default!;
        public ICollection<Connection> Connections { get; set; } = new List<Connection>();
        public virtual ICollection<PlayRoom> Rooms { get; set; } = new List<PlayRoom>();
    }

    public class Connection
    {
        public string ConnectionID { get; set; } = default!;
        public string UserAgent { get; set; } = default!;
        public bool Connected { get; set; } = default!;
    }

    public class PlayRoom
    {
        [Key]
        public string RoomName { get; set; } = default!;
        public int RoomMax { get; set; } = 2;
        public virtual ICollection<GameUser> GameUsers { get; set; } = new List<GameUser>();
    }
}
