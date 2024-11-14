using TextBlade.Core.Commands;

namespace TextBlade.Core.Locations;

/// <summary>
/// Your regular location. Has reachable locations (sub-locations, adjacent locations, however you concieve of it.)
/// </summary>
public class Location : BaseLocation
{
    public List<LocationLink> LinkedLocations { get; set; } = new();

    public Location(string name, string description, string locationClass) : base(name, description, locationClass)
    {
    } 

    public virtual Command GetCommandFor(string input)
    {
        // Leave it up to sub-types, like inn, to handle their own input and return a command.
        return new DoNothingCommand();
    }
}

