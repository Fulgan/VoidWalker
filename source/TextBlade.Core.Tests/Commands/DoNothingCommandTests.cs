using NUnit.Framework;
using TextBlade.Core.Commands;

namespace TextBlade.Core.Tests.Commands;

[TestFixture]
public class DoNothingCommandTests
{
    [Test]
    public void Execute_DoesNothing_Literally()
    {
        // Arrange/Act/Assert
        Assert.That(new DoNothingCommand().Execute(null, null), Is.Empty);
    }
}
