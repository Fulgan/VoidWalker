using NUnit.Framework;
using TextBlade.Core.Commands.Display;
using TextBlade.Core.IO;

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
        // Arrange/Act
        var actuals = new ShowHelpCommand().Execute(null, null);

        // Assert
        var expected = $"[{Colours.Command}]{expectedCommand}[/]".ToUpperInvariant();
        var actual = actuals.Single(a => a.ToUpperInvariant().Contains(expected));
        Assert.That(actual.ToUpperInvariant(), Does.Contain(expected));
    }
}
