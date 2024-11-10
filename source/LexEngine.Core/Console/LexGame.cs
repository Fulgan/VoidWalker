namespace LexEngine.Core;

using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class LexGame
{
    public void Run()
    {
        var gameJsonPath = Path.Join("Content", "game.json");

        if (!File.Exists(gameJsonPath))
        {
            throw new InvalidOperationException("Content/game.json file is missing!");
        }

        var gameJsonContents = File.ReadAllText(gameJsonPath);
        var gameJson = JsonConvert.DeserializeObject(gameJsonContents) as JObject;
        var gameName = gameJson["GameName"];

        Console.WriteLine($"Welcome to {gameName}!");
    }
}
