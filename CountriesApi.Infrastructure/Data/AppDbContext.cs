using CountriesApi.Domain.Entites;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace CountriesApi.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Country> Countries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Country>(entity =>
            {
                entity.Property(e => e.Capital)
                    .IsRequired(false);
            });
        }
    }
}
