using UnityEngine;


public abstract class ActionCardSO : ScriptableObject
{
    public Sprite cardSprite;
    public string cardTitle;
    public string cardSummary;
    public string[] cardEffectKeys;
    public string[] cardEffectValues;
    public int cardCost = 10;

    public abstract void Action();

    public override string ToString()
    {
        return $"{cardTitle} ({cardCost}), [{cardSummary}]";
    }
}