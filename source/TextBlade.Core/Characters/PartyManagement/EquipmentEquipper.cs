using System.Text;
using TextBlade.Core.Inv;

namespace TextBlade.Core.Characters.PartyManagement;

public static class EquipmentEquipper
{
    internal static void EquipIfRequested(Equipment itemData, Inventory inventory, List<Character> party)
    {
        if (itemData == null)
        {
            throw new ArgumentException("Item data is missing", nameof(itemData));
        }

        if (itemData.ItemType == ItemType.Consumable)
        {
            throw new ArgumentException("Can't equip consumables!");
        }

        if (inventory == null)
        {
            throw new ArgumentException("Inventory is missing", nameof(inventory));
        }

        if (party == null || party.Count == 0)
        {
            throw new ArgumentException("Party is missing", nameof(party));
        }

        DisplayEquipmentStats(itemData);
    }

    private static void DisplayEquipmentStats(Equipment itemData)
    {
        var messages = new StringBuilder();
        messages.Append($"{itemData.Name}: {itemData.Description}");
        messages.Append("Stats:");
        messages.Append(itemData.ToString());
    }
}
