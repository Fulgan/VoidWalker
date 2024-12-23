using TextBlade.Core.Battle;
using TextBlade.Core.Game;
using TextBlade.Core.IO;

namespace TextBlade.Core.Commands;

public class FightCommand : ICommand
{
    private readonly IConsole _console;
    private readonly List<string> _monsterNames;
    private readonly List<string> _loot;

    public FightCommand(IConsole console, List<string> monsterNames, List<string> loot)
    {
        _console = console;
        _monsterNames = monsterNames;
        _loot = loot;
    }
    
    public bool Execute(SaveData saveData)
    {
        // Signal to game to begin the fight. Fight! Fight! Fight!
        var system = new TurnBasedBattleSystem(_console, saveData, _monsterNames, _loot);
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
