using System;
using Newtonsoft.Json;
using TextBlade.Core.Characters;
using TextBlade.Core.Game;

namespace TextBlade.Core.IO;

public static class SaveGameManager
{
    private const string SaveFolder = "SaveData";
    private const string SaveFileExtension = ".save";

    public static void SaveGame(string saveSlot, List<Character> party)
    {
        var saveData = new Dictionary<string, object>
        {
            { "party", party },
            { "switches", GameSwitches.Switches },
            { "gold", 0 },
            { "inventory", new Dictionary<string, int>() },
            { "totalGameTimeSeconds", 0 }
        };

        var serialized = JsonConvert.SerializeObject(saveData);
        if (!Directory.Exists(SaveFolder))
        {
            Directory.CreateDirectory(SaveFolder);
        }
        
        var path = Path.Join(SaveFolder, $"{saveSlot}{SaveFileExtension}");
        File.WriteAllText(path, serialized);
    }

    public static Dictionary<string, object> LoadGame(string saveSlot)
    {
        var path = Path.Join(SaveFolder, $"{saveSlot}{SaveFileExtension}");
        if (!File.Exists(path))
        {
            throw new ArgumentException(nameof(saveSlot));
        }

        var json = File.ReadAllText(path);
        var deserialized = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
        return deserialized;
    }
}
