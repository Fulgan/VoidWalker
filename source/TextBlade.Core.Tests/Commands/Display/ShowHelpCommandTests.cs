using NUnit.Framework;
using TextBlade.Core.Commands.Display;
using TextBlade.Core.IO;
using TextBlade.Core.Tests.Stubs;

namespace TextBlade.Core.Tests.Commands.Display;

[TestFixture]
public class ShowHelpCommandTests
{
    [Test]
    [TestCase("credits")]
    [TestCase("help")]
    [TestCase("inv")]
    [TestCase("look")]
    [TestCase("party/status")]
    [TestCase("save")]
    [TestCase("quit")]
    public void Execute_ReturnsABunchOfCommands(string expectedCommand)
    {
        // Arrange
        var console = new ConsoleStub();
        
        // Act
        new ShowHelpCommand(console).Execute(null, null);

        // Assert
        var expected = $"[{Colours.Command}]{expectedCommand}[/]".ToUpperInvariant();
        var actual = string.Join('\n', console.Messages);
        Assert.That(actual.ToUpperInvariant(), Does.Contain(expected));
    }
}
