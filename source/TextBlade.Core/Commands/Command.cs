using TextBlade.Core.Characters;
using TextBlade.Core.Locations;

namespace TextBlade.Core.Commands;

public abstract class Command
{
    /// <summary>
    /// Execute a command. And return a new location (if changed), else null.
    /// Everything else is here so we can act on it.
    /// </summary>
    public abstract Location? Execute(List<Character> party);
}
