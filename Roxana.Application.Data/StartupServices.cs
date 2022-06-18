using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Roxana.Application.Data.Contextes;

namespace Roxana.Application.Data;

public static class StartupServices
{
    public static void Register(IServiceCollection services)
    {
        var version = new MySqlServerVersion("8.0.29");
        var connectionString = Environment.GetEnvironmentVariable("APP_DB_CONNECTION")!;
        services.AddDbContextPool<ApplicationDbContext>(options => { options.UseMySql(connectionString, version); });  
        services.AddDbContextPool<AccountDbContext>(options => { options.UseMySql(connectionString, version); });  
        services.AddDbContextPool<AuthorizeDbContext>(options => { options.UseMySql(connectionString, version); });  
    }
}