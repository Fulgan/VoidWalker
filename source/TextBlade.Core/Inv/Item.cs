namespace TextBlade.Core.Inv;

public class Item
{
    public string Name { get; set; } // Set after serialization
    public readonly string Description; // AKA flavour text
    public readonly ItemType ItemType;
    public readonly int Value; // Value, aka cost.

    public Item(string name, string description, string itemType, int value = 1)
    {
        // Name is null because we don't duplicate it in our JSON.
        Name = name;
        Description = description;
        Value = value;
        
        if (!Enum.TryParse(itemType, out ItemType))
        {
            throw new InvalidOperationException($"Can't deserialize {name}; item type is invalid: {itemType}");
        }
    }

    /// <summary>
    /// Returns the audio file name, if the file exists.
    /// Otherwise, returns empty string.
    /// Assumes a lot of things...
    /// </summary>
    internal string GetAudioFileName()
    {
        // Assumes too much.
        var toReturn = Path.Join("Content", "Audio", "sfx", "items", $"{Name.ToLower().Replace(' ', '-')}.wav");
        return File.Exists(toReturn) ? toReturn : string.Empty;
    }
}
