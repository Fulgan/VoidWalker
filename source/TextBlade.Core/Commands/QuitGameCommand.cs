using TextBlade.Core.Characters;
using TextBlade.Core.Game;

namespace TextBlade.Core.Commands;

public class QuitGameCommand : ICommand
{
    public IEnumerable<string> Execute(IGame game, List<Character> party)
    {
        Console.WriteLine("Quit the game? Are you sure? y/n");
        var input = Console.ReadKey();

        if (input.KeyChar != 'y' && input.KeyChar != 'Y')
        {
            Console.WriteLine("Cancelling ...");
            return Array.Empty<string>();
        }

        Console.WriteLine("Bye!");
        Environment.Exit(0);
        return null; // Unreachable code. Makes compiler go brrrr.
    }
}
