using FantasyRestServer.Models.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace FantasyRestServer.Models.Common
{
    public class FantasyDbContext : IdentityDbContext<FantasyUser>
    {
        public FantasyDbContext(DbContextOptions<FantasyDbContext> opt)
            : base(opt)
        {
        }

        public DbSet<Player> Players { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Transfer> Transfers { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            this.Seed(modelBuilder);
        }

        protected void Seed(ModelBuilder modelBuilder)
        {
            this.SeedPositions(modelBuilder);
        }

        protected void SeedPositions(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Position>().HasData(
                new Position() { ID = 1, Name = "Goalkeeper", AmountInTeam = 3 },
                new Position() { ID = 2, Name = "Defender", AmountInTeam = 6 },
                new Position() { ID = 3, Name = "Midfielder", AmountInTeam = 6 },
                new Position() { ID = 4, Name = "Attacker", AmountInTeam = 5 }
            );
        }
    }
}
