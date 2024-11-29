using NUnit.Framework;
using TextBlade.Core.Characters.PartyManagement;
using TextBlade.Core.Inv;

namespace TextBlade.Core.Tests.Characters.PartyManagement;

[TestFixture]
public class EquipmentDifferTests
{
    [Test]
    public void GetDiff_ReturnsNewThingStats_IfCurrentThingIsNull()
    {
        // Arrange
        var proposed = new Equipment("Black Sword", ItemType.Weapon.ToString(), new()
        {
            { CharacterStats.Strength, 15 },
            { CharacterStats.Special, 7 },
        });

        // Act
        var actual = EquipmentDiffer.GetDiff(null, proposed);

        // Assert
        Assert.That(actual, Is.EqualTo(proposed.StatsModifiers));
    }

        [Test]
    public void GetDiff_ReturnsNegationOfCurrentThingStats_IfNewThingThingIsNull()
    {
        // Unequipping something completely? It'll set everything to negative/zero.
        // Arrange
        var current = new Equipment("White Sword", ItemType.Weapon.ToString(), new()
        {
            { CharacterStats.Strength, 15 },
            { CharacterStats.Special, 7 },
        });

        // Act
        var actual = EquipmentDiffer.GetDiff(current, null);

        // Assert
        foreach (var stat in current.StatsModifiers.Keys)
        {
            var expectedValue = -current.StatsModifiers[stat];
            var actualValue = actual[stat];
            Assert.That(actualValue, Is.EqualTo(expectedValue));
        }
    }

    [Test]
    public void GetDiff_HandlesExclusiveOrCommonStats()
    {
        // Arrange
        var current = new Equipment("Blue Sword", ItemType.Weapon.ToString(), new()
        {
            { CharacterStats.Strength, 10 },
            { CharacterStats.Toughness, 5 },
        });

        var proposed = new Equipment("Red Sword", ItemType.Weapon.ToString(), new()
        {
            { CharacterStats.Strength, 15 },
            { CharacterStats.Special, 7 },
        });

        // Act
        var actual = EquipmentDiffer.GetDiff(current, proposed);

        // Assert
        Assert.That(actual, Does.ContainKey(CharacterStats.Strength));
        Assert.That(actual, Does.ContainKey(CharacterStats.Toughness));
        Assert.That(actual, Does.ContainKey(CharacterStats.Special));

        // Additive
        Assert.That(actual[CharacterStats.Special], Is.EqualTo(7));
        // Common; calculate diff
        Assert.That(actual[CharacterStats.Strength], Is.EqualTo(5));
        // Gone, so negated
        Assert.That(actual[CharacterStats.Toughness], Is.EqualTo(-5));
    }
}
