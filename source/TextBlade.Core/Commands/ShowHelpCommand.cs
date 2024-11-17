using TextBlade.Core.Characters;
using TextBlade.Core.Locations;

namespace TextBlade.Core.Commands;

public class ShowHelpCommand : Command
{
    private readonly Dictionary<string, string> _knownCommands = new()
    {
        { "help", "Shows this detailed help text"},
        { "quit", "Quits the game" },
    };

    public override Location? Execute(List<Character> party)
    {
        // If you update this, update the huge case statement in InputProcessor for commands.
        Console.WriteLine("Each location lists other locations you can visit; use numbers to indicate where to travel.");
        Console.WriteLine("Some locations have location-specific keys, like S to sleep at inns, so watch out for those.");
        Console.WriteLine("The following commands are also available:");
        foreach (var command in _knownCommands.Keys)
        {
            var explanation = _knownCommands[command];
            Console.WriteLine($"    {command}: {explanation}");
        }
        return null;
    }
}
