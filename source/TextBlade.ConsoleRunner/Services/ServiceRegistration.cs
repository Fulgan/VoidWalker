using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using TextBlade.ConsoleRunner.Audio;
using TextBlade.ConsoleRunner.IO;
using TextBlade.Core.Audio;
using TextBlade.Core.Game;
using TextBlade.Core.IO;

namespace TextBlade.ConsoleRunner
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddTextBlade(this IServiceCollection services)
        {            
            services.TryAddSingleton<IConsole, TextConsole>(); // Keyboard input and coloured output
            services.TryAddSingleton<ISoundPlayer, AudioPlayerWrapper>();
            services.TryAddSingleton<ISerialSoundPlayer, SerialSoundPlayer>();
            services.TryAddSingleton<IGame, Game>();
            services.TryAddSingleton<NewGameRunner>();
            return services;
        }
    }
}
