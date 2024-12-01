using TextBlade.Core.Commands;
using TextBlade.Core.IO;
using TextBlade.Core.Locations;

namespace TextBlade.Core.Battle;

public static class BattleResultsApplier
{
    public static Dictionary<string, object>? ApplyResultsIfBattle(ICommand command, Location currentLocation, SaveData saveData)
    {
        // Kinda a special case for battle commands
        if (command is not IBattleCommand battleCommand)
        {
            return null;
        }

        saveData.Gold += battleCommand.TotalGold;

        if (battleCommand.IsVictory)
        {
            foreach (var character in saveData.Party.Where(c => c.CurrentHealth > 0))
            {
                character.GetExperiencePoints(battleCommand.TotalExperiencePoints);
            }
        }
        else
        {
            foreach (var character in saveData.Party)
            {
                character.Revive();
            }
        }

        var dungeon = currentLocation as Dungeon;
        if (dungeon == null)
        {
            return null;
        }

        if (battleCommand.IsVictory)
        {
            // Wipe out the dungeon floor's inhabitants.
            dungeon.OnVictory(saveData.Inventory);
        }
        
        return new Dictionary<string, object>
        {
            { "CurrentFloor", dungeon.CurrentFloorNumber },
            { "IsClear", battleCommand.IsVictory }
        };
    }
}
