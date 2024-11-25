using NUnit.Framework;
using TextBlade.Core.Commands;
using TextBlade.Core.Tests.TestHelpers;

namespace TextBlade.Core.Tests.Commands;

[TestFixture]
public class ManuallySaveCommandTests
{
    private readonly string[] EmptyStringArray = [string.Empty]; 


    [Test]
    public async Task Execute_DoesNothing_LiterallyAsync()
    {
        // Arrange/Act/Assert
        Assert.That(await AsyncToList.ToList(new ManuallySaveCommand().Execute(null, null)), Is.EqualTo(EmptyStringArray));
    }
}
