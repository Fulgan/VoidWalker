using System.Runtime.InteropServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using TextBlade.Core.Game;
using TextBlade.Core.Services;
using TextBlade.Platform.Windows.Audio;

namespace TextBlade.ConsoleRunner
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddTextBlade(this IServiceCollection services)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                // windows-specific services
                services.TryAddSingleton<ISoundPlayer, NAudioSoundPlayer>(); // sound player
            } else
            {
                services.TryAddSingleton<ISoundPlayer, NullSoundPlayer>(); // null implementation if nothing is found
            }
            services.TryAddSingleton<IGame, Game>();
            services.TryAddSingleton<NewGameRunner>();
            return services;
        }
    }
}
