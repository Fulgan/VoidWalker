using TextBlade.Core.Characters;
using TextBlade.Core.Game;
using TextBlade.Core.IO;

namespace TextBlade.Core.Commands.Display;

public class ShowCreditsCommand : ICommand
{
    private readonly IConsole _console;

    public ShowCreditsCommand(IConsole console)
    {
        _console = console;
    }
    
    public void Execute(IGame game, List<Character> party)
    {
        var creditsFilePath = Path.Join("Content", "Credits.txt");
        if (File.Exists(creditsFilePath))
        {
            _console.WriteLine(File.ReadAllText(creditsFilePath));
        }
        
        _console.WriteLine("TextBlade text JRPG engine: programming by NightBlade.");
    }
}
