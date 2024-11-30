using Microsoft.Extensions.DependencyInjection;
using TextBlade.ConsoleRunner;
using TextBlade.Core.Game;


internal class Program
{
    public static void Main(string[] args)
    {
        var services = new ServiceCollection();
        ConfigureServices(services);
        var serviceProvider = services.BuildServiceProvider();
        serviceProvider.GetRequiredService<IGame>().Run();
    }
    private static void ConfigureServices(IServiceCollection services)
    {
        services.AddTextBlade();
        // Register other services and dependencies here
    }
}