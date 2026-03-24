using FavoriteCurrency.CurrencyUpdater.Worker;
using FavoriteCurrency.CurrencyUpdater.Worker.Configuration;
using FavoriteCurrency.CurrencyUpdater.Worker.Data;
using FavoriteCurrency.CurrencyUpdater.Worker.Interfaces;
using FavoriteCurrency.CurrencyUpdater.Worker.Services;
using Microsoft.EntityFrameworkCore;
using System.Text;

Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

var builder = Host.CreateApplicationBuilder(args);

builder.Services.Configure<CbrOptions>(
    builder.Configuration.GetSection(CbrOptions.SectionName));

var connectionString = builder.Configuration.GetConnectionString("Postgres")
    ?? throw new InvalidOperationException("Connection string 'Postgres' was not found.");

builder.Services.AddDbContextFactory<CurrencyUpdaterDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddHttpClient<ICbrCurrencyClient, CbrCurrencyClient>(client =>
{
    client.Timeout = TimeSpan.FromSeconds(30);
});

builder.Services.AddScoped<ICurrencySyncService, CurrencySyncService>();

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();