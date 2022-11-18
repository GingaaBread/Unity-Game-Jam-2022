using PlayerData;
using UnityEngine;
using UnityEngine.Events;

public abstract class AbstractQuestSO : ScriptableObject {

    public string questName;
    public GameObject prefabForDisplayingMissionUI;

    [HideInInspector] public UnityEvent OnUpdate = new UnityEvent(); // for UI components to listen for updates

    public abstract void NotifyOfResourceCollected(ResourceSO resource, int countCollected);
    public abstract void NotifyOfTilePlaced(ActionCardSO card);
    public abstract void NotifyOfResourceSale(ResourceSO resource, int MoneyEarnedFromSale);
    public abstract string GetQuestAsSentence();
    public abstract string GetStatusAsSentence();
    public abstract void ResetActualsCounters();
    public abstract float GetPercentageCompleted();

}

