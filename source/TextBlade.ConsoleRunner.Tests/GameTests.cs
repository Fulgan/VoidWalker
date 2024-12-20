using Newtonsoft.Json;
using NSubstitute;
using NUnit.Framework;
using TextBlade.ConsoleRunner.Tests.Stubs;
using TextBlade.Core.Inv;
using TextBlade.Core.IO;
using TextBlade.Core.Locations;
using TextBlade.Core.Services;

namespace TextBlade.ConsoleRunner.Tests;

[TestFixture]
public class GameTests
{
    [SetUp]
    [TearDown]
    public void SetSaveGameFilename()
    {
        SaveGameManager.CurrentGameSlot = "unittests";
    }
    
    [Test]
    public void SaveGame_SavesCurrentLocationData_IfItExists()
    {
        // Arrange
        var game = CreateGameStub();
        var dungeon = new Dungeon("Dinky Dungeon", "It's ... dinky ...", 3, ["Black Mold", "White Mold"], "Mulder") { LocationId = "DinkyDungeon.json"};
        game.CurrentLocation = dungeon;

        // Act
        game.SaveGame();

        // Assert
        var saveFileName = Path.Join(SaveGameManager.SaveFolder, $"{SaveGameManager.CurrentGameSlot}{SaveGameManager.SaveFileExtension}");
        var actual = JsonConvert.DeserializeObject<SaveData>(File.ReadAllText(saveFileName));
        Assert.That(actual.LocationSpecificData, Is.Not.Null);
        Assert.That(actual.LocationSpecificData.Keys, Does.Contain("CurrentFloor"));
        Assert.That(actual.LocationSpecificDataLocationId, Is.EqualTo(dungeon.LocationId));
    }

    [Test]
    public void SaveGame_UsesExistingSaveData_IfCurrentLocationHasNone()
    {
        // Arrange
        // Previous save location was a dungeon
        var game = CreateGameStub();
        var dungeon = new Dungeon("Dinky Dungeon", "It's ... dinky ...", 3, ["Black Mold", "White Mold"], "Mulder") { LocationId = "DinkyDungeon.json"};
        game.CurrentLocation = dungeon;
        game.SaveGame();

        // Current location is an inn
        var inn = new Inn("Snoozeville", "It's made of clouds", "SnoozeVilleInn") { LocationId = "snoozeVille.json" };
        game.CurrentLocation = inn;

        // Act
        game.SaveGame();

        // Assert
        // Dungeon data should still be there
        var saveFileName = Path.Join(SaveGameManager.SaveFolder, $"{SaveGameManager.CurrentGameSlot}{SaveGameManager.SaveFileExtension}");
        var actual = JsonConvert.DeserializeObject<SaveData>(File.ReadAllText(saveFileName));
        Assert.That(actual.LocationSpecificData, Is.Not.Null);
        Assert.That(actual.LocationSpecificData.Keys, Does.Contain("CurrentFloor"));
        Assert.That(actual.LocationSpecificDataLocationId, Is.EqualTo(dungeon.LocationId));
    }

    public GameStub CreateGameStub()
    {
        return new GameStub(Substitute.For<IConsole>(), Substitute.For<ISoundPlayer>());
    }
}
