using NSubstitute;
using NUnit.Framework;
using TextBlade.Core.Characters;
using TextBlade.Core.Inv;
using TextBlade.Core.IO;

namespace TextBlade.Core.Tests.Characters;


[TestFixture]
public class CharacterTests
{
    [Test]
    public void FullyHeal_FullyHealsHealthAndSkillPoints()
    {
        // Arrange
        var c = new Character("Mohammed", 200, 1, 1)
        { 
            CurrentHealth = 0,
            CurrentSkillPoints = 0,
            TotalSkillPoints = 100,
        };

        // Act
        c.FullyHeal();

        // Assert
        Assert.That(c.CurrentHealth, Is.EqualTo(c.TotalHealth));
        Assert.That(c.CurrentSkillPoints, Is.EqualTo(c.TotalSkillPoints));
    }

    [Test]
    public void Revive_SetsHealthToOne_IfHealthIsZero()
    {
        // Arrange
        var c = new Character("Moe", 200, 1, 1)
        { 
            CurrentHealth = 0,
        };

        // Act
        c.Revive();

        // Assert
        Assert.That(c.CurrentHealth, Is.EqualTo(1));
    }

    [Test]
    [TestCase(1)]
    [TestCase(10)]
    [TestCase(100)]
    [TestCase(999)]
    public void Revive_DoesNotChangeHealth_IfHealthIsPositive(int expectedHealth)
    {
        // Arrange
        var c = new Character("Moses", 2000, 1, 1)
        { 
            CurrentHealth = expectedHealth,
        };

        // Act
        c.Revive();

        // Assert
        Assert.That(c.CurrentHealth, Is.EqualTo(expectedHealth));
    }

    [Test]
    public void TotalStrength_IncludesAllEquipment()
    {
        // Arrange
        var paladin = new Character("Ahmed the Wise", 100, 5, 0);
        paladin.Equipment[ItemType.Weapon] = new ("Sword of +7", ItemType.Weapon.ToString(), new Dictionary<CharacterStats, int>
        {
            { CharacterStats.Strength, 7 },
        });
        paladin.Equipment[ItemType.Helmet] = new ("Helm of +3", ItemType.Helmet.ToString(), new Dictionary<CharacterStats, int>
        {
            { CharacterStats.Strength, 3 },
        });
        paladin.Equipment[ItemType.Armour] = new ("Spiky Plate", ItemType.Helmet.ToString(), new Dictionary<CharacterStats, int>
        {
            { CharacterStats.Strength, 1 },
        });

        // Act
        var actual = paladin.TotalStrength;

        // Assert
        Assert.That(actual, Is.EqualTo(paladin.Strength + (7 + 3 + 1) * 2));
    }

    [Test]
    public void TotalToughness_IncludesAllEquipment()
    {
        // Arrange
        var paladin = new Character("Ahmed the Wise", 100, 5, 7);
        paladin.Equipment[ItemType.Helmet] = new ("Helm of +3", ItemType.Helmet.ToString(), new Dictionary<CharacterStats, int>
        {
            { CharacterStats.Toughness, 3 },
        });
        paladin.Equipment[ItemType.Armour] = new ("Spiky Plate", ItemType.Helmet.ToString(), new Dictionary<CharacterStats, int>
        {
            { CharacterStats.Toughness, 1 },
        });

        // Act
        var actual = paladin.TotalToughness;

        // Assert
        Assert.That(actual, Is.EqualTo(paladin.Toughness + 3 + 1));
    }

    [Test]
    public void GetExperiencePoints_LevelsUpMultipleTimesIfNecessary()
    {
        // Arrange
        var noob = new Character("Noor", 10, 10, 10, 10, 10, 0);

        // Act
        noob.GainExperiencePoints(Substitute.For<IConsole>(), 9999);

        // Assert
        Assert.That(noob.Level, Is.GreaterThan(1));
        Assert.That(noob.TotalHealth, Is.GreaterThan(10));
        Assert.That(noob.Strength, Is.GreaterThan(10));
        Assert.That(noob.Toughness, Is.GreaterThan(10));
        Assert.That(noob.Special, Is.GreaterThan(10));
        Assert.That(noob.SpecialDefense, Is.GreaterThan(10));
    }

    [Test]
    public void GetExperiencePoints_GivesNoXp_IfCharacterIsDead()
    {
        // Arrange
        var deadGuy = new Character("Charlie", 10, 10, 10, 10, 10, 0);
        deadGuy.CurrentHealth = 0;

        // Act
        deadGuy.GainExperiencePoints(Substitute.For<IConsole>(), 9999);

        // Assert
        Assert.That(deadGuy.ExperiencePoints, Is.EqualTo(0));
    }

    [Test]
    public void EquippedOn_ReturnsItem_IfEquippedOnSlot_ElseReturnsNull()
    {
        // Arrange
        var paladin = new Character("Ahmed the Great", 100, 5, 7);
        var helmet = new Equipment("Helm of +3", ItemType.Helmet.ToString(), new Dictionary<CharacterStats, int>
        {
            { CharacterStats.Toughness, 3 },
        });
        paladin.Equipment[ItemType.Helmet] = helmet;

        // Act/Assert
        Assert.That(paladin.EquippedOn(ItemType.Helmet), Is.EqualTo(helmet));
        Assert.That(paladin.EquippedOn(ItemType.Weapon), Is.Null);
        Assert.That(paladin.EquippedOn(ItemType.Armour), Is.Null);
    }

    [Test]
    public void Defend_SetsIsDefendingToTrue()
    {
        // Arrange
        var c = new Character("Muhammad", 100, 10, 3);

        // Act
        c.Defend(Substitute.For<IConsole>());

        // Assert
        Assert.That(c.IsDefending, Is.True);
    }

    [Test]
    public void OnRoundComplete_SetsIsDefendingToFalse()
    {
        // Arrange
        var c = new Character("Mohammad", 100, 10, 3);
        c.Defend(Substitute.For<IConsole>());

        // Act
        c.OnRoundComplete();

        // Assert
        Assert.That(c.IsDefending, Is.False);
    }
}
