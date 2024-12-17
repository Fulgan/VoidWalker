using NSubstitute;
using NUnit.Framework;
using TextBlade.Core.Commands;
using TextBlade.Core.IO;
using TextBlade.Core.Locations;

namespace TextBlade.Core.Tests.Locations;

[TestFixture]
public class LocationTests
{
    [Test]
    public void GetCommandFor_ReturnDoNothingCommand()
    {
        // Arrange
        var location = new Location("A", "B");

        // Act
        var actual = location.GetCommandFor(Substitute.For<IConsole>(), "q");

        // Assert
        Assert.That(actual, Is.InstanceOf<DoNothingCommand>());
    }

    [Test]
    public void GetExtraDescription_ReturnsEmptyString()
    {
        // Arrange
        var location = new Location("A", "B");

        // Act
        var actual = location.GetExtraDescription();

        // Assert
        Assert.That(actual, Is.Empty);
    }

    [Test]
    public void GetExtraMenuOption_ReturnsEmptyString()
    {
        // Arrange
        var location = new Location("A", "B");

        // Act
        var actual = location.GetExtraMenuOptions();

        // Assert
        Assert.That(actual, Is.Empty);
    }
}
