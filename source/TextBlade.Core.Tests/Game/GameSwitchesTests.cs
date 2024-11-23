using NUnit.Framework;
using TextBlade.Core.Game;

namespace TextBlade.Core.Tests.Game;

[TestFixture]
public class GameSwitchesTests
{
    [Test]
    public void Has_ReturnsTrue_IfSwitchExists()
    {
        // Arrange
        var switches = new GameSwitches();
        switches.Set("Met da SULTAAN", true);
        switches.Set("kicked by a goat", false);

        // Act/Assert
        Assert.That(switches.Has("Met da SULTAAN"), Is.True);
        Assert.That(switches.Has("kicked by a goat"), Is.True);
        Assert.That(switches.Has("MeT dA SuLtAaN"), Is.False);
        Assert.That(switches.Has("ate some carrots"), Is.False);
    }

    [Test]
    public void Set_SetsValue()
    {
        // Arrange
        var switches = new GameSwitches();

        // Act
        switches.Set("one", true);
        switches.Set("two", false);

        // Arrange
        var actual = switches.Data;
        Assert.That(actual["one"], Is.True);
        Assert.That(actual["two"], Is.False);
    }

    [Test]
    [TestCase("got the earth crystal")]
    [TestCase("got the fire crystal")]
    [TestCase("got backstabbed")]
    public void Get_Throws_IfKeyDoesntExist(string switchName)
    {
        // Arrange
        var switches = new GameSwitches();

        // Act/Assert
        Assert.Throws<ArgumentException>(() => switches.Get(switchName));
    }

    [Test]
    public void Get_ReturnsValue()
    {
        // Arrange
        var switches = new GameSwitches();
        switches.Set("Town 1 complete", true);
        switches.Set("Boss 1 complete", false);

        // Act/Assert
        Assert.That(switches.Get("Town 1 complete"), Is.True);
        Assert.That(switches.Get("Boss 1 complete"), Is.False);
    }

    [Test]
    public void Switches_ReturnsSameInstance()
    {
        // Arrange
        var one = new GameSwitches();

        // Act
        var two = GameSwitches.Switches;

        // Assert
        Assert.That(two, Is.EqualTo(one));
    }
}
