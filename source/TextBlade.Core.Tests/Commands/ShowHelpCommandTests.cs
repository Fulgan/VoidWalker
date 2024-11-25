using NUnit.Framework;
using TextBlade.Core.Commands;
using TextBlade.Core.Tests.TestHelpers;

namespace TextBlade.Core.Tests.Commands;

[TestFixture]
public class ShowHelpCommandTests
{
    [Test]
    public async Task Execute_ReturnsABunchOfCommands()
    {
        // Arrange/Act
        var actuals = await AsyncToList.ToList(new ShowHelpCommand().Execute(null, null));

        // Assert
        // Check a bunch of commands
        foreach (var expectedCommand in new string[] {"credits", "help", "quit"})
        {
            var expected = $"[red]{expectedCommand}[/]".ToUpperInvariant();
            var actual = actuals.Single(a => a.ToUpperInvariant().Contains(expected));
            Assert.That(actual.ToUpperInvariant(), Does.Contain(expected));
        }
    }
}
