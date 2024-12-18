using TextBlade.Core.Characters;
using TextBlade.Core.IO;

namespace TextBlade.Core.Commands;

public class DoNothingCommand : ICommand
{
    public void Execute(SaveData saveData)
    {
    }
}
