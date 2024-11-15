using TextBlade.Core.Characters;
using TextBlade.Core.Locations;

namespace TextBlade.Core.Commands;

public class ShowHelpCommand : Command
{
    public override Location? Execute(List<Character> party)
    {
        // If you update this, update the huge case statement in InputProcessor for commands.
        Console.WriteLine("Travel to locations and use numbers to indicate what option you want to go with.");
        Console.WriteLine("The following commands are also available: help, quit.");
        return null;
    }
}
