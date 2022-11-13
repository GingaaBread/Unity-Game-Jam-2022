using PlayerData;
using UnityEngine;
using System.Collections.Generic;
using TMPro;
using System;
using UnityEngine.Assertions;

[CreateAssetMenu(fileName = "New Resource Collection SO", menuName = "Quests/ResourceCollectionSO")]
public class ResourceCollectionQuestSO : AbstractQuestSO {

    [SerializeField] public ResourceSO[] targetResources;
    [SerializeField] public int[] targetQuantity;
    private int[] actualQuantity;

    public override string GetQuestAsSentence() {
        string s = "Collect";
        for(int i = 0; i < targetResources.Length; i++) {
            if (i > 0)
                s+= " and";
            s += $" {targetQuantity[i]} {targetResources[i].name.ToLower()}";
        }
        return s;
    }

    public override string GetStatusAsSentence() {
        return string.Join(", ", actualQuantity);
    }

    public override void ResetActualsCounters() {
        actualQuantity = new int[targetResources.Length];
        OnUpdate.Invoke();
    }

    public override void NotifyOfResourceCollected(ResourceSO resource, int countCollected) {
        Assert.IsNotNull(actualQuantity);
        for (int i = 0; i < targetResources.Length; i++) {
            if(targetResources[i] == resource) {
                actualQuantity[i] += countCollected;
                OnUpdate.Invoke();
                return;
            }
        }
    }

    public bool IsComplete(Dictionary<ResourceSO, int> countOfResourcesCollected) {

        // if any target resource has no count, return false
        foreach(ResourceSO targetResource in targetResources) {
            if (!countOfResourcesCollected.ContainsKey(targetResource))
                return false;
        }

        // if any target resource has less count than target, return false
        for(int i=0; i<targetResources.Length; i++) {
            if (countOfResourcesCollected[targetResources[i]] < targetQuantity[i])
                return false;
        }

        return true;
    }

}
