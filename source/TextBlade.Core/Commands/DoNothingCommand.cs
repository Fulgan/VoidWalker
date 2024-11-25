using TextBlade.Core.Characters;
using TextBlade.Core.Game;

namespace TextBlade.Core.Commands;

public class DoNothingCommand : ICommand
{
    public IEnumerable<string> Execute(IGame game, List<Character> party)
    {
        yield return string.Empty;
    }
}
