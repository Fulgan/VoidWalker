using TextBlade.Core.IO;

namespace TextBlade.Core.Commands;

public interface ICommand
{
    /// <summary>
    /// Execute a command. Passes in the latest save data.
    /// </summary>
    public void Execute(SaveData saveData);
}
