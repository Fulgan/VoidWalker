namespace TextBlade.Core.Locations;

/// <summary>
/// Your regular region. Has reachable regions (sub-regions, adjacent regions, however you concieve of it.)
/// </summary>
public class Region : BaseRegion
{
    public List<LocationLink> ReachableRegions { get; set; } = new();

    public Region(string name, string description) : base(name, description)
    {
    }
}

