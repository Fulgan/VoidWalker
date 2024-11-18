using System;

namespace TextBlade.Core.Commands;

/// <summary>
/// Used for things that are self-contained battles. Like, "battle engines" or whatever.
/// </summary>
public interface IBattleCommand
{
    public bool IsVictory { get; }
}
