namespace TextBlade.Core.Characters;

public class QuestGiver : Npc
{
    /// <summary>
    /// The texts this NPC says after the quest is done.
    /// </summary>
    public string[] PostQuestTexts { get; }

    /// <summary>
    /// Which switch indicates that this quest is complete? It must be a boolean switch, and set to true, to indicate completion.
    /// </summary>
    public string QuestCompleteSwitchName { get; }

    // Texts are pre-quest texts
    public QuestGiver(string name, string[] texts, string[] postQuestTexts, string questCompleteSwitchName) : base(name, texts)
    {
        PostQuestTexts = postQuestTexts;
        QuestCompleteSwitchName = questCompleteSwitchName;
    }
}
