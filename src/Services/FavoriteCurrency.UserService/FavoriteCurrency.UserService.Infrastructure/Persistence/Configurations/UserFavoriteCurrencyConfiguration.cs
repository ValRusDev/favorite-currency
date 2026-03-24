using FavoriteCurrency.UserService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FavoriteCurrency.UserService.Infrastructure.Persistence.Configurations
{
    public sealed class UserFavoriteCurrencyConfiguration : IEntityTypeConfiguration<UserFavoriteCurrency>
    {
        public void Configure(EntityTypeBuilder<UserFavoriteCurrency> builder)
        {
            builder.ToTable("user_favorite_currencies");

            builder.HasKey(x => new { x.UserId, x.CurrencyCode });

            builder.Property(x => x.CurrencyCode)
                .HasMaxLength(10)
                .IsRequired();
        }
    }
}
