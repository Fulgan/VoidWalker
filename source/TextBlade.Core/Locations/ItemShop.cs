using System.Text;
using TextBlade.Core.Audio;
using TextBlade.Core.Commands;
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

        if (Items == null || !Items.Any())
        {
            throw new ArgumentException("Shop has no items", nameof(Items));
        }

        foreach (var itemName in Items)
        {
            var value = ItemsData.GetItem(itemName).Value;
            _itemCosts[itemName] = value;
        }
    }

    public override string GetExtraDescription()
    {
        var message = new StringBuilder();
        message.AppendLine("Items for sale:");
        var i = 0;

        foreach (var item in Items)
        {
            i++;
            message.AppendLine($"   {i}: {item}: {_itemCosts[item]} gold");
        }

        message.AppendLine($"You have {CurrentSaveData.Gold} gold.");
        return message.ToString();
    }

    public override string GetExtraMenuOptions()
    {
        return "Type \"B item number\" to buy that item.";
    }

    public override ICommand GetCommandFor(IConsole console, ISerialSoundPlayer serialSoundPlayer, ISoundPlayer soundPlayer, string input)
    {
        input = input.ToLower();
        if (!input.StartsWith("b "))
        {
            return new DoNothingCommand();
        }

        int number;
        if (!int.TryParse(input.Substring(input.IndexOf(' ')).Trim(), out number))
        {
            console.WriteLine("That's not a number!");
            return new DoNothingCommand();
        }

        if (number <= 0 || number > _itemCosts.Count)
        {
            console.WriteLine("There's no item with that number!");
            return new DoNothingCommand();
        }

        var selectedItem = Items.ElementAt(number - 1);
        var cost = _itemCosts[selectedItem];
        var gold = CurrentSaveData.Gold;

        if (gold < cost)
        {
            console.WriteLine($"You can't afford that! You have only {gold} gold.");
            return new DoNothingCommand();
        }

        CurrentSaveData.Gold -= cost;

        var item = ItemsData.GetItem(selectedItem);
        CurrentSaveData.Inventory.Add(item);
        
        console.WriteLine($"Purchased! You have {CurrentSaveData.Gold} gold left.");
        return new DoNothingCommand();
    }
}
