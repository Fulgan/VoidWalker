# TextBlade

TextBlade is a game engine I specifically created to help run text-based JRPGs (primarily to support making them for the Games for Blind Gamers jams).

# Creating a Game

- Run `dotnet new console --name <projectName>` to create your new project
- Clone the lastest `TextBlade` repo, add it as a submodule, or copy the projects under your source directory
- Add a reference to `TextBlade.Player` in your project
- In your `Program.cs`, call `new Game().Run()`
- Create a `Content` directory with a `game.json` file inside; it should have a `GameName` property set
- Set `game.json` to copy on build by adding this to your `.csproj` file:
```xml
  <ItemGroup>
    <Content Include="Content/*">
          <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
    </Content>
  </ItemGroup>
``` 
- Run your game. It should run!