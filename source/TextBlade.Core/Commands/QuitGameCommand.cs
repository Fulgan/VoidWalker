using TextBlade.Core.Characters;
using TextBlade.Core.Game;
using TextBlade.Core.IO;

namespace TextBlade.Core.Commands;

public class QuitGameCommand : ICommand
{
    public async IAsyncEnumerable<string> Execute(IGame game, List<Character> party)
    {
        yield return $"Quit the game? Are you sure? [{Colours.Command}]y[/]/[{Colours.Command}]n[/]";
        var input = Console.ReadKey();

        if (input.KeyChar != 'y' && input.KeyChar != 'Y')
        {
            yield return "Cancelling ...";
            yield break;
        }

        yield return "Bye!";
        Environment.Exit(0);
    }
}
