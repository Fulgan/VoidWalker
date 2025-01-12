using TextBlade.Core.IO;

namespace TextBlade.Core.Commands;

public class DoNothingCommand : ICommand
{
    public bool Execute(IConsole console, SaveData saveData)
    {
        return true;
    }
}
