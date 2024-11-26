using TextBlade.Core.Characters;
using TextBlade.Core.Game;

namespace TextBlade.Core.Commands.Display;

public class ShowInventoryCommand : ICommand
{
    public IEnumerable<string> Execute(IGame game, List<Character> party)
    {
        yield return "Inventory:";

        var inventory = game.Inventory;
        var i = 1;

        foreach (var item in inventory.ItemsInOrder)
        {
            yield return$"  {i}: {item} x{inventory.ItemQuantities[item]}";
            i++;
        }

        yield return"Use/equip which item?";
        
        var index = 0;
        while (index == 0)
        {        
            var rawInput = (Console.ReadLine() ?? string.Empty).Trim().ToLowerInvariant();
            if (rawInput == "b" || rawInput == "back")
            {
                yield break;
            }

            if (!int.TryParse(rawInput, out index))
            {
                continue;
            }

            if (index < 1 || index > inventory.ItemsInOrder.Count())
            {
                yield return "Please enter a valid number!";
                index = 0;
             }
        }

        var picked = inventory.ItemsInOrder[index - 1];
        var quantity = inventory.ItemQuantities[picked];
        var itemData = inventory.NameToData[picked];

        yield return$"You picked: {picked}, of which you have {quantity}. Data: {itemData}";
    }
}
