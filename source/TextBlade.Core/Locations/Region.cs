namespace TextBlade.Core.Locations;

public class Region : BaseRegion
{
    public List<LocationLink> ReachableRegions { get; set; } = new();

    public Region(string name, string description) : base(name, description)
    {
    }
}
