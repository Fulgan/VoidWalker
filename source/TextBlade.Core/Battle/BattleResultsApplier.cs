using TextBlade.Core.Commands;
using TextBlade.Core.IO;
using TextBlade.Core.Locations;

namespace TextBlade.Core.Battle;

public static class BattleResultsApplier
{
    public static void ApplyResultsIfBattle(ICommand command, Location currentLocation, SaveData saveData)
    {
        // Kinda a special case for battle commands. And manual save.
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
                foreach (var message in character.GetExperiencePoints(battleCommand.TotalExperiencePoints))
                {
                    // TODO: return higher-up so we can apply styling/colour to it...
                    Console.WriteLine(message);
                }
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
            return;
        }

        if (battleCommand.IsVictory)
        {
            // Wipe out the dungeon floor's inhabitants.
            dungeon.OnVictory(saveData.Inventory);
        }
    }
}
