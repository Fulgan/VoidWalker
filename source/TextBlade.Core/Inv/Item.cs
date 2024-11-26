namespace TextBlade.Core.Inv;

public class Item
{
    public string Name { get; set; }
    public ItemType ItemType { get; set; }

    public Item(string name)
    {
        Name = name;
    }
}
