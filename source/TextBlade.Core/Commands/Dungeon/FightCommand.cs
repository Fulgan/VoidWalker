using TextBlade.Core.Battle;
using TextBlade.Core.IO;

namespace TextBlade.Core.Commands;

public class FightCommand : ICommand
{
    public IBattleSystem System { get; set; }

    public bool Execute(IConsole console, SaveData saveData)
    {
        ArgumentNullException.ThrowIfNull(console);
        ArgumentNullException.ThrowIfNull(System);

        // Signal to game to begin the fight. Fight! Fight! Fight!
        var spoils = System.Execute(saveData);

        // Process results: victory, or defeat?
        if (spoils.IsVictory)
        {
            saveData.Gold += spoils.GoldGained;

            foreach (var character in saveData.Party)
            {
                if (character.IsAlive)
                {
                    character.GainExperiencePoints(console, spoils.ExperiencePointsGained);
                }
            }

            if (spoils.Loot.Any())
            {
                Console.WriteLine("Your party spies a treasure chest. You hurry over and open it. Within it, you find: ");
                foreach (var itemName in spoils.Loot)
                {
                    Console.WriteLine($"    {itemName}");
                    
                    var item = ItemsData.GetItem(itemName);
                    saveData.Inventory.Add(item);
                }
            }
        }
        
        foreach (var character in saveData.Party)
        {
            character.Revive();
        }

        return true;
    }
}
