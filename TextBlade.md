# TextBlade

Note: this is all ridiculously out of date, as development proceeds at an accelerated clip. Please remind me to come back and update this once things are stable.

TextBlade is a game engine I specifically created to help run text-based JRPGs (primarily to support making them for the Games for Blind Gamers jams).

Note that you still need to write code to use TextBlade; you can leverage a lot of our base code, yes. Each game will need its own requirements and customization, so go ahead and write your own code for that.


# Creating a Game

- Run `dotnet new console --name <projectName>` to create your new project
- Clone the lastest `TextBlade` repo, add it as a submodule, or copy the projects under your source directory
- Add a reference to `TextBlade.ConsoleRunner` in your project
- In your `Program.cs`, call `new Game().Run()`
- Create a `Content` directory with a `game.json` file inside; it should have a `GameName` property set
- Set `game.json` to copy all files from `Content`, by adding this to your `.csproj` file:
```xml
  <ItemGroup>
    <Content Include="Content/**/*">
          <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
    </Content>
  </ItemGroup>
``` 

Run your game. It should crash! Yes! Because games need a location!

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