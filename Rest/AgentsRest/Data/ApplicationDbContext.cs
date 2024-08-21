using AgentsRest.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace AgentsRest.Data
{
    public class ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        IConfiguration configuration) 
        : DbContext(options)
    {
        public DbSet<Point> Points { get; set; }
        public DbSet<Agent> Agents { get; set; }
        public DbSet<Target> Targets { get; set; }
        public DbSet<Mission> Missions { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Agent>()
                .HasOne(a => a.Position);

            modelBuilder.Entity<Target>()
                .HasOne(t => t.Position);

            modelBuilder.Entity<Agent>()
                .HasMany(agent => agent.Missions)
                .WithOne(mission => mission.Agent)
                .HasForeignKey(mission => mission.AgentId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Target>()
                .HasMany(agent => agent.Missions)
                .WithOne(mission => mission.Target)
                .HasForeignKey(mission => mission.TargetId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}