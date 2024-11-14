using TextBlade.Core.Characters;
using TextBlade.Core.Locations;

namespace TextBlade.Core.Commands;

public class DoNothingCommand : Command
{
    public override Location? Execute(List<Character> party)
    {
        return null;
    }
}
