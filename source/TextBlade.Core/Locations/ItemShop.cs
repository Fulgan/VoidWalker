using TextBlade.Core.IO;

namespace TextBlade.Core.Locations;

public class ItemShop : Location
{
    // Deserialized, hence public.
    // TODO: Possibly make this tuples of (item, cost) so users can override costs.
    public IEnumerable<string> Items { get; set; }
    private readonly Dictionary<string, int> _itemCosts = new();
    
    public ItemShop(string name, string description, IEnumerable<string> items, string? locationClass = null) : base(name, description, locationClass)
    {
        this.Items = items;

        if (Items == null || Items.Any())
        {
            throw new ArgumentException("Shop has no items", nameof(Items));
        }

        foreach (var itemName in Items)
        {
            var value = ItemsData.GetItem(itemName).Value;
            _itemCosts[itemName] = value;
        }
    }
}
