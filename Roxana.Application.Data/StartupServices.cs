using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Roxana.Application.Data.Contextes;

namespace Roxana.Application.Data;

public static class StartupServices
{
    public static void Register(IServiceCollection services)
    {
        var connectionString = Environment.GetEnvironmentVariable("APP_DB_CONNECTION")!;
        services.AddDbContextPool<ApplicationDbContext>(options => { options.UseNpgsql(connectionString); });  
        services.AddDbContextPool<AccountDbContext>(options => { options.UseNpgsql(connectionString); });   
        services.AddDbContextPool<AuthorizeDbContext>(options => { options.UseNpgsql(connectionString); });  
    }
}