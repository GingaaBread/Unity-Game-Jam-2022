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
    public int cardCost = 10;


    public abstract void Action(UICardPanel uiPanel);

    public override string ToString()
    {
        return $"{cardTitle} ({cardCost}), [{cardSummary}]";
    }
}
