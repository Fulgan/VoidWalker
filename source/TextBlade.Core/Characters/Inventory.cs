namespace TextBlade.Core.Characters;

public class Inventory
{
    public Dictionary<string, int> ItemQuantities = new();

    public bool Has(string item)
    {
        return ItemQuantities.ContainsKey(item);
    }

    public void Add(string item, int quantity = 1)
    {
        if (string.IsNullOrWhiteSpace(item))
        {
            throw new ArgumentException(nameof(item));
        }

        if (quantity <= 0)
        {
            throw new ArgumentException(nameof(quantity));
        }
        
        if (!ItemQuantities.ContainsKey(item))
        {
            ItemQuantities[item] = 0;
        }

        ItemQuantities[item] += quantity;
    }

    public void Remove(string item, int quantity = 1)
    {
        if (string.IsNullOrWhiteSpace(item))
        {
            throw new ArgumentException(nameof(item));
        }

        if (quantity <= 0)
        {
            throw new ArgumentException(nameof(quantity));
        }

        if (!ItemQuantities.ContainsKey(item))
        {
            throw new ArgumentException($"Can't remove {item}, we don't have any.");
        }
        else
        {
            var existingQuantity = ItemQuantities[item];
            if (existingQuantity < quantity)
            {
                throw new ArgumentException($"Can't remove {quantity} of {item}, we only have {existingQuantity}.");
            }

            ItemQuantities[item] -= quantity;
            if (ItemQuantities[item] == 0)
            {
                ItemQuantities.Remove(item);
            }
        }
    }
}
