namespace TextBlade.Core.Locations;

/// <summary>
/// Your regular location. Has reachable locations (sub-locations, adjacent locations, however you concieve of it.)
/// </summary>
public class Location : BaseLocation
{
    public List<LocationLink> LinkedLocations { get; set; } = new();

    public Location(string name, string description) : base(name, description)
    {
    }

    public virtual void HandleInput(string input)
    {
        // Leave it up to sub-types, like inn, to handle their own input.
    }
}

