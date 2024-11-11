namespace TextBlade.Core.Locations;

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