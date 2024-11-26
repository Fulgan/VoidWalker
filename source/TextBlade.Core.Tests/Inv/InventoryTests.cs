using NUnit.Framework;
using TextBlade.Core.Inv;

namespace TextBlade.Core.Tests.Inv;

[TestFixture]
public class InventoryTests
{
    [Test]
    [TestCase("Apple", 1, true)]
    [TestCase("Orange", 33, true)]
    [TestCase("Pear", 777, true)]
    [TestCase("Banana", 0, false)]
    public void Has_ReturnsTrue_IfItemIsInInventoryInAnyQuantity(string item, int inventoryQuantity, bool expected)
    {
        // Arrange
        var inventory = new Inventory();
        
        for (var i = 0; i < inventoryQuantity; i++)
        {
            inventory.Add(new Item(item));
        }

        // Act
        var actual = inventory.Has(item);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    [TestCase(null)]
    [TestCase("")]
    [TestCase("             ")]
    public void Add_Throws_IfItemIsEmpty(string itemName)
    {
        // Arrange
        var inventory = new Inventory();

        // Act/Assert
        Assert.Throws<ArgumentException>(() => inventory.Add(new Item(itemName), 7));
    }

    [Test]
    [TestCase(0)]
    [TestCase(-1)]
    [TestCase(-20)]
    [TestCase(-333)]
    public void Add_Throws_IfQuantityIsNonPositive(int quantity)
    {
        // Arrange
        var inventory = new Inventory();

        // Act/Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => inventory.Add(new Item("Super Awesome Item"), quantity));
    }

    [Test]
    public void Add_AddsToOrIncreasesQuantity()
    {
        // Arrange
        var inventory = new Inventory();

        // Act. With and without quantity; new and existing item.
        inventory.Add(new Item("Tomato"));
        inventory.Add(new Item("Carrot"), 3);
        inventory.Add(new Item("Bell Pepper"));
        inventory.Add(new Item("Bell Pepper"), 5);

        // Assert
        var actual = inventory.ItemQuantities;
        Assert.That(actual["Tomato"], Is.EqualTo(1));
        Assert.That(actual["Carrot"], Is.EqualTo(3));
        Assert.That(actual["Bell Pepper"], Is.EqualTo(6));
    }

    [Test]
    public void Add_TreatsItemsOfTheSameName_AsTheSameItem()
    {
        // Arrange
        var inventory = new Inventory();
        
        // Act
        inventory.Add(new Item("Starfruit"));
        inventory.Add(new Item("Starfruit"));
        inventory.Add(new Item("Starfruit"));

        // Assert
        Assert.That(inventory.ItemQuantities["Starfruit"], Is.EqualTo(3));
    }

    [Test]
    [TestCase(null)]
    [TestCase("")]
    [TestCase("             ")]
    public void Remove_Throws_IfItemIsEmpty(string itemName)
    {
        // Arrange
        var inventory = new Inventory();

        // Act/Assert
        Assert.Throws<ArgumentException>(() => inventory.Remove(itemName));
    }

    [Test]
    [TestCase(0)]
    [TestCase(-1)]
    [TestCase(-20)]
    [TestCase(-333)]
    public void Remove_Throws_IfQuantityIsNonPositive(int quantity)
    {
        // Arrange
        var inventory = new Inventory();

        // Act/Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => inventory.Remove("Super Awesome Item", quantity));
    }

    [Test]
    public void Remove_Throws_IfWeDontHaveTheItem()
    {
        // Arrange
        var inventory = new Inventory();

        // Act/Assert
        Assert.Throws<ArgumentException>(() => inventory.Remove("Steak"));
    }

    [Test]
    [TestCase(4)]
    [TestCase(44)]
    [TestCase(444)]
    public void Remove_Throws_IfWeDontHaveTheItemInSufficientQuantities(int askQuantity)
    {
        // Arrange
        var inventory = new Inventory();
        inventory.Add(new Item("Steak"), 3);

        // Act/Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => inventory.Remove("Steak", askQuantity));
    }

    [Test]
    public void Remove_DecrementsQuantity_AndRemovesItemIfQuantityBecomesZero()
    {
        // Arrange
        // Arrange
        var inventory = new Inventory();
        inventory.Add(new Item("Fried Chicken"), 3);
        inventory.Add(new Item("French Fries"), 3);
        inventory.Add(new Item("Onion Rings"), 3);

        // Act
        inventory.Remove("Fried Chicken");
        inventory.Remove("French Fries", 2);
        inventory.Remove("Onion Rings", 3);

        // Assert.
        Assert.That(inventory.ItemQuantities["Fried Chicken"], Is.EqualTo(2));
        Assert.That(inventory.ItemQuantities["French Fries"], Is.EqualTo(1));
        Assert.That(inventory.ItemQuantities.ContainsKey("Onion Rings"), Is.False);
    }
}
