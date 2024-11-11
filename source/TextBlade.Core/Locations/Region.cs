namespace TextBlade.Core.Locations;

public class Region : BaseRegion
{
    public IList<string> ReachableRegionIds = new List<string>();

    public Region(string name, string description) : base(name, description)
    {
    }
}
