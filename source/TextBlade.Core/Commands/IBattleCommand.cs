namespace TextBlade.Core.Commands;

/// <summary>
/// Used for things that are self-contained battles. Like, "battle engines" or whatever.
/// </summary>
public interface IBattleCommand : ICommand
{
    public bool IsVictory { get; }
    public int TotalGold { get; }
    public int TotalExperiencePoints { get; }
}
