using Microsoft.Extensions.DependencyInjection;
using Roxana.Application.Business.Implementations;
using Roxana.Application.Core.Contracts;
using Roxana.Application.Core.Models;

namespace Roxana.Application.Business;

public class StartupServices
{
    public static void Register(IServiceCollection services)
    {
        services.AddSingleton<IJsonService, JsonService>();
        services.AddSingleton<IServerInfo, ServerInfo>();
        services.AddTransient<IAccountService, AccountService>();
        
        Data.StartupServices.Register(services);
    }
}