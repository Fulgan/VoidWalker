using TextBlade.Core.Characters;
using TextBlade.Core.Game;
using TextBlade.Core.IO;

namespace TextBlade.Core.Commands.Display;

public class ShowHelpCommand : ICommand
{
    private readonly Dictionary<string, string> _knownCommands = new()
    {
        { "help", "Shows this detailed help text"},
        { "quit", "Quits the game" },
        { "credits", "Shows the credits" },
    };

    public IEnumerable<string> Execute(IGame game, List<Character> party)
    {
        var toReturn = new List<string>
        {
            // If you update this, update the huge case statement in InputProcessor for commands.
            $"Each location lists other locations you can visit; use [{Colours.Command}]numbers[/] to indicate where to travel.",
            "Some locations have location-specific keys, like [{Colours.Command}]S[/] to sleep at inns, so watch out for those.",
            "The following commands are also available:"
        };

        foreach (var t in toReturn)
        {
            yield return t; 
        }

        foreach (var command in _knownCommands.Keys)
        {
            var explanation = _knownCommands[command];
            yield return $"    [{Colours.Command}]{command}[/]: {explanation}";
        }
    }
}
