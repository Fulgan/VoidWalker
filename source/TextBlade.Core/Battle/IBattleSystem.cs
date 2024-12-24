using TextBlade.Core.IO;

namespace TextBlade.Core.Battle;

public interface IBattleSystem
{
    public Spoils Execute(SaveData saveData);
}
