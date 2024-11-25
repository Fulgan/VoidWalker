using System.Runtime.CompilerServices;
using NUnit.Framework;
using TextBlade.Core.Commands;
using TextBlade.Core.Tests.TestHelpers;

namespace TextBlade.Core.Tests.Commands;

[TestFixture]
public class ShowCreditsCommandTests
{
    [Test]
    public async Task ShowCreditsCommand_ReturnsTextBladeMessage()
    {
        // Arrange
        var command = new ShowCreditsCommand();
        
        // Act
        var actuals = await AsyncToList.ToList(command.Execute(null, null));
        var actual = actuals.Single();

        // Assert
        Assert.That(actual, Does.Contain("TextBlade"));
        Assert.That(actual, Does.Contain("by NightBlade"));
    }
}
