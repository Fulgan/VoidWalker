namespace TextBlade.Core.Locations;

public class Region : BaseRegion
{
    public List<LocationLink> ReachableRegions = new();

    public Region(string name, string description, List<LocationLink> reachableRegions) : base(name, description)
    {
        this.ReachableRegions = reachableRegions;
    }
}
