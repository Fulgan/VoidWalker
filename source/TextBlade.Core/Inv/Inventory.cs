namespace TextBlade.Core.Inv;

public class Inventory
{
    public Dictionary<string, int> ItemQuantities { get; set; } = new();
    public Dictionary<string, Item> NameToData { get; set; } = new();

    internal IEnumerable<string> ItemsInOrder
    {
        get
        { 
            // Order by item type, then by name
            return NameToData.OrderBy(kvp => kvp.Value.ItemType).ThenBy(kvp => kvp.Key).Select(s => s.Key);
        }
    }

    public bool Has(string itemName)
    {
        return NameToData.ContainsKey(itemName);
    }

    public void Add(Item item, int quantity = 1)
    {
        if (item == null)
        {
            throw new ArgumentException("Please specify a non-empty item", nameof(item));
        }
        else if (string.IsNullOrWhiteSpace(item.Name))
        {
            throw new ArgumentException("Please specify a non-empty item name", nameof(item.Name));
        }

        if (quantity <= 0)
        {
            throw new ArgumentOutOfRangeException("Please specify a positive quantity,", nameof(quantity));
        }
        
        if (!NameToData.ContainsKey(item.Name))
        {
            NameToData[item.Name] = item;
        }
        if (!ItemQuantities.ContainsKey(item.Name))
        {
            ItemQuantities[item.Name] = 0;
        }

        ItemQuantities[item.Name] += quantity;
    }

    public void Remove(string itemName, int quantity = 1)
    {
        if (string.IsNullOrWhiteSpace(itemName))
        {
            throw new ArgumentException("Please specify a non-empty item", nameof(itemName));
        }

        if (quantity <= 0)
        {
            throw new ArgumentOutOfRangeException("Please specify a positive quantity", nameof(quantity));
        }

        if (!ItemQuantities.ContainsKey(itemName))
        {
            throw new ArgumentException($"Can't remove {itemName}, we don't have any.", nameof(itemName));
        }
        else
        {
            var existingQuantity = ItemQuantities[itemName];
            if (existingQuantity < quantity)
            {
                throw new ArgumentOutOfRangeException($"Can't remove {quantity} of {itemName}, we only have {existingQuantity}.", nameof(quantity));
            }

            ItemQuantities[itemName] -= quantity;
            if (ItemQuantities[itemName] == 0)
            {
                ItemQuantities.Remove(itemName);
                NameToData.Remove(itemName);
            }
        }
    }
}
