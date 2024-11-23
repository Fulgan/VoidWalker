using NUnit.Framework;
using TextBlade.Core.Commands;

namespace TextBlade.Core.Tests.Commands;

[TestFixture]
public class ShowCreditsCommandTests
{
    [Test]
    public void ShowCreditsCommand_ReturnsTextBladeMessage()
    {
        // Arrange
        var command = new ShowCreditsCommand();
        
        // Act
        var actual = command.Execute(null, null).Single();

        // Assert
        Assert.That(actual, Does.Contain("TextBlade"));
        Assert.That(actual, Does.Contain("by NightBlade"));
    }
}
