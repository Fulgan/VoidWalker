using NSubstitute;
using NUnit.Framework;
using TextBlade.Core.Battle;
using TextBlade.Core.Commands;
using TextBlade.Core.IO;
using TextBlade.Core.Locations;

namespace TextBlade.Core.Tests.Battle;

[TestFixture]
public class BattleResultsApplierTests
{
    [Test]
    public void ApplyResultsIfBattle_DoesNothing_IfCommandIsManualSaveAndLocationIsDungeon()
    {
        // Arrange
        var command = new ManuallySaveCommand();
        var location = new Dungeon("Rabbit's Warren", "Cute and deadly", 1, new List<string>() { "Rabbit" }, "X-Hare");
        var data = new SaveData();

        // Act
        new BattleResultsApplier(Substitute.For<IConsole>()).ApplyResultsIfBattle(command, location, data);

        // Assert: no gold = nothing happened
        Assert.That(data.Gold, Is.EqualTo(0));
    }

    [Test]
    public void ApplyResultsIfBattle_DoesNothing_IfCommandIsNotBattleCommand()
    {
        // Arrange
        var command = new DoNothingCommand();
        var location = new Dungeon("Rabbit's Warren", "Cute and deadly", 1, new List<string>() { "Rabbit" }, "X-Hare");
        var data = CreateSaveData();

        // Act
        new BattleResultsApplier(Substitute.For<IConsole>()).ApplyResultsIfBattle(command, location, data);

        // Assert: no gold = nothing happened
        Assert.That(data.Gold, Is.EqualTo(0));
    }

    [Test]
    public void ApplyResultsIfBattle_GivesGold()
    {
        // Arrange
        var command = Substitute.For<IBattleCommand>();
        command.TotalGold.Returns(100);
        var location = new Dungeon("Rabbit's Warren", "Cute and deadly", 1, new List<string>() { "Rabbit" }, "X-Hare");
        var data = CreateSaveData(150);

        // Act
        new BattleResultsApplier(Substitute.For<IConsole>()).ApplyResultsIfBattle(command, location, data);

        // Assert
        Assert.That(data.Gold, Is.EqualTo(250));
    }

    [Test]
    public void ApplyResultsIfBattle_GivesExperiencePointsToAllLivingPlayers_IfVictory()
    {
        // Arrange
        var command = Substitute.For<IBattleCommand>();
        command.IsVictory.Returns(true);
        command.TotalExperiencePoints.Returns(9999);

        var location = new Location("Mountain Pass", "Omnious");
        var data = CreateSaveData();
        data.Party.Add(new Core.Characters.Character("Player Two", 100, 100, 100) { CurrentHealth = 0});

        // Act
        new BattleResultsApplier(Substitute.For<IConsole>()).ApplyResultsIfBattle(command, location, data);

        // Assert
        var p1 = data.Party[0];
        // Alive: gained XP
        Assert.That(p1.ExperiencePoints, Is.EqualTo(command.TotalExperiencePoints));
        var p2 = data.Party[1];
        // Dead: no XP
        Assert.That(p2.ExperiencePoints, Is.EqualTo(0));
    }

    [Test]
    public void ApplyResultsIfBattle_RevivesAllCharacters_IfDefeat()
    {
        // Arrange
        var command = Substitute.For<IBattleCommand>();
        command.IsVictory.Returns(false);

        var location = new Location("Steppe Pass", "Lovely");
        var data = CreateSaveData();
        data.Party.Add(new Core.Characters.Character("Player Two", 100, 100, 100));

        foreach (var p in data.Party)
        {
            p.CurrentHealth = 0;
        }

        // Act
        new BattleResultsApplier(Substitute.For<IConsole>()).ApplyResultsIfBattle(command, location, data);

        // Assert
        Assert.That(data.Party.All(p => p.CurrentHealth == 1));
    }

    [Test]
    public void ApplyResultsIfBattle_GivesLoot_IfVictoryInDungeonAndFloorHasLoot()
    {
        // Arrange
        var command = Substitute.For<IBattleCommand>();
        command.IsVictory.Returns(true);

        var location = new Dungeon("Owl's Nest", "Cute and deadly", 1, new List<string>() { "Owl" }, "Owlicious");
        location.FloorLoot["B1"] = new List<string>
        {
            "Iron Sword",
            "Potion-A",
            "Potion-A",
        };

        var data = CreateSaveData();
        data.Inventory.Add(ItemsData.GetItem("Potion-A"));

        // Act
        new BattleResultsApplier(Substitute.For<IConsole>()).ApplyResultsIfBattle(command, location, data);

        // Assert
        Assert.That(data.Inventory.ItemsInOrder.Any(a => a.Name == "Iron Sword"));
        Assert.That(data.Inventory.ItemsInOrder.Any(a => a.Name == "Potion-A"));
        Assert.That(data.Inventory.ItemQuantities["Potion-A"], Is.EqualTo(3)); // one existing, two new
    }

    private SaveData CreateSaveData(int gold = 0)
    {
        return new SaveData
        {
            Party = new List<Core.Characters.Character>
            {
                new ("Player One", 100, 10, 5, 3),
            },
            Gold = gold,
            Inventory = new(),
        };
    }
}
