using PlayerData;
using UnityEngine;
using UnityEngine.Events;

public abstract class AbstractQuestSO : ScriptableObject {

    public GameObject prefabForDisplayingMissionUI;

    public UnityEvent OnUpdate = new UnityEvent(); // for UI components to listen for updates

    public abstract void NotifyOfResourceCollected(ResourceSO resource, int countCollected);
    public abstract string GetQuestAsSentence();
    public abstract string GetStatusAsSentence();
    public abstract void ResetActualsCounters();

}

