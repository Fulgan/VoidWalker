using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TextBlade.Core.Inv;

namespace TextBlade.Core.IO;

public static class ItemsData
{
    /// <summary>
    /// Cached item JSON data, since items don't change mid-game, and are required for dungeons, shops, etc.
    /// </summary>
    private static JObject s_itemJson;

    static ItemsData()
    {
        // TODO: validation etc. of the file path. And maybe the JSON.
        var jsonContents = File.ReadAllText(Path.Join("Content", "Data", "Items.json"));
        var itemJson = JsonConvert.DeserializeObject<JObject>(jsonContents);

        if (itemJson == null)
        {
            throw new InvalidOperationException("Content/Data/Items.json doesn't seem to be valid JSON");
        }

        s_itemJson = itemJson;
    }

    internal static Item GetItem(string itemName)
    {
        var jsonBlob = s_itemJson[itemName];
        if (jsonBlob == null)
        {
            throw new InvalidOperationException($"Items.json doesn't seem to have data for {itemName}");
        }

        // Code smells... Not sure how to fix it...
        return Serializer.Deserialize<Item>(jsonBlob.ToString());
    }
}
