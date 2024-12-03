namespace TextBlade.Core.Inv;

public class Item
{
    public string Name { get; set; } // Set after serialization
    public readonly string Description; // AKA flavour text
    public readonly ItemType ItemType;
    public readonly int Value; // Value, aka cost.

    public Item(string name, string description, string itemType, int value)
    {
        // Name is null because we don't duplicate it in our JSON.
        Name = name;
        Description = description;
        Value = value;
        
        if (!Enum.TryParse<ItemType>(itemType, out ItemType))
        {
            throw new InvalidOperationException($"Can't deserialize {name}; item type is invalid: {itemType}");
        }
    }
}
