namespace TextBlade.Core.Characters.PartyManagement;

public static class LevelManager
{
    // Clean and simple. Deterministic.
    private const int StatPointsIncreaseOnLevelUp = 3;
    private const float HealthAndSkillPointsIncreaseOnLevelUpPercent = 0.1f;

    public static void LevelUp(Character character)
    {
        while (CanLevelUp(character))
        {
            character.Level++;

            var healthDiff = (int)Math.Ceiling(character.TotalHealth * HealthAndSkillPointsIncreaseOnLevelUpPercent);
            character.TotalHealth += healthDiff;
            character.CurrentHealth += healthDiff;

            var skillPointsDiff = (int)Math.Ceiling(character.TotalSkillPoints * HealthAndSkillPointsIncreaseOnLevelUpPercent);
            character.CurrentSkillPoints += skillPointsDiff;
            character.TotalSkillPoints += skillPointsDiff;

            character.Strength += StatPointsIncreaseOnLevelUp;
            character.Toughness += StatPointsIncreaseOnLevelUp;
            character.Special += StatPointsIncreaseOnLevelUp;
            character.SpecialDefense += StatPointsIncreaseOnLevelUp;
        }
    }

    public static bool CanLevelUp(Character character) => character.ExperiencePoints >= ExperiencePointsRequiredToLevelUp(character);

    // Simplest is best to start; a simple formula of n^2 * 100,
    // where n is the current level. Level 1? Need 100xp. Level 2? 400xp.
    // Level 100 requires 1M XP total. And each level requires as much as
    // the previous level's diff, plus some more.
    private static int ExperiencePointsRequiredToLevelUp(Character character)
    {
        return (int)Math.Pow(character.Level, 2) * 100;
    }
}
