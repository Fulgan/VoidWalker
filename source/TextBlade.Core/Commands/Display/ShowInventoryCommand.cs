using TextBlade.Core.Characters;
using TextBlade.Core.Characters.PartyManagement;
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

        yield return"Use/equip which item? Type 0 or b or back to go back.";
        
        var index = 0;
        while (index == 0)
        {        
            var rawInput = (Console.ReadLine() ?? string.Empty).Trim().ToLowerInvariant();
            if (rawInput == "0" || rawInput == "b" || rawInput == "back")
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

        var picked = inventory.ItemsInOrder.ElementAt(index - 1);
        var itemData = inventory.NameToData[picked];

        switch (itemData.ItemType)
        {
            case Inv.ItemType.Helmet:
            case Inv.ItemType.Armour:
            case Inv.ItemType.Weapon:
                foreach (var message in EquipmentEquipper.EquipIfRequested(itemData, inventory, party))
                {
                    yield return message;
                }
                break;
        }
    }
}
