using TextBlade.Core.Characters;
using TextBlade.Core.Game;

namespace TextBlade.Core.Commands;

public class DoNothingCommand : ICommand
{
    public IEnumerable<string> Execute(IGame game, List<Character> party)
    {
        return new string[0];
    }
}
