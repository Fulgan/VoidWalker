using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace TextBlade.Core.Inv;

[JsonConverter(typeof(StringEnumConverter))]
public enum CharacterStats
{
    Strength = 0,
    Toughness = 1,
    Special = 2,
    SpecialDefense = 3,
    TotalHealth = 4,
    TotalSkillPoints = 5,
}
