using Microsoft.Extensions.DependencyInjection;
using Roxana.BackgroundServices.WhatsappBusiness.Services;

namespace Roxana.BackgroundServices.WhatsappBusiness;

public static class StartupServices
{
    public static void Register(IServiceCollection services)
    {
        services.AddHostedService<ServiceWorker>();
    }
}