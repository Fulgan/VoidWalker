using TextBlade.Core.IO;
using TextBlade.Core.Locations;

namespace TextBlade.Core.Commands.Display;

public class ShowHelpCommand : ICommand
{
    private readonly Dictionary<string, string> _knownCommands = new()
    {
        { "help", "Shows this detailed help text"},
        { "look", "Check where you are"},
        { "talk", "Talk to any people present (some may have multiple things to say!)"},
        { "inv", "Open your inventory to equip or use items"},
        { "party/status", "See your party's status"},
        { "save", "Save the game"},
        { "quit", "Quits the game" },
        { "credits", "Shows the credits" },
    };

    public bool Execute(IConsole console, Location currentLocation, SaveData saveData)
    {
        var helpText = new List<string>
        {
            // If you update this, update the huge case statement in InputProcessor for commands.
            $"Each location lists other locations you can visit; use [{Colours.Command}]numbers[/] to indicate where to travel.",
            $"Some locations have location-specific keys, like [{Colours.Command}]S[/] to sleep at inns, so watch out for those.",
            "The following commands are also available:"
        };

        foreach (var t in helpText)
        {
            console.WriteLine(t);
        }

        foreach (var command in _knownCommands.Keys)
        {
            var explanation = _knownCommands[command];
            console.WriteLine($"    [{Colours.Command}]{command}[/]: {explanation}");
        }

        return true;
    }
}
