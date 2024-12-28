using TextBlade.Core.Characters;
using TextBlade.Core.Collections;
using TextBlade.Core.IO;

namespace TextBlade.Core.Battle;

/// <summary>
/// A "basic" monster AI. Attacks a random target, or uses a random skill on a random target.
/// Not very intelligent, but will suffice for many games.
/// </summary>
public class BasicMonsterAi
{
    private readonly IConsole _console;
    private readonly List<Character> _party;

    public BasicMonsterAi(IConsole console, List<Character> party)
    {
        ArgumentNullException.ThrowIfNull(console);
        
        _console = console;
        _party = party;
    }
    
    public void ProcessTurnFor(Monster monster)
    {
        var validTargets = _party.Where(p => p.CurrentHealth > 0).ToList();
        if (validTargets.Count == 0)
        {
            // Player party is wiped out, nothing to do
            return;
        }

        var target = validTargets[Random.Shared.Next(0, validTargets.Count)];
        var usableSkills = monster.SkillProbabilities.Where(kvp => Skill.GetSkill(kvp.Key).Cost <= monster.CurrentSkillPoints);

        // Should we use a skill?
        var attackProbability = 1.0 - monster.SkillProbabilities?.Sum(s => s.Value);
        var probability = Random.Shared.NextDouble();
        if (!usableSkills.Any() || probability < attackProbability)
        {
            Attack(monster, target);
            return;
        }
        
        // Use a skill, aye. ASSUMES this is not a HEALING skill.
        var skillName = new WeightedRandomBag<string>(usableSkills.ToDictionary()).GetRandom();
        var skill = monster.Skills.Single(s => s.Name == skillName);
        var targets = skill.Target == "AllEnemies" ? _party : [_party.First(p => p.CurrentHealth > 0)];
        new SkillApplier(_console).Apply(monster, skill, targets);
    }   

    private void Attack(Monster monster, Character target)
    {
        // Nah, nah, just attack.
        var damage = monster.Attack(target);
        var message = $"{monster.Name} attacks {target.Name} for [{Colours.Highlight}]{damage}[/] damage! ";
        if (target.CurrentHealth <= 0)
        {
            message += $"{target.Name} [{Colours.Highlight}]DIES![/] Oh no!";
        }

        _console.WriteLine(message);
    } 
}
