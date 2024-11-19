using System.Runtime.Serialization;
using Newtonsoft.Json;
using TextBlade.Core.Characters;
using TextBlade.Core.Game;

namespace TextBlade.Core.IO;

public static class SaveGameManager
{
    private const string SaveFolder = "SaveData";
    private const string SaveFileExtension = ".save";

    public static void SaveGame(string saveSlot, string currentLocationId, List<Character> party)
    {
        var saveData = new SaveData()
        {
            Party = party,
            Switches = GameSwitches.Switches,
            CurrentLocationId = currentLocationId,
            
            Gold = 0,
            Inventory = new (),
            TotalGameTimeSeconds = 0,
        };

        var serialized = JsonConvert.SerializeObject(saveData);
        if (!Directory.Exists(SaveFolder))
        {
            Directory.CreateDirectory(SaveFolder);
        }
        
        var path = Path.Join(SaveFolder, $"{saveSlot}{SaveFileExtension}");
        File.WriteAllText(path, serialized);
    }

    public static bool HasSave(string saveSlot)
    {
        var path = Path.Join(SaveFolder, $"{saveSlot}{SaveFileExtension}");
        return File.Exists(path);
 
    }

    public static SaveData LoadGame(string saveSlot)
    {
        var path = Path.Join(SaveFolder, $"{saveSlot}{SaveFileExtension}");
        if (!File.Exists(path))
        {
            throw new ArgumentException(nameof(saveSlot));
        }

        var json = File.ReadAllText(path);
        var deserialized = JsonConvert.DeserializeObject<SaveData>(json);
        ValidateSaveData(deserialized);
        return deserialized;
    }

    private static void ValidateSaveData(SaveData data)
    {
        if (string.IsNullOrWhiteSpace(data.CurrentLocationId))
        {
            throw new SerializationException($"Your save data looks corrupt: the current location is missing");
        }

        if (data.Party.Count <= 0)
        {
            throw new SerializationException($"Your save data looks corrupt: there are no party members");
        }
        
        if (data.Gold < 0)
        {
            throw new SerializationException($"Your save data looks corrupt: gold is {data.Gold}");
        }
    }
}
