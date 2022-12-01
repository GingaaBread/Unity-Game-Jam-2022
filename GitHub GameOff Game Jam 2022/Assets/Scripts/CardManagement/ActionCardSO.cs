using UnityEngine;


public abstract class ActionCardSO : ScriptableObject
{
    public Sprite Background;
    public Sprite PreviewBackground;
    public Sprite cardSprite;
    public Sprite summaryBackground;
    public Sprite costBackground;
    public string cardTitle;
    public string cardSubtitle;
    public string cardSummary;
    public string[] cardEffectKeys;
    public string[] cardEffectValues;
    [TextArea(3,3)]
    public string[] bonusEffectTextDescription; // these could be filled by the Bonus script but time :/
    [TextArea(3,3)]
    public string[] bonusEffectValues; // these could be filled by the Bonus script but time :/
    public Sprite[] bonusEffectSprites; // these could be filled by the Bonus script but time :/
    public int cardCost = 10;


    public abstract void Action(UICardPanel uiPanel);

    public override string ToString()
    {
        return $"{cardTitle} ({cardCost}), [{cardSummary}]";
    }
}
