using FavoriteCurrency.CurrencyUpdater.Worker.Data.Configurations;
using FavoriteCurrency.CurrencyUpdater.Worker.Entities;
using Microsoft.EntityFrameworkCore;

namespace FavoriteCurrency.CurrencyUpdater.Worker.Data
{
    public sealed class CurrencyUpdaterDbContext : DbContext
    {
        public CurrencyUpdaterDbContext(DbContextOptions<CurrencyUpdaterDbContext> options)
            : base(options)
        {
        }

        public DbSet<CurrencyRecord> Currencies => Set<CurrencyRecord>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("finance");
            modelBuilder.ApplyConfiguration(new CurrencyConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
