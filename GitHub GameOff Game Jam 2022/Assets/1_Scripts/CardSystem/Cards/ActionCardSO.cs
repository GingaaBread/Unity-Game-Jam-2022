using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public abstract class ActionCardSO : ScriptableObject
{

    public Sprite cardSprite;
    public Sprite buildingSprite;
    public string cardTitle;
    public string cardSummary;
    public string[] cardEffectKeys;
    public string[] cardEffectValues;
    public int cardCost = 10;

    public abstract void Action();
}
