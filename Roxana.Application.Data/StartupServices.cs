using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Roxana.Application.Data.Contextes;

namespace Roxana.Application.Data;

public static class StartupServices
{
    public static void Register(IServiceCollection services)
    {
        var connectionString = Environment.GetEnvironmentVariable("APP_DB_CONNECTION")!;
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseMySql(connectionString, new MySqlServerVersion("8.0.29"));
        });  
    }
}