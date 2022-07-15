
using Microsoft.Extensions.Hosting;

namespace Roxana.BackgroundServices.WhatsappBusiness.Services;

public class ServiceWorker : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Console.WriteLine("ServiceWorker Executed");
        return Task.CompletedTask;
    }
}