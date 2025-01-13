using NSubstitute;
using NUnit.Framework;
using TextBlade.Core.Commands;
using TextBlade.Core.IO;
using TextBlade.Core.Tests.Stubs;

namespace TextBlade.Core.Tests.Commands;

[TestFixture]
public class ChangeLocationCommandTests
{
    [Test]
    public void Execute_SetsLocationAndLocationIdOnGame()
    {
        // Arrange
        var game = new GameStub();
        var expectedLocationId = Path.Join("Data", "Locations", "FrozenSea");
        var command = new ChangeLocationCommand(game, expectedLocationId);

        // Act
        var actual = command.Execute(Substitute.For<IConsole>(), null, new SaveData());

        // Assert
        Assert.That(game.Location.LocationId, Is.EqualTo(expectedLocationId));
        Assert.That(game.Location.Name, Is.EqualTo("The Frozen Sea"));
    }
}
