using System;
using TextBlade.Core.IO;

namespace TextBlade.Core.Tests.Stubs;

public class ConsoleStub : IConsole
{

    public string LastMessage { get { return this.Messages[this.Messages.Count - 1]; } }
    public List<string> Messages { get; } = new(); 
    
    public char ReadKey()
    {
        throw new NotImplementedException();
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
