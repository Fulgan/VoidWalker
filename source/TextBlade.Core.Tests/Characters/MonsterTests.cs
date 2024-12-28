using NSubstitute;
using NUnit.Framework;
using TextBlade.Core.Characters;
using TextBlade.Core.IO;

namespace TextBlade.Core.Tests.Characters;

[TestFixture]
public class MonsterTests
{
    [Test]
    public void Attack_DamagesByStrengthMinusTargetToughness()
    {
        // Arrange
        var monster = new Monster("Green Slime", 100, 10, 5, 0, 0, 0, 0);
        var target = new Character("Asad", 100, 100, 7, 0, 0, 0);
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
        var monster = new Monster("Green Slime", 100, 10, 5, 0, 0, 0, 0);
        var target = new Character("Asad", 100, 100, 3, 0, 0, 0);
        target.Defend(Substitute.For<IConsole>());

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
        var monster = new Monster("Orange Slime", 100, 5, 0, 0, 0, 0, 0);
        var target = new Character("Ahmed", 100, 100, 9999, 0, 0, 0);

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
        var monster = new Monster("Orange Slime", 1000, 1000, 0, 0, 0, 0, 0);
        var target = new Character("Ahmed", 100, 0, 0, 0, 0, 0);

        // Act
        monster.Attack(target);

        // Assert
        Assert.That(target.CurrentHealth, Is.EqualTo(0));
    }
}
