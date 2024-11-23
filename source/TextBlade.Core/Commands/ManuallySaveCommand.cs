using TextBlade.Core.Characters;
using TextBlade.Core.Game;

namespace TextBlade.Core.Commands;

/// <summary>
/// This stinks. This command is used as a message, of sorts, to communicate to Game.
/// Because Game references TextBlade.Core, we can't reference it the other way around.
/// </summary>
public class ManuallySaveCommand : ICommand
{
    public IEnumerable<string> Execute(IGame game, List<Character> party)
    {
        // This stinks.
        return Array.Empty<string>();
    }
}
