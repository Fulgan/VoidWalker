using TextBlade.Core.IO;
using TextBlade.Core.Locations;

namespace TextBlade.Core.Commands.Display;

public class ShowCreditsCommand : ICommand
{
    public bool Execute(IConsole console, Location currentLocation, SaveData saveData)
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
