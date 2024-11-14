namespace TextBlade.Core.Locations;

/// <summary>
/// Base class that includes all common fields across all types of locations.
/// </summary>
public abstract class BaseLocation
{
    public string Name { get; set; }
    public string Description { get; set; }

    protected BaseLocation(string name, string description)
    {
        this.Name = name;
        this.Description = description;
    }
}