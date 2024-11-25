using TextBlade.Core.Characters;
using TextBlade.Core.Game;

namespace TextBlade.Core.Commands;

public class ShowCreditsCommand : ICommand
{
    public IEnumerable<string> Execute(IGame game, List<Character> party)
    {
        var creditsFilePath = Path.Join("Content", "Credits.txt");
        if (File.Exists(creditsFilePath))
        {
            yield return File.ReadAllText(creditsFilePath);
        }
        
        yield return "TextBlade text JRPG engine: programming by NightBlade.";
    }
}
