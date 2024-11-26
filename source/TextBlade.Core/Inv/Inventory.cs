namespace TextBlade.Core.Inv;

public class Inventory
{
    public Dictionary<string, int> ItemQuantities { get; set; } = new();
    public IList<string> ItemsInOrder
    {
        get { 
            var toReturn = ItemQuantities.Select(i => i.Key).ToList();
            toReturn.Sort();
            return toReturn;
        }
    }

    public bool Has(string item)
    {
        return ItemQuantities.ContainsKey(item);
    }

    public void Add(string item, int quantity = 1)
    {
        if (string.IsNullOrWhiteSpace(item))
        {
            throw new ArgumentException("Please specify a non-empty item", nameof(item));
        }

        if (quantity <= 0)
        {
            throw new ArgumentOutOfRangeException("Please specify a positive quantity,", nameof(quantity));
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
            throw new ArgumentException("Please specify a non-empty item name", nameof(item));
        }

        if (quantity <= 0)
        {
            throw new ArgumentOutOfRangeException("Please specify a positive quantity", nameof(quantity));
        }

        if (!ItemQuantities.ContainsKey(item))
        {
            throw new ArgumentException($"Can't remove {item}, we don't have any.", nameof(item));
        }
        else
        {
            var existingQuantity = ItemQuantities[item];
            if (existingQuantity < quantity)
            {
                throw new ArgumentOutOfRangeException($"Can't remove {quantity} of {item}, we only have {existingQuantity}.", nameof(quantity));
            }

            ItemQuantities[item] -= quantity;
            if (ItemQuantities[item] == 0)
            {
                ItemQuantities.Remove(item);
            }
        }
    }
}
