using TextBlade.Core.Characters;
using TextBlade.Core.Game;

namespace TextBlade.Core.Commands;

public class DoNothingCommand : ICommand
{
    public void Execute(IGame game, List<Character> party)
    {
    }
}
