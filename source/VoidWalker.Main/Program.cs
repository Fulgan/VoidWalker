using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TextBlade.ConsoleRunner;
using TextBlade.Core.Game;


namespace VoidWalker;

internal class Program
{
    public static void Main(string[] args)
    {
        using var host = Host.CreateDefaultBuilder(args)
            .ConfigureServices((_, services) =>
            {
                ConfigureServices(services);
            })
            .Build();
        host.Services.GetRequiredService<IGame>().Run();
    }
    private static void ConfigureServices(IServiceCollection services)
    {
        services.AddTextBlade();
        // Register other services and dependencies here
    }
}