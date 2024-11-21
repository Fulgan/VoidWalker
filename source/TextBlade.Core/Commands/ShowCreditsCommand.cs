using System;
using TextBlade.Core.Characters;
using TextBlade.Core.Game;

namespace TextBlade.Core.Commands;

public class ShowCreditsCommand : ICommand
{
    public IEnumerable<string> Execute(IGame game, List<Character> party)
    {
        var toReturn = new List<string>
        {
            "All programming, content, etc. created by NightBlade!",
            "Shout out to the following people for providing free resources:",
            "    Seagulls close-up.wav by juskiddink -- https://freesound.org/s/100724/ -- License: Attribution 4.0"
        };
        return toReturn;
    }
}
