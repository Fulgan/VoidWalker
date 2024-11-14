namespace TextBlade.Core.Locations;

/// <summary>
/// Base class that includes all common fields across all types of regions.
/// </summary>
public abstract class BaseRegion
{
    public string Name { get; set; }
    public string Description { get; set; }

    protected BaseRegion(string name, string description)
    {
        this.Name = name;
        this.Description = description;
    }
}