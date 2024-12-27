using System;
using NSubstitute;
using NUnit.Framework;
using TextBlade.Core.Battle;
using TextBlade.Core.Characters;
using TextBlade.Core.Commands;
using TextBlade.Core.IO;
using TextBlade.Core.Locations;

namespace TextBlade.Core.Tests.Commands;

[TestFixture]
public class FightCommandTests
{
    [Test]
    public void Constructor_Throws_IfAnyArgumentsAreNull()
    {
        // Act/Assert
        Assert.Throws<ArgumentException>(() => new FightCommand(null, Substitute.For<TurnBasedBattleSystem>()));
        Assert.Throws<ArgumentNullException>(() => new FightCommand(Substitute.For<IConsole>(), null));
    }

    [Test]
    public void Execute_GivesNoSpoilsOrXp_IfDefeat()
    {
        // Arrange
        var system = Substitute.For<IBattleSystem>();
        var command = new FightCommand(Substitute.For<IConsole>(), system);
        var saveData = CreateSaveData();

        system.Execute(saveData).Returns(new Spoils
        {
            IsVictory = false,
            ExperiencePointsGained = 9999,
            GoldGained = 999999,
            Loot = new List<string> { "Apple", "Banana", "Cobbler", "Dog", "Egg", "Fish" },
        });

        // Act
        command.Execute(saveData);

        // Assert. No gold, no loot in inventory, no XP.
        Assert.That(saveData.Gold, Is.EqualTo(0));
        Assert.That(saveData.Inventory.ItemQuantities.Count, Is.EqualTo(0));
        Assert.That(saveData.Party.TrueForAll(p => p.ExperiencePoints ==  0));
    }

    [Test]
    public void Execute_GivesSpoilsAndXp_IfVictory()
    {
        // Arrange
        var system = Substitute.For<IBattleSystem>();
        var command = new FightCommand(Substitute.For<IConsole>(), system);
        var saveData = CreateSaveData();

        system.Execute(saveData).Returns(new Spoils
        {
            IsVictory = true,
            ExperiencePointsGained = 9999,
            GoldGained = 999999,
            Loot = new List<string> { "Potion-A" },
        });

        // Act
        command.Execute(saveData);

        Assert.That(saveData.Gold, Is.GreaterThan(0));
        Assert.That(saveData.Inventory.ItemQuantities.Count, Is.GreaterThan(0));
        Assert.That(saveData.Party.TrueForAll(p => p.ExperiencePoints > 0));
        Assert.That(saveData.Party.TrueForAll(p => p.Level > 1));
    }


    private SaveData CreateSaveData()
    {
        return new SaveData()
        {
            Inventory = new(),
            Party = new List<Character>
            {
                new Character("Ahmed", 10, 10, 10, 0, 0, 0),
                new Character("Bilal", 10, 100, 1000, 0, 0, 0),
            },
        };
    }
}
