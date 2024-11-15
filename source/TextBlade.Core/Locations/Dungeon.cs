using System;

namespace TextBlade.Core.Locations;

public class Dungeon : Location
{
    // It's a list, one entry per floor.
    // Each entry is a list of monsters and other stuff on that floor.
    // I'm sure I'll put treasure and stuff in here eventually.
    private List<List<string>> _floorMonsters = new();

    public Dungeon(string name, string description, int numFloors, List<string> monsters, string locationClass = null) : base(name, description, locationClass)
    {
        if (numFloors <= 0)
        {
            throw new ArgumentException(nameof(numFloors));
        }

        if (!monsters.Any())
        {
            throw new ArgumentException(nameof(monsters));
        }

        for (int i = 0; i < numFloors; i++)
        {
            var currentFloorOccupants = new List<string>();
            
            // Assumes monsters are in order of difficulty, roughly.
            // Which monsters can we spawn on this floor? Up to (floor_number + 1) inclusive.
            var lastMonsterId = Math.Min(i + 2, monsters.Count);
            
            List<string> validMonsters = [];
            for (int j = 0; j < lastMonsterId; j++)
            {
                validMonsters.Add(monsters[j]);
            }
            
            var pointsLeft = (i + 1) * 5;
            while (pointsLeft > 0)
            {
                var nextMonsterIndex = Random.Shared.Next(0, lastMonsterId);
                currentFloorOccupants.Add(validMonsters[nextMonsterIndex]);
                pointsLeft -= (nextMonsterIndex + 1);
            }

            _floorMonsters.Add(currentFloorOccupants);
        }
    }
}
