using System.Text;
using TextBlade.Core.Inv;

namespace TextBlade.Core.Characters.PartyManagement;

public static class EquipmentEquipper
{
    internal static void EquipIfRequested(Item itemData, Inventory inventory, List<Character> party)
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

        var equipment = itemData as Equipment;
        if (equipment == null)
        {
            throw new InvalidOperationException($"Item data for {itemData.Name} doesn't seem to be Equipment. Is the $type field specified correctly in Items.json?");
        }

        DisplayEquipmentStats(equipment);
    }

    private static void DisplayEquipmentStats(Equipment itemData)
    {
        var messages = new StringBuilder();
        messages.Append($"{itemData.Name}: {itemData.Description}");
        messages.Append("Stats:");
        messages.Append(itemData.ToString());
    }
}
