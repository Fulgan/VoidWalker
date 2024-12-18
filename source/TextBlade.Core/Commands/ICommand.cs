using TextBlade.Core.Characters;
using TextBlade.Core.IO;

namespace TextBlade.Core.Commands;

public interface ICommand
{
    /// <summary>
    /// Execute a command. And return strings to run through console.
    /// Everything else is here so we can act on it.
    /// </summary>
    public void Execute(SaveData saveData);
}
