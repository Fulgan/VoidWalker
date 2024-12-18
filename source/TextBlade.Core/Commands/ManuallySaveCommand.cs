using TextBlade.Core.Characters;
using TextBlade.Core.IO;

namespace TextBlade.Core.Commands;

/// <summary>
/// This stinks. This command is used as a message, of sorts, to communicate to Game.
/// Because Game references TextBlade.Core, we can't reference it the other way around.
/// </summary>
public class ManuallySaveCommand : ICommand
{
    public void Execute(SaveData saveData)
    {
        // This stinks.
    }
}
