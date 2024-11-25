using NUnit.Framework;
using TextBlade.Core.Commands;

namespace TextBlade.Core.Tests.Commands;

[TestFixture]
public class DoNothingCommandTests
{
    private readonly string[] EmptyStringArray = [string.Empty]; 
    
    [Test]
    public void Execute_DoesNothing_LiterallyAsync()
    {
        // Arrange/Act/Assert
        Assert.That(new DoNothingCommand().Execute(null, null), Is.EqualTo(EmptyStringArray));
    }
}
