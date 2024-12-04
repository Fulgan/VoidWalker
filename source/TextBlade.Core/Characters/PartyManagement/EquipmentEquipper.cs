using TextBlade.Core.Inv;

namespace TextBlade.Core.Characters.PartyManagement;

public static class EquipmentEquipper
{
    internal static IEnumerable<string> EquipIfRequested(Item itemData, Inventory inventory, List<Character> party)
    {
        Equipment equipment = ValidateArguments(itemData, inventory, party);

        var messages = DisplayEquipmentStats(equipment, party);
        foreach (var message in messages)
        {
            yield return message;
        }

        yield return "Equip for who? Or pess 0 to cancel.";

        var input = 0;
        if (!int.TryParse(Console.ReadLine().Trim(), out input))
        {
            yield return "Cancelling.";
            yield break;
        }

        if (input <= 0 || input > party.Count)
        {
            yield return "Invalid number, cancelling.";
        }

        var partyMember = party[input - 1];
        var equipped = partyMember.EquippedOn(equipment.ItemType);

        var equipMessage = $"Equipped on {partyMember.Name}";
        if (equipped != null)
        {
            equipMessage = $"{equipMessage}, replacing {equipped.Name}";
            inventory.Add(equipped);
        }

        partyMember.Equipment[itemData.ItemType] = equipment;
        inventory.Remove(equipment.Name);
        yield return equipMessage;
    }

    private static Equipment ValidateArguments(Item itemData, Inventory inventory, List<Character> party)
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

        return equipment;
    }

    private static IEnumerable<string> DisplayEquipmentStats(Equipment itemData, List<Character> party)
    {
        var messages = new List<string>
        {
            $"{itemData.Name}. Stats: {itemData}"
        };

        foreach (var member in party)
        {
            Equipment? currentlyEquipped = member.EquippedOn(itemData.ItemType);
            var diff = EquipmentDiffer.GetDiff(currentlyEquipped, itemData);
            var wearingMessage = "";
            if (currentlyEquipped != null)
            {
                wearingMessage = $", wearing {currentlyEquipped.Name}";
            }
            messages.Add($"For {member.Name}{wearingMessage}: {EquipmentDiffer.DiffToString(diff)}");
        }

        return messages;
    }
}
