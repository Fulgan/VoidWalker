using TextBlade.Core.IO;
using TextBlade.Core.Locations;

namespace TextBlade.Core.Commands;

public class QuitGameCommand : ICommand
{

    public bool Execute(IConsole console, Location currentLocation, SaveData saveData)
    {
        console.WriteLine($"Quit the game? Are you sure? [{Colours.Command}]y[/]/[{Colours.Command}]n[/]");
        var input = console.ReadKey();

        if (input != 'y')
        {
            console.WriteLine("Cancelling ...");
            return false;
        }

        console.WriteLine("Bye!");
        Environment.Exit(0);
        return true; // makes compiler go brrr
    }
}
