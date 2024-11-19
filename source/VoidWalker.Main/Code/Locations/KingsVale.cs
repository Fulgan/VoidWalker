using TextBlade.ConsoleRunner;
using TextBlade.Core.Game;
using TextBlade.Core.Locations;
using TextBlade.Core.UserCode;

namespace VoidWalker.Main.Code.Locations;

public class KingsVale : LocationCodeBehind
{
    public KingsVale()
    {
    }

    override public void BeforeShowingLocation(Location currentLocation)
    {
        if (Game.Current.Switches.HasSwitch(Switches.MetKingSulayman))
        {
            currentLocation.LinkedLocations.Add(new LocationLink("Dungeons/NorthSeasideCave", "Explore the North Seaside Cave"));
        }
    }
}
