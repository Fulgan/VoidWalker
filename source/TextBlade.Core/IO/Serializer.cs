using System;
using Newtonsoft.Json;

namespace TextBlade.Core.IO;

public static class Serializer
{
    // TODO: delete, unless we don't know the type coming back and need to serialize type info
    private static readonly JsonSerializerSettings SerializationSettings = new JsonSerializerSettings() 
    {
         TypeNameHandling = TypeNameHandling.All
    };

    private static readonly Formatting FormattingSettings = Formatting.Indented;
    
    public static string Serialize(object o)
    {
        return JsonConvert.SerializeObject(o, FormattingSettings);
    }

    public static T Deserialize<T>(string serializedJson) where T : class
    {
        return JsonConvert.DeserializeObject<T>(serializedJson);
    }
}
