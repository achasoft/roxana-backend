using Microsoft.Extensions.DependencyInjection;
using Roxana.BackgroundServices.WhatsappPersonal.Services;

namespace Roxana.BackgroundServices.WhatsappPersonal;

public static class StartupServices
{
    public static void Register(IServiceCollection services)
    {
        services.AddHostedService<ServiceWorker>();
    }
}