using System.Net;

namespace Roxana.Endpoints.WhatsappPersonal.Engine;

public class Program
{
    public static void Main(string[] args)
    {
        var host = BuildWebHost(args);
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

    private static IHost BuildWebHost(string[] args)
    {
        var config = new ConfigurationBuilder().AddCommandLine(args).Build();
        var ip = config.GetValue<string>("ip") ?? "0.0.0.0";
        var httpPort = config.GetValue<int?>("port") ?? 5021;
        return Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder
                    .ConfigureAppConfiguration((hostingContext, cfg) => { })
                    .ConfigureLogging(logging =>
                    {
                        logging.ClearProviders();
                        logging.AddConsole();
                    })
                    .UseKestrel(options =>
                    {
                        options.Limits.MaxRequestBodySize = 1048576000; //1024MB
                        options.Listen(IPAddress.Parse(ip), httpPort);
                    })
                    .UseStartup<Startup>();
            }).Build();
    }
}