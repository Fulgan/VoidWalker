using NUnit.Framework;
using TextBlade.Core.Inv;
using TextBlade.Core.IO;

namespace TextBlade.Core.Tests.IO;

[TestFixture]
public class ItemsDataTests
{
    [Test]
    public void GetItem_InfersEquipmentAsEquipmentType()
    {
        // Arrange is done in Items.json
        // Act
        var actual = ItemsData.GetItem("Iron Helmet");
        
        // Assert
        Assert.That(actual, Is.InstanceOf<Equipment>());
    }
}
