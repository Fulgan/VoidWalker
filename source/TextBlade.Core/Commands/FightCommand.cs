using TextBlade.Core.Battle;
using TextBlade.Core.IO;

namespace TextBlade.Core.Commands;

public class FightCommand : ICommand
{
    private readonly IConsole _console;
    private readonly IBattleSystem _system;

    public FightCommand(IConsole console, IBattleSystem system)
    {
        ArgumentNullException.ThrowIfNull(console);
        ArgumentNullException.ThrowIfNull(system);

        _console = console;
        _system = system;
    }
    
    public bool Execute(SaveData saveData)
    {
        // Signal to game to begin the fight. Fight! Fight! Fight!
        var spoils = _system.Execute(saveData);

        // Process results: victory, or defeat?
        if (spoils.IsVictory)
        {
            saveData.Gold += spoils.GoldGained;

            foreach (var character in saveData.Party)
            {
                if (character.IsAlive)
                {
                    character.GainExperiencePoints(_console, spoils.ExperiencePointsGained);
                }
            }

            if (spoils.Loot.Any())
            {
                _console.WriteLine("Your party spies a treasure chest. You hurry over and open it. Within it, you find: ");
                foreach (var itemName in spoils.Loot)
                {
                    _console.WriteLine($"    {itemName}");
                    
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
