using TextBlade.Core.IO;

namespace TextBlade.Core.Characters.PartyManagement;

public class LevelManager
{
    // Clean and simple. Deterministic.
    private const int StatPointsIncreaseOnLevelUp = 3;
    private const float HealthAndSkillPointsIncreaseOnLevelUpPercent = 0.1f;
    private readonly IConsole _console;


    public LevelManager(IConsole console)
    {
        _console = console;
    }

    public void LevelUp(Character character)
    {
        while (CanLevelUp(character))
        {
            character.Level++;
            _console.WriteLine($"{character.Name} is now on level {character.Level}!");

            var healthDiff = (int)Math.Ceiling(character.TotalHealth * HealthAndSkillPointsIncreaseOnLevelUpPercent);
            character.TotalHealth += healthDiff;
            character.CurrentHealth += healthDiff;
            _console.WriteLine($"Gained {healthDiff} health!");

            var skillPointsDiff = (int)Math.Ceiling(character.TotalSkillPoints * HealthAndSkillPointsIncreaseOnLevelUpPercent);
            character.CurrentSkillPoints += skillPointsDiff;
            character.TotalSkillPoints += skillPointsDiff;
            _console.WriteLine($"Gained {skillPointsDiff} skill points!");

            character.Strength += StatPointsIncreaseOnLevelUp;
            character.Toughness += StatPointsIncreaseOnLevelUp;
            character.Special += StatPointsIncreaseOnLevelUp;
            character.SpecialDefense += StatPointsIncreaseOnLevelUp;
            _console.WriteLine($"Gained {StatPointsIncreaseOnLevelUp} strength, toughness, special, and special defense!");
        }
    }

    public bool CanLevelUp(Character character) => character.ExperiencePoints >= ExperiencePointsRequiredToLevelUp(character);

    // Simplest is best to start; a simple formula of n^2 * 100,
    // where n is the current level. Level 1? Need 100xp. Level 2? 400xp.
    // Level 100 requires 1M XP total. And each level requires as much as
    // the previous level's diff, plus some more.
    private int ExperiencePointsRequiredToLevelUp(Character character)
    {
        return (int)Math.Pow(character.Level, 2) * 100;
    }
}
