using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace TextBlade.Core.Inv;

[JsonConverter(typeof(StringEnumConverter))]
public enum ItemType
{
    Consumable = 0,
    Helmet = 1,
    Armour = 2,
    Weapon = 3,
}