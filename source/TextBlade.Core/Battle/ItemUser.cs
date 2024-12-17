using TextBlade.Core.Characters;
using TextBlade.Core.Inv;
using TextBlade.Core.IO;

namespace TextBlade.Core.Battle;

public class ItemUser
{
    private readonly IConsole _console;

    public ItemUser(IConsole console)
    {
        _console = console;
    }

    internal IEnumerable<string> UseIfRequested(Item itemData, Inventory inventory, List<Character> party)
    {
        Consumable consumable = ValidateArguments(itemData, inventory, party);
        
        var healthMessage = consumable.RestoreHealth > 0 ? $"Restores {consumable.RestoreHealth} health" : "";
        var skillPointsMessage = consumable.RestoreSkillPoints > 0 ? $"Restores {consumable.RestoreHealth} skill points" : "";
        var message = healthMessage;
        
        if (skillPointsMessage != "")
        {
            message += ", " + skillPointsMessage;
        }

        yield return message;

        int i = 0;
        foreach (var member in party)
        {
            i++;
            yield return $"    {i}: {member}";
        }

        yield return "Use on who? Or press 0 to cancel.";

        var input = 0;
        if (!int.TryParse(_console.ReadKey().ToString(), out input))
        {
            yield return "Cancelling.";
            yield break;
        }

        if (input <= 0 || input > party.Count)
        {
            yield return "Invalid number, cancelling.";
            yield break;
        }

        var partyMember = party[input - 1];
        // Do not allow over-healing
        partyMember.CurrentHealth = Math.Min(partyMember.CurrentHealth + consumable.RestoreHealth, partyMember.TotalHealth);
        partyMember.CurrentSkillPoints = Math.Min(partyMember.CurrentSkillPoints + consumable.RestoreSkillPoints, partyMember.TotalSkillPoints);

        inventory.Remove(consumable.Name);
        yield return "Healed.";
    }

    private Consumable ValidateArguments(Item itemData, Inventory inventory, List<Character> party)
    {
        if (itemData == null)
        {
            throw new ArgumentException("Item data is missing", nameof(itemData));
        }

        if (itemData.ItemType != ItemType.Consumable)
        {
            throw new ArgumentException("Can't use non-consumables!");
        }

        if (inventory == null)
        {
            throw new ArgumentException("Inventory is missing", nameof(inventory));
        }

        if (party == null || party.Count == 0)
        {
            throw new ArgumentException("Party is missing", nameof(party));
        }

        var consumable = itemData as Consumable;
        if (consumable == null)
        {
            throw new InvalidOperationException($"Item data for {itemData.Name} doesn't seem to be a Consumable. Is the $type field specified correctly in Items.json?");
        }

        return consumable;
    }
}
