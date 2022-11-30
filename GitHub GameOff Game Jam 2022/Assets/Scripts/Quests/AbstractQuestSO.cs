using PlayerData;
using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public abstract class AbstractQuestSO : ScriptableObject {

    public string textPrompt;
    [HideInInspector] public UnityEvent OnUpdate = new UnityEvent(); // for UI components to listen for updates
    [HideInInspector] public UnityEvent OnCompletion = new UnityEvent(); // for UI components to listen for completion

    public abstract void NotifyOfResourceCollected(ResourceSO resource, int countCollected);
    public abstract void NotifyOfTilePlaced(ActionCardSO card);
    public abstract void NotifyOfResourceSale(ResourceSO resource, int MoneyEarnedFromSale);
    public abstract string GetStatusAsSentence();
    public abstract void ResetActualsCounters();
    public abstract float GetPercentageCompleted();

}

