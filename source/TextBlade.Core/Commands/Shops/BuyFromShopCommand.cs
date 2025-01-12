using System;
using TextBlade.Core.IO;

namespace TextBlade.Core.Commands.Shops;

public class BuyFromShopCommand : ICommand
{
    private IEnumerable<string> _items;
    private Dictionary<string, int> _itemCosts;

    public BuyFromShopCommand(IEnumerable<string> items, Dictionary<string, int> itemCosts)
    {
        _items = items;
        _itemCosts = itemCosts;
    }

    public bool Execute(IConsole console, SaveData saveData)
    {
        bool isDone = false;

        while (!isDone)
        {
            // Assumes less than ten items
            console.WriteLine($"What do you want to buy? Enter a number from 1 to {_items.Count()}");
            var input = console.ReadKey();

            int number;
            if (!int.TryParse(input.ToString(), out number))
            {
                console.WriteLine("That's not a number!");
                continue;
            }

            if (number == 0)
            {
                console.WriteLine("Cancelling.");
                return false;
            }

            if (number < 1 || number >= _items.Count())
            {
                console.WriteLine("There's no item with that number!");
                continue;
            }

            var selectedItem = _items.ElementAt(number - 1);
            var cost = _itemCosts[selectedItem];
            var gold = saveData.Gold;

            if (gold < cost)
            {
                console.WriteLine($"You can't afford that! You have only {gold} gold.");
                continue;
            }

            saveData.Gold -= cost;

            var item = ItemsData.GetItem(selectedItem);
            saveData.Inventory.Add(item);
            
            console.WriteLine($"Purchased! You have {saveData.Gold} gold left.");
            return true;
        }

        return true; // Makes compiler go brrr
    }
}
