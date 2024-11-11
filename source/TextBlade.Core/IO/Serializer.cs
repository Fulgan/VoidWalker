using System;
using Newtonsoft.Json;

namespace TextBlade.Core.IO;

public static class Serializer
{
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
