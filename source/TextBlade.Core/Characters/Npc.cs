namespace TextBlade.Core.Characters;

public class Npc
{
    protected int _readNextIndex = 0;
    private string[] _texts = [];
    
    public string Name { get; }
    public string[] Texts
    {
        get { return _texts; }
        set {
            _texts = value;
            _readNextIndex = 0;
        }
    }

    public Npc(string name, string[] texts)
    {
        this.Name = name;
        this.Texts = texts;
    }

    public virtual string Speak()
    {
        var toReturn = _texts[_readNextIndex];
        _readNextIndex = (_readNextIndex + 1) % Texts.Length;
        return toReturn;
    }
}
