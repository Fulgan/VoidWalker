using Spectre.Console;
using TextBlade.Core.IO;

namespace TextBlade.Platform.Windows.IO;

public class TextConsole : IConsole
{
    public TextConsole()
    {
        // Assume nothing. Set background.
        AnsiConsole.Background = Color.Black;
    }

    /// <summary>
    /// Guarantees a lower-case key
    /// </summary>
    /// <returns></returns>
    public char ReadKey()
    {
        return Console.ReadKey().KeyChar.ToString().ToLower()[0];
    }

    public void WriteLine(string markup)
    {
        AnsiConsole.MarkupLine(markup);
    }
}
