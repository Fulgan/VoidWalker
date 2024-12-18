using TextBlade.Core.Commands;
using TextBlade.Core.IO;
using TextBlade.Core.Locations;

namespace TextBlade.Core.Battle;

public class BattleResultsApplier
{
    private readonly IConsole _console;

    public BattleResultsApplier(IConsole console)
    {
        _console = console;
    }

    public void ApplyResultsIfBattle(ICommand command, Location currentLocation, SaveData saveData)
    {
        // Kinda a special case for battle commands. And manual save. Do nothing here, logic is handled in Game.
        if (command is ManuallySaveCommand && currentLocation is Dungeon)
        {
            return;
        }

        if (command is not IBattleCommand battleCommand)
        {
            return;
        }

        saveData.Gold += battleCommand.TotalGold;

        if (battleCommand.IsVictory)
        {
            foreach (var character in saveData.Party.Where(c => c.CurrentHealth > 0))
            {
                character.GainExperiencePoints(_console, battleCommand.TotalExperiencePoints);
            }
        }
        else
        {
            foreach (var character in saveData.Party)
            {
                character.Revive();
            }
            return;
        }

        var dungeon = currentLocation as Dungeon;
        if (dungeon == null)
        {
            return;
        }

        if (battleCommand.IsVictory)
        {
            // Wipe out the dungeon floor's inhabitants.
            var loot = dungeon.OnVictory(saveData.Inventory);
            if (loot.Any())
            {
                _console.WriteLine("Your party spies a treasure chest. You hurry over and open it. Within it, you find: ");
                foreach (var itemName in loot)
                {
                    _console.WriteLine($"    {itemName}");
                }
            }
        }
    }
}
