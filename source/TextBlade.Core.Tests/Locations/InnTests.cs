using NSubstitute;
using NUnit.Framework;
using TextBlade.Core.Commands;
using TextBlade.Core.IO;
using TextBlade.Core.Locations;

namespace TextBlade.Core.Tests.Locations;

[TestFixture]
public class InnTests
{
    [Test]
    [TestCase("sS")]
    [TestCase("Ss")]
    [TestCase("ss")]
    [TestCase("SS")]
    [TestCase("sleep")]
    [TestCase("meditate")]
    [TestCase("you tell me")]
    [TestCase("q")]
    [TestCase("1")]
    [TestCase("!")]
    public void GetCommandFor_ReturnsDoNothingCommand(string input)
    {
        // Arrange
        var inn = new Inn("The Inn of Fluffy Pillows", "An odd pillow-shaped white building", string.Empty);
        
        // Act
        var actual = inn.GetCommandFor(input);

        // Assert
        Assert.That(actual, Is.InstanceOf<DoNothingCommand>());
    }

    [Test]
    [TestCase("s")]
    [TestCase("S")]
    public void GetCommandFor_ReturnsInnCommand_IfInputIsS(string s)
    {
        // Arrange
        var inn = new Inn("The Inn of Fluffy Minnows", "An odd fish-shaped white building", string.Empty);
        
        // Act
        var actual = inn.GetCommandFor(s);

        // Assert
        Assert.That(actual, Is.InstanceOf<SleepAtInnCommand>());
    }
}
