using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Roxana.BackgroundServices.WhatsappBusiness;

public static class Program
{
    public static void Main(string[] args)
    {
        var host = Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                StartupServices.Register(services);
                Application.Business.StartupServices.Register(services);
            })
            .Build();
        using (var scope = host.Services.CreateScope())
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        host.Run();
    }
}