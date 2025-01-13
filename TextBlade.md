# TextBlade

Note: this is all ridiculously out of date, as development proceeds at an accelerated clip. Please remind me to come back and update this once things are stable.

TextBlade is a game engine I specifically created to help run text-based JRPGs (primarily to support making them for the Games for Blind Gamers jams).

Note that you still need to write code to use TextBlade; you can leverage a lot of our base code, yes. Each game will need its own requirements and customization, so go ahead and write your own code for that.


# Creating a Game

Just copy VoidWalkers. It's a very minimal tech demo.

# Creating a Location

Let's add a location to our game: a starting town called King's Vale.

- Modify your `game.json` and add a `StartingLocation` attribute. Set the value to `KingsVale`
- In `Content`, create a folder called `Locations` with a file called `KingsVale.json`
- In `KingsVale.json`, add a `Name`, `Description`, and `"LocationType": "Town"` attributes.

Run the game again. You should get a print statement indicating you're in King's Vale.

## Adding Code to the Location

- Add the attribute `LocationClass` to your `.json` file, and specify the class name, e.g. `ThroneRoom`
- Create a matching `ThroneRoom.cs` file anywhere in your project (outside of `Content` of course)
- Add the `[LocationCode]` attribute to it
- Add whatever custom code you like to the constructor

Note that TextBlade manages and serializes/deserializes global game-wide switches; access them via the `TextBlade.Core.Game.GameSwitches.Switches` class.


## Game Switches

The game code allows you to set boolean switches for things, like global conditions (e.g. is quest XYZ done?). 

By default, whenever you complete a dungeon, TextBlade also sets a switch for you; the switch name is `CompletedDungeon_{dungeonName}`, where `dungeonName` is the dungeon's name, without spaces or other special characters (e.g. `North Seaside Town` becomes `NorthSeasideTown`).


## NPCs

NPCs need a name, like `Dock Worker`, and can say multiple things. If they say multiple things, the NPC rotates through them in order as you talk to him again and again.

## Quest Givers

A special type of NPCs, quest givers say one thing when a quest is pending, and another once the quest is completed. Since they're so special, talking to a quest giver automatically sets the switch `TalkedTo_{questGiverName}` where `questGiverName` is the quest giver's name, without spaces or special characters.

# Credits

TextBlade includes a `credits` command. If you want to show credits, such as citation of audio resources, create a `Credits.txt` file (with a capital C) in your Content directory. Typing credits will read this out.