namespace TextBlade.Core.IO;

public interface IConsole
{
    public void WriteLine(string markUp);
    // Used to enter commands ONLY, and custom commands, like "sleep"
    public string ReadLine();
    public char ReadKey();
}
