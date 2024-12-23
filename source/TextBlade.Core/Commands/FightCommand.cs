using TextBlade.Core.IO;
using TextBlade.Core.Locations;

namespace TextBlade.Core.Commands;

public class FightCommand : ICommand
{
    private readonly IConsole _console;
    private readonly Dungeon? _dungeon;

    private readonly List<string> _monsterNames;
    private readonly List<string> _loot;

    public FightCommand(IConsole console, Location currentLocation, List<string> monsterNames, List<string> loot)
    {
        ArgumentNullException.ThrowIfNull(console);
        ArgumentNullException.ThrowIfNull(currentLocation);
        currentLocation.ThrowIfNot<Dungeon>();
        ArgumentNullException.ThrowIfNull(monsterNames);
        if (!monsterNames.Any())
        {
            throw new ArgumentException("Monster list is empty.");  
        }
        ArgumentNullException.ThrowIfNull(loot);

        _console = console;
        _dungeon = currentLocation as Dungeon;
        _monsterNames = monsterNames;
        _loot = loot;
    }
    
    public bool Execute(SaveData saveData)
    {
        // Signal to game to begin the fight. Fight! Fight! Fight!
        var system = new TurnBasedBattleSystem(_console, saveData, _dungeon, _monsterNames, _loot);
        var spoils = system.Execute();

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
