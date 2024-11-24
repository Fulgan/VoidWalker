using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TextBlade.Core.Battle;
using TextBlade.Core.Characters;

namespace TextBlade.Core.IO;

public static class Serializer
{
    private static readonly Formatting FormattingSettings = Formatting.Indented;
    private static readonly JsonSerializerSettings TypeSerializationSettings = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All };
    
    public static string Serialize(object o)
    {
        return JsonConvert.SerializeObject(o, FormattingSettings, TypeSerializationSettings);
    }

    public static T Deserialize<T>(string serializedJson) where T : class
    {
        return JsonConvert.DeserializeObject<T>(serializedJson, TypeSerializationSettings);
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

            // Convert an array of skill names, into an array of Skill instances.
            foreach (var jsonToken in member["SkillNames"])
            {
                var skill = DeserializeSkill(allSkillData, jsonToken);
                character.Skills.Add(skill);
            }
            
            toReturn.Add(character);
        }
        return toReturn;
    }

    private static Skill DeserializeSkill(JObject allSkillData, JToken jsonToken)
    {
        var skillName = jsonToken.Value<string>();
        if (skillName == null)
        {
            throw new ArgumentException("Skill name is empty/null");
        }

        var skillData = allSkillData[skillName];
        if (skillData == null)
        {
            throw new ArgumentException($"Skill data for {skillName} is null");
        }

        var skill = Deserialize<Skill>(skillData.ToString());
        skill.Name = skillName;
        return skill;
    }
}
