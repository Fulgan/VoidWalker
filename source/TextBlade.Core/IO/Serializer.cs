using System;
using Newtonsoft.Json;

namespace TextBlade.Core.IO;

public static class Serializer
{
    private static readonly JsonSerializerSettings SerializationSettings = new JsonSerializerSettings() 
    {
         TypeNameHandling = TypeNameHandling.All
    };

    private static readonly Formatting FormattingSettings = Formatting.Indented;
    
    public static string Serialize(object o)
    {
        return JsonConvert.SerializeObject(o, FormattingSettings, SerializationSettings);
    }

    public static T Deserialize<T>(string serializedJson)
    {
        return JsonConvert.DeserializeObject<T>(serializedJson, SerializationSettings);
    }
}
