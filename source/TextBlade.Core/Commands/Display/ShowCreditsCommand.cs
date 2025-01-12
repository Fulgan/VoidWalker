using TextBlade.Core.IO;

namespace TextBlade.Core.Commands.Display;

public class ShowCreditsCommand : ICommand
{
    public bool Execute(IConsole console, SaveData saveData)
    {
        var creditsFilePath = Path.Join("Content", "Credits.txt");
        if (File.Exists(creditsFilePath))
        {
            console.WriteLine(File.ReadAllText(creditsFilePath));
        }
        
        console.WriteLine("TextBlade text JRPG engine: programming by NightBlade.");
        return true;
    }
}
