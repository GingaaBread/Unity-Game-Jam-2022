using PlayerData;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

/// <summary>
/// 
/// </summary>
/// <author>Ben</author>
public class QuestManager : MonoBehaviour {

    public static QuestManager Instance { get; private set; }

    [Tooltip("Debugging")]
    [SerializeField] private bool DebugMode_AutoInitOnStart;

    [Tooltip("Pool Of ResourceCollection Quests To Choose From")] 
    [SerializeField] private ResourceCollectionQuestSO[] ResourceCollectionQuests;
    private ResourceCollectionQuestSO[] _activeResourceCollectionQuests;

    private void Awake() {
        Assert.IsNull(Instance);
        Instance = this;

        Assert.IsNotNull(ResourceCollectionQuests);
        Assert.IsTrue(ResourceCollectionQuests.Length >= 3, "need pool of at least 3 RC quests so we can choose 3");
    }

    private void Start() {
        Assert.IsNotNull(QuestPanel.Instance, "QuestManager expects QuestPanel to also exist in the scene");

        if (DebugMode_AutoInitOnStart) {
            OnInitializeGame();
        }
    }

    private void OnInitializeGame() {
        _activeResourceCollectionQuests = RandomlyChooseRCQuests(3);
        foreach(ResourceCollectionQuestSO quest in _activeResourceCollectionQuests)
            quest.ResetActualsCounters();
        QuestPanel.Instance.UpdateQuests(_activeResourceCollectionQuests);
    }

    private ResourceCollectionQuestSO[] RandomlyChooseRCQuests(int count) {
        Assert.IsTrue(ResourceCollectionQuests.Length >= count);

        List<ResourceCollectionQuestSO> tempList = new List<ResourceCollectionQuestSO>(ResourceCollectionQuests);
        ResourceCollectionQuestSO[] chosenQuests = new ResourceCollectionQuestSO[count];

        for(int i=0; i < count; i++) {
            chosenQuests[i] = tempList[UnityEngine.Random.Range(0, tempList.Count)];
            tempList.Remove(chosenQuests[i]);
        }

        return chosenQuests;
    }

    public void NotifyOfResourceCollected(ResourceSO resource, int countCollected) {
        foreach(ResourceCollectionQuestSO quest in _activeResourceCollectionQuests) {
            quest.NotifyOfResourceCollected(resource, countCollected);
        }
    }


}

