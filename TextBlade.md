# TextBlade

Note: this is all ridiculously out of date, as development proceeds at an accelerated clip. Please remind me to come back and update this once things are stable.

TextBlade is a game engine I specifically created to help run text-based JRPGs (primarily to support making them for the Games for Blind Gamers jams).

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