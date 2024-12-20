using Newtonsoft.Json;
using NSubstitute;
using NUnit.Framework;
using TextBlade.ConsoleRunner.Tests.Stubs;
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
        var dungeon = new Dungeon("Dinky Dungeon", "It's ... dinky ...", 3, ["Black Mold", "White Mold"], "Mulder");
        game.CurrentLocation = dungeon;

        // Act
        game.SaveGame();

        // Assert
        var saveFileName = Path.Join(SaveGameManager.SaveFolder, $"{SaveGameManager.CurrentGameSlot}{SaveGameManager.SaveFileExtension}");
        var actual = JsonConvert.DeserializeObject<SaveData>(File.ReadAllText(saveFileName));
        Assert.That(actual.LocationSpecificData, Is.Not.Null);
        Assert.That(actual.LocationSpecificData.Keys, Does.Contain("CurrentFloor"));
    }

    public GameStub CreateGameStub()
    {
        return new GameStub(Substitute.For<IConsole>(), Substitute.For<ISoundPlayer>());
    }
}
