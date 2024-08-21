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
        public DbSet<Agent> Agents { get; set; }
        public DbSet<Target> Targets { get; set; }
        public DbSet<Mission> Missions { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Mission>()
                .HasOne(m => m.Agent)
                .WithMany()
                .HasForeignKey(m => m.AgentId);

            modelBuilder.Entity<Mission>()
                .HasOne(m => m.Target)
                .WithMany()
                .HasForeignKey(m => m.TargetId);
            base.OnModelCreating(modelBuilder);
        }
    }
}