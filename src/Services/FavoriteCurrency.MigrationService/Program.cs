using FavoriteCurrency.FinanceService.Infrastructure.Persistence;
using FavoriteCurrency.UserService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("Postgres")
    ?? throw new InvalidOperationException("Connection string 'Postgres' was not found.");

builder.Services.AddDbContext<UserDbContext>(options =>
    options.UseNpgsql(connectionString, npgsql =>
        npgsql.MigrationsHistoryTable("__EFMigrationsHistory", "users")));

builder.Services.AddDbContext<FinanceDbContext>(options =>
    options.UseNpgsql(connectionString, npgsql =>
        npgsql.MigrationsHistoryTable("__EFMigrationsHistory", "finance")));

var host = builder.Build();

using var scope = host.Services.CreateScope();
var services = scope.ServiceProvider;

var userDbContext = services.GetRequiredService<UserDbContext>();
var financeDbContext = services.GetRequiredService<FinanceDbContext>();

await userDbContext.Database.MigrateAsync();
await financeDbContext.Database.MigrateAsync();

Console.WriteLine("Database migrations applied successfully.");