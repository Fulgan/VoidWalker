using TextBlade.Core.Battle;
using TextBlade.Core.Characters;
using TextBlade.Core.Characters.PartyManagement;
using TextBlade.Core.Game;

namespace TextBlade.Core.Commands.Display;

public class ShowInventoryCommand : ICommand
{
    private bool _isInBattle = false;

    public ShowInventoryCommand(bool isInBattle = false)
    {
        _isInBattle = isInBattle;
    }

    public IEnumerable<string> Execute(IGame game, List<Character> party)
    {
        yield return "Inventory:";

        var inventory = game.Inventory;
        var items = inventory.ItemsInOrder;
        if (_isInBattle)
        {
            items = items.Where(i => i.ItemType == Inv.ItemType.Consumable);
        }
        
        var i = 1;

        foreach (var item in items)
        {
            yield return $"  {i}: {item.Name} x{inventory.ItemQuantities[item.Name]}";
            i++;
        }

        yield return $"Use{(_isInBattle ? "" : "/equip")} which item? Type 0 or b or back to go back.";
        
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

            if (index < 1 || index > items.Count())
            {
                yield return "Please enter a valid number!";
                index = 0;
             }
        }

        var itemData = items.ElementAt(index - 1);

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
            case Inv.ItemType.Consumable:
                foreach (var message in ItemUser.UseIfRequested(itemData, inventory, party))
                {
                    yield return message;
                }
                break;
        }
    }
}
