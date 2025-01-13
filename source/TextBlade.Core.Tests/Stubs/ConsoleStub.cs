using System;
using TextBlade.Core.IO;

namespace TextBlade.Core.Tests.Stubs;

public class ConsoleStub : IConsole
{

    public string LastMessage { get { return this.Messages[this.Messages.Count - 1]; } }
    public List<string> Messages { get; } = new(); 
    
    private List<char> _keysPressed = new();
    
    public void PressKey(char key)
    {
        _keysPressed.Add(key);
    }

    public char ReadKey()
    {
        if (!_keysPressed.Any())
        {
            throw new InvalidOperationException("No keys pressed yet!");
        }

        var toReturn = _keysPressed.First();
        _keysPressed.RemoveAt(0);
        return toReturn;
    }


    public string ReadLine()
    {
        return string.Empty;
    }

    public void WriteLine(string markUp)
    {
        this.Messages.Add(markUp);
    }
}
