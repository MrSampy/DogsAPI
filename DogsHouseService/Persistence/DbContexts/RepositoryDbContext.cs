using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.DbContexts
{
    public sealed class RepositoryDbContext : DbContext
    {
        public RepositoryDbContext(DbContextOptions options, bool ensureDeleted = false)
        : base(options)
        {
            Database.EnsureCreated();
            if (ensureDeleted)
            {
                Database.EnsureDeleted();
            }
        }

        public DbSet<Dog> Dogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Dog>(entity =>
            {                 
                entity.HasKey(b => b.Name);

                entity.Property(d => d.Color)
                    .HasMaxLength(100)
                    .IsRequired();
                
                entity.Property(d => d.TailLength)
                    .IsRequired();

                entity.Property(d => d.Weight)
                    .IsRequired();
            });
        }
    }
}
