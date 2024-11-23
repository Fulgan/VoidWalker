using NUnit.Framework;
using TextBlade.Core.Characters;

namespace TextBlade.Core.Tests.Characters;

[TestFixture]
public class MonsterTests
{
    [Test]
    public void Constructor_SetsHealthToTotalHealth()
    {
        // Arrange/Act
        var monster = new Monster("Slime", 10, 5, 3);
        // Assert
        Assert.That(monster.CurrentHealth, Is.EqualTo(10));
    }

    [Test]
    public void Damage_ReducesHp()
    {
        // Arrange
        var monster = new Monster("Blue Slime", 100, 10, 1);

        // Act
        monster.Damage(55);

        // Assert
        Assert.That(monster.CurrentHealth, Is.EqualTo(45));
    }

    [Test]
    public void Damage_BottomsOutHealthAtZero()
    {
        // Arrange
        var monster = new Monster("Yellow Slime", 25, 5, 1);
    
        // Act
        monster.Damage(99999);

        // Assert
        Assert.That(monster.CurrentHealth, Is.EqualTo(0));
    }

    [Test]
    public void Attack_DamagesByStrengthMinusTargetToughness()
    {
        // Arrange
        var monster = new Monster("Green Slime", 100, 10, 5);
        var target = new Character("Asad", 100, 100) { Toughness = 7 };
        var expectedDamage = monster.Strength - target.Toughness;

        // Act
        monster.Attack(target);

        // Assert
        Assert.That(target.CurrentHealth, Is.EqualTo(target.TotalHealth - expectedDamage));
    }
    
    [Test]
    public void Attack_DamagesByLessThanStrengthMinusTargetToughness_IfTargetIsBlocking()
    {
        // Arrange
        var monster = new Monster("Green Slime", 100, 10, 5);
        var target = new Character("Asad", 100, 100) { Toughness = 3 };
        target.Defend();

        var expectedDamage = monster.Strength - target.Toughness;

        // Act
        monster.Attack(target);

        // Assert. Health should be more than what we expected damage-wise. But not 100%.
        Assert.That(target.CurrentHealth, Is.Not.EqualTo(target.TotalHealth));
        Assert.That(target.CurrentHealth, Is.GreaterThan(target.TotalHealth - expectedDamage));
    }

    [Test]
    [TestCase(10)] // equals toughness
    [TestCase(171)]
    [TestCase(9999)] // overblock
    public void Attack_DoesNoDamage_IfDamageIsZeroOrNegative(int strength)
    {
        // Arrange
        var monster = new Monster("Orange Slime", 100, strength, 5);
        var target = new Character("Ahmed", 100, 100) { Toughness = 9999 };

        // Act
        monster.Attack(target);

        // Assert
        Assert.That(target.CurrentHealth, Is.EqualTo(target.TotalHealth));
    }

    [Test]
    [TestCase(101)]
    [TestCase(999)]
    [TestCase(20717)]
    public void Attack_SetsTargetHealthToZero_IfTargetDies(int strength)
    {
        // Arrange
        var monster = new Monster("Orange Slime", 1000, strength, 5);
        var victim = new Character("Ahmed", 100, 100);

        // Act
        monster.Attack(victim);

        // Assert
        Assert.That(victim.CurrentHealth, Is.EqualTo(0));
    }
}
