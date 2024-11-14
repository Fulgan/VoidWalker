using TextBlade.Core.Locations;

namespace TextBlade.Core.Commands;

public abstract class Command
{
    public abstract Location Execute();
}
