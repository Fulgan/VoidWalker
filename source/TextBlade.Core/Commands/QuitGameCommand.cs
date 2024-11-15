using TextBlade.Core.Characters;
using TextBlade.Core.Locations;

namespace TextBlade.Core.Commands;

public class QuitGameCommand : Command
{
    public override Location? Execute(List<Character> party)
    {
        Console.WriteLine("Quit the game? Are you sure? y/n");
        var input = Console.ReadKey();

        if (input.KeyChar != 'y' && input.KeyChar != 'Y')
        {
            Console.WriteLine("Cancelling ...");
            return null;
        }

        Console.WriteLine("Bye!");
        Environment.Exit(0);
        return null; // Makes compiler go brrrr
    }
}
