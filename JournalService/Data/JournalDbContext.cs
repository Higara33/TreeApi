using Common.Models;
using Microsoft.EntityFrameworkCore;

namespace JournalService.Data
{
    public class JournalDbContext : DbContext
    {
        public JournalDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Journal> JournalEntries => Set<Journal>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Journal>()
                .HasKey(j => j.EventId);

            modelBuilder.Entity<Journal>()
                .Property(j => j.EventId)
                .ValueGeneratedOnAdd();

            base.OnModelCreating(modelBuilder);
        }
    }
}
