using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TextBlade.Core.Battle;
using TextBlade.Core.Characters;

namespace TextBlade.Core.IO;

public static class Serializer
{
    private static readonly Formatting s_formattingSettings = Formatting.Indented;
    private static readonly JsonSerializerSettings s_typeSerializationSettings = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All };
    
    public static string Serialize(object o)
    {
        return JsonConvert.SerializeObject(o, s_formattingSettings, s_typeSerializationSettings);
    }

    public static T Deserialize<T>(string serializedJson)
    {
        return JsonConvert.DeserializeObject<T>(serializedJson, s_typeSerializationSettings);
    }

    public static List<Character> DeserializeParty(JArray partyMembers)
    {
        // This could do with more error handling.
        var allSkillData = JsonConvert.DeserializeObject<JObject>(File.ReadAllText("Content/Data/Skills.json"));
        if (allSkillData == null)
        {
            throw new InvalidOperationException("Can't parse skill data.");
        }

        var toReturn = new List<Character>();
        foreach (var member in partyMembers)
        {
            var character = JsonConvert.DeserializeObject<Character>(member.ToString());
            toReturn.Add(character);

        }
        return toReturn;
    }

    public static IDictionary<string, Skill> DeserializeSkillsData()
    {
        var allSkillData = JsonConvert.DeserializeObject<JObject>(File.ReadAllText("Content/Data/Skills.json"));
        if (allSkillData == null)
        {
            throw new InvalidOperationException("Can't parse skill data.");
        }

        var toReturn = new Dictionary<string, Skill>();

        foreach (var skillData in allSkillData)
        {
            var skill = DeserializeSkill(allSkillData, skillData);
            toReturn[skill.Name] = skill;
        }

        return toReturn;
    }

    private static Skill DeserializeSkill(JObject allSkillData, KeyValuePair<string, JToken> jsonToken)
    {
        var skillName = jsonToken.Key;
        if (skillName == null)
        {
            throw new ArgumentException("Skill name is empty/null");
        }

        var skillData = jsonToken.Value;
        if (skillData == null)
        {
            throw new ArgumentException($"Skill data for {skillName} is null");
        }

        var skill = Deserialize<Skill>(jsonToken.Value.ToString());
        skill.Name = skillName;
        return skill;
    }
}
