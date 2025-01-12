using TextBlade.Core.Audio;
using TextBlade.Core.Battle;
using TextBlade.Core.Characters.PartyManagement;
using TextBlade.Core.IO;

namespace TextBlade.Core.Commands.Display;

public class ShowInventoryCommand : ICommand
{
    private readonly IConsole _console;
    private readonly ISoundPlayer _soundPlayer;

    private readonly EquipmentEquipper _equipper;
    private readonly ItemUser _itemUser;

    private bool _isInBattle = false;

    public ShowInventoryCommand(IConsole console, ISoundPlayer soundPlayer, bool isInBattle = false)
    {
        ArgumentNullException.ThrowIfNull(console);
        ArgumentNullException.ThrowIfNull(soundPlayer);

        _console = console;
        _soundPlayer = soundPlayer;

        _equipper = new(console);
        _itemUser = new(console, soundPlayer);

        _isInBattle = isInBattle;
    }

    public bool Execute(IConsole console, SaveData saveData)
    {
        _console.WriteLine("Inventory:");

        var inventory = saveData.Inventory;
        var items = inventory.ItemsInOrder;
        if (_isInBattle)
        {
            items = items.Where(i => i.ItemType == Inv.ItemType.Consumable);
        }
        
        var i = 1;

        foreach (var item in items)
        {
            _console.WriteLine($"  {i}: {item.Name} x{inventory.ItemQuantities[item.Name]}");
            i++;
        }

        _console.WriteLine($"Use{(_isInBattle ? "" : "/equip")} which item? Type 0 or b or back to go back.");
        
        var index = 0;
        while (index == 0)
        {        
            var rawInput = _console.ReadKey();
            if (rawInput == '0' || rawInput == 'b')
            {
                _console.WriteLine($"[{Colours.Cancel}]Cancelling.[/]");
                return false;
            }

            if (!int.TryParse(rawInput.ToString(), out index))
            {
                _console.WriteLine("That's not a valid number.");
            }

            if (index < 1 || index > items.Count())
            {
                _console.WriteLine($"Please enter a number between {1} and {items.Count()}.");
                index = 0;
             }
        }

        var itemData = items.ElementAt(index - 1);

        switch (itemData.ItemType)
        {
            case Inv.ItemType.Helmet:
            case Inv.ItemType.Armour:
            case Inv.ItemType.Weapon:
                return _equipper.EquipIfRequested(itemData, inventory, saveData.Party);
            case Inv.ItemType.Consumable:
                return _itemUser.UseIfRequested(itemData, inventory, saveData.Party);
        }

        return true;
    }
}
