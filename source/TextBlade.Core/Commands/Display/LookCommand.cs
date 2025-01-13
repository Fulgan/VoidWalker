using TextBlade.Core.IO;
using TextBlade.Core.Locations;

namespace TextBlade.Core.Commands.Display;

/// <summary>
/// This stinks. This command is used as a message, of sorts, to communicate to Game.
/// Because Game references TextBlade.Core, we can't reference it the other way around.
/// </summary>
public class LookCommand : ICommand
{
    public bool Execute(IConsole console, Location currentLocation, SaveData saveData)
    {
        // This stinks.
        return true;
    }
}
