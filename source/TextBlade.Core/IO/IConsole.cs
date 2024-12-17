namespace TextBlade.Core.IO;

public interface IConsole
{
    public void WriteLine(string markUp);
    public char ReadKey();
}
