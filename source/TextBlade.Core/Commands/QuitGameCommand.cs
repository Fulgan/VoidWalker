using TextBlade.Core.IO;

namespace TextBlade.Core.Commands;

public class QuitGameCommand : ICommand
{
    private readonly IConsole _console;

    public QuitGameCommand(IConsole console)
    {
        _console = console;
    }

    public bool Execute(SaveData saveData)
    {
        _console.WriteLine($"Quit the game? Are you sure? [{Colours.Command}]y[/]/[{Colours.Command}]n[/]");
        var input = _console.ReadKey();

        if (input != 'y')
        {
            _console.WriteLine("Cancelling ...");
            return false;
        }

        _console.WriteLine("Bye!");
        Environment.Exit(0);
        return true; // makes compiler go brrr
    }
}
