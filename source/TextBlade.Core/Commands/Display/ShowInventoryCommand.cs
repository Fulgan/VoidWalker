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
        
        var id = -1;
        while (id == -1)
        {        
            var rawInput = Console.ReadLine().Trim();
            if (rawInput.ToLowerInvariant() == "b" || rawInput.ToLowerInvariant() == "back")
            {
                yield break;
            }

            if (!int.TryParse(rawInput, out id))
            {
                continue;
            }

            if (id < 1 || id > inventory.ItemsInOrder.Count())
            {
                yield return"Please enter a valid number!";
                id = -1;
            }
        }

        var picked = inventory.ItemsInOrder[id];
        var quantity = inventory.ItemQuantities[picked];

        yield return$"You picked: {picked}, of which you have {quantity}";
        yield break;
    }
}
