using TextBlade.Core.Commands;

namespace TextBlade.Core.Locations;

/// <summary>
/// Your regular location. Has reachable locations (sub-locations, adjacent locations, however you concieve of it.)
/// </summary>
public class Location : BaseLocation
{
    public List<LocationLink> LinkedLocations { get; set; } = new();
    public string LocationId { get; internal set; } = null!; // Saved so we know our location
    
    public Location(string name, string description, string? locationClass = null) : base(name, description, locationClass)
    {
    } 

    public virtual ICommand GetCommandFor(string input)
    {
        // Leave it up to sub-types, like inn, to handle their own input and return a command.
        return new DoNothingCommand();
    }

    public virtual string GetExtraDescription()
    {
        // Override for stuff like "You are in 2B, you see three Tiramisu Bettles"
        return string.Empty;
    }

    public virtual string GetExtraMenuOption()
    {
        // Override for stuff like "type f/fight to fight"
        return string.Empty;
    }
}

