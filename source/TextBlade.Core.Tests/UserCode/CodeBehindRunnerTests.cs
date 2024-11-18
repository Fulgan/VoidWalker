using System.Reflection;
using NUnit.Framework;
using TextBlade.Core.Game;
using TextBlade.Core.Locations;
using TextBlade.Core.UserCode;

namespace TextBlade.Core.Tests.UserCode;

[TestFixture]
public class CodeBehindRunnerTests
{
    [Test]
    public void ExecuteLocationCode_DoesNotThrow_IfClassNameIsNull()
    {
        // Act/Assert
        Assert.DoesNotThrow(() => CodeBehindRunner.ExecuteLocationCode(new Location("a", "b", null)));
    }

    [Test]
    [TestCase("")]
    [TestCase("              ")]
    public void ExecuteLocationCode_Throws_IfClassNameIsEmptyStringOrWhiteSpace(string className)
    {
        // Act/Assert
        var ex = Assert.Throws<ArgumentException>(() => CodeBehindRunner.ExecuteLocationCode(new Location("a", "b", className)));
        Assert.That(ex.Message, Does.Contain("empty LocationClass"));
    }

    [Test]
    [TestCase("NotARealClass")]
    [TestCase("FloatingIsland17")]
    [TestCase("Throne_Room")]
    public void ExecuteLocationCode_Throws_IfClassNameIsNotRegistered(string className)
    {
        // Act/Assert
        var ex = Assert.Throws<ArgumentException>(() => CodeBehindRunner.ExecuteLocationCode(new Location("a", "b", className)));
        Assert.That(ex.Message, Does.Contain("TextBlade can't find the class"));
    }

    [Test]
    public void ExecuteLocationCode_ExecutesConstructorCode_IfClassExists()
    {
        // Arrange: See PhantomForest class below.
        // WARNING: has side effects. Assembly's classes will always be registered in subsequent tests.
        CodeBehindRunner.RegisterAssemblyClasses(Assembly.GetExecutingAssembly());

        // Act
        CodeBehindRunner.ExecuteLocationCode(new Location("Phantom Forest", "See: FF6", "PhantomForest"));

        // Assert
        var actual = PhantomForest.LatestInstance;
        Assert.That(actual.ConstructorInvoked, Is.True);
        Assert.That(actual.BeforeShowingLocationCalled, Is.True);
        Assert.That(actual.MethodInvoked, Is.False);
    }

    class PhantomForest : LocationCodeBehind
    {
        public static PhantomForest LatestInstance { get; private set; } = null!;
        public bool ConstructorInvoked { get; private set; } = false;
        public bool MethodInvoked { get; private set; } = false;
        public bool BeforeShowingLocationCalled  { get; private set; } = false;

        public PhantomForest()
        {
            LatestInstance = this;
            this.ConstructorInvoked = true;
        }

        public void Method()
        {
            this.MethodInvoked = true;
        }

        public override void BeforeShowingLocation(Location currentLocation)
        {
            this.BeforeShowingLocationCalled = true;
        }
    }
}
