using NUnit.Framework;
using TextBlade.Core.Commands.Display;

namespace TextBlade.Core.Tests.Commands.Display;

[TestFixture]
public class ShowCreditsCommandTests
{
    [Test]
    public void ShowCreditsCommand_ReturnsTextBladeMessage()
    {
        // Arrange
        var command = new ShowCreditsCommand();
        
        // Act
        var actuals = command.Execute(null, null);
        var actual = actuals.Single();

        // Assert
        Assert.That(actual, Does.Contain("TextBlade"));
        Assert.That(actual, Does.Contain("by NightBlade"));
    }
}
