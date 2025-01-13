using TextBlade.Core.IO;
using TextBlade.Core.Locations;

namespace TextBlade.Core.Commands;

public class DoNothingCommand : ICommand
{
    public bool Execute(IConsole console, Location currentLocation, SaveData saveData)
    {
        return true;
    }
}
