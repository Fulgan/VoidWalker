using TextBlade.Core.Characters;

namespace TextBlade.Core.Tests.TestHelpers;

public static class CharacterMaker
{
    public static Character CreateCharacter(string name = "?", int currentHealth = 100, int totalHealth = 100, int strength = 0, int toughness = 0)
    {
        return new Character()
        {
            Name = name,
            CurrentHealth = currentHealth,
            TotalHealth = totalHealth,
            Strength = strength,
            Toughness = toughness,
        };
    }
}
