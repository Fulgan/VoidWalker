using NUnit.Framework;
using TextBlade.Core.Collections;

namespace TextBlade.Core.Tests.Collections;

[TestFixture]
public class WeightedBagTests
{
    [Test]
    public void GetRandom_ReturnsItemBasedOnWeights()
    {
        // Arrange
        var bag = new WeightedRandomBag<string>(new Dictionary<string, double>()
        {
            // Notice they don't add up to 1.0
            { "Apple", 0.7f },
            { "Blueberry", 0.3f },
            { "Pear", 0.1f },
        });

        // Act
        var results = new Dictionary<string, int>();
        for (int i = 0; i < 100; i++)
        {
            var next = bag.GetRandom();
            if (!results.ContainsKey(next))
            {
                results[next] = 1;
            }
            else
            {
                results[next]++;
            }
        }

        // Assert
        Assert.That(results.ContainsKey("Apple"));
        Assert.That(results.ContainsKey("Blueberry"));
        Assert.That(results.ContainsKey("Pear"));

        // Approximate counts
        Assert.That(results["Apple"], Is.GreaterThan(results["Blueberry"]));
        Assert.That(results["Blueberry"], Is.GreaterThan(results["Pear"]));
    }
}
