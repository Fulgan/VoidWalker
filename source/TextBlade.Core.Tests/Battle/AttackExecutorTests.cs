using System;
using NSubstitute;
using NUnit.Framework;
using TextBlade.Core.Battle;
using TextBlade.Core.Characters;
using TextBlade.Core.Inv;
using TextBlade.Core.IO;
using TextBlade.Core.Tests.Stubs;

namespace TextBlade.Core.Tests.Battle;

[TestFixture]
public class AttackExecutorTests
{
    [Test]
    public void Attack_ThrowsIfCharacterIsNull()
    {
        // Arrange
        var executor = new AttackExecutor(Substitute.For<IConsole>());
        var monster = new Monster("Bunny", 10, 5, 3, 5, 5, 0, 0);

        // Act
        var ex = Assert.Throws<ArgumentNullException>(() => executor.Attack(null, monster));
        
        // Assert
        Assert.That(ex.Message, Does.Contain("character"));
    }

    [Test]
    public void Attack_ThrowsIfMonsterIsNull()
    {
        // Arrange
        var executor = new AttackExecutor(Substitute.For<IConsole>());
        var character = new Character("Bonnie", 10, 5, 3, 0, 0, 0);

        // Act
        var ex = Assert.Throws<ArgumentNullException>(() => executor.Attack(character, null));
        
        // Assert
        Assert.That(ex.Message, Does.Contain("targetMonster"));
    }

    [Test]
    public void Attack_UsesWeaponStrengthForDamage()
    {
        // Arrange
        var attacker = new Character("Faris", 100, 10, 1, 0, 0, 0);
        var defender = new Monster("Slime", 100, 0, 3, 0, 0, 0);
        
        var knifeStats = new Dictionary<CharacterStats, int>
        {
            { CharacterStats.Strength, 5 },
        };
        var helmetStats = new Dictionary<CharacterStats, int>
        {
            { CharacterStats.Strength, 1 },
            { CharacterStats.Toughness, 3 },
        };

        attacker.Equipment[ItemType.Weapon] = new Equipment("Dirk", ItemType.Weapon.ToString(), knifeStats);
        attacker.Equipment[ItemType.Armour] = new Equipment("Spiky Helmet", ItemType.Helmet.ToString(), helmetStats);
        
        var executor = new AttackExecutor(Substitute.For<IConsole>());

        // Act
        executor.Attack(attacker, defender);

        // Assert
        var expectedDamage = attacker.TotalStrength - defender.Toughness;
        var actualDamage = defender.TotalHealth - defender.CurrentHealth;

        Assert.That(actualDamage, Is.EqualTo(expectedDamage));
    }

    [Test]
    public void Attack_DoesDoubleDamageAndMentionsIt_IfCharacterWeaponIsMonsterWeakness()
    {
        // Arrange
        var attacker = new Character("Faraz", 100, 10, 1, 0, 0, 0, 0);
        var defender = new Monster("Slime", 100, 0, 3, 0, 0, 0, weakness: "Lightning");
        
        var knifeStats = new Dictionary<CharacterStats, int>
        {
            { CharacterStats.Strength, 5 },
        };

        attacker.Equipment[ItemType.Weapon] = new Equipment("Dirk", ItemType.Weapon.ToString(), knifeStats, "Lightning");
        var console = new ConsoleStub();
        var executor = new AttackExecutor(console);

        // Act
        executor.Attack(attacker, defender);

        // Assert
        var expectedDamage = 2 * (attacker.TotalStrength - defender.Toughness);
        var actualDamage = defender.TotalHealth - defender.CurrentHealth;

        Assert.That(actualDamage, Is.EqualTo(expectedDamage));
        Assert.That(console.Messages.Any(m => m.ToUpperInvariant().Contains("Super effective!".ToUpperInvariant())));
    }

    [Test]
    public void Attack_MentionsIfMonsterDies()
    {
        // Arrange
        var attacker = new Character("Farhan", 100, 999, 1, 0, 0, 0);
        var defender = new Monster("Slime", 10, 5, 3 , 0, 0, 0);

        var console = new ConsoleStub();
        var executor = new AttackExecutor(console);

        // Act
        executor.Attack(attacker, defender);
        Assert.That(console.Messages.Any(m => m.ToUpperInvariant().Contains("Slime DIES!".ToUpperInvariant())));
    }
}
