using TextBlade.Core.Characters;
using TextBlade.Core.Game;

namespace TextBlade.Core.Commands;

public class ShowCreditsCommand : ICommand
{
    public IEnumerable<string> Execute(IGame game, List<Character> party)
    {
        var creditsFilePath = Path.Join("Content", "Credits.txt");
        var toReturn = new List<string>();
        if (File.Exists(creditsFilePath))
        {
            toReturn.Add(File.ReadAllText(creditsFilePath));
        }
        
        toReturn.Add("TextBlade text JRPG engine: programming by NightBlade.");
        return toReturn;
    }
}
