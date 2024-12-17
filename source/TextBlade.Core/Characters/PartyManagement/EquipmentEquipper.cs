using TextBlade.Core.Inv;
using TextBlade.Core.IO;

namespace TextBlade.Core.Characters.PartyManagement;

public class EquipmentEquipper
{
    private readonly IConsole _console;

    public EquipmentEquipper(IConsole console)
    {
        _console = console;
    }

    internal void EquipIfRequested(Item itemData, Inventory inventory, List<Character> party)
    {
        Equipment equipment = ValidateArguments(itemData, inventory, party);

        var messages = DisplayEquipmentStats(equipment, party);
        foreach (var message in messages)
        {
            _console.WriteLine(message);
        }

        _console.WriteLine("Equip for who? Or press 0 to cancel.");

        var input = 0;
        if (!int.TryParse(_console.ReadKey().ToString(), out input))
        {
            _console.WriteLine("Cancelling.");
            return;
        }

        if (input <= 0 || input > party.Count)
        {
            _console.WriteLine("Invalid number, cancelling.");
            return;
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
        _console.WriteLine(equipMessage);
    }

    private Equipment ValidateArguments(Item itemData, Inventory inventory, List<Character> party)
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

    private IEnumerable<string> DisplayEquipmentStats(Equipment itemData, List<Character> party)
    {
        var messages = new List<string>
        {
            $"{itemData.Name}. Stats: {itemData}"
        };

        var i = 0;
        foreach (var member in party)
        {
            i++;

            Equipment? currentlyEquipped = member.EquippedOn(itemData.ItemType);
            var diff = EquipmentDiffer.GetDiff(currentlyEquipped, itemData);
            
            var wearingMessage = "";
            if (currentlyEquipped != null)
            {
                wearingMessage = $", wearing {currentlyEquipped.Name}";
            }
            
            messages.Add($"    {i}: For {member.Name}{wearingMessage}: {EquipmentDiffer.DiffToString(diff)}");
        }

        return messages;
    }
}
