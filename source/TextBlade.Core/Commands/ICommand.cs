using TextBlade.Core.IO;

namespace TextBlade.Core.Commands;

public interface ICommand
{
    /// <summary>
    /// Execute a command. Passes in the latest save data.
    /// Returns true if executed, false if not (e.g. user cancelled).
    /// </summary>
    public bool Execute(IConsole console, SaveData saveData);
}
