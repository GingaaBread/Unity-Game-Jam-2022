using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Assertions;
using System;

public class QuestPanel : MonoBehaviour {

    [SerializeField] private Transform containerForQuests;

    private List<GameObject> questsDisplayed = new List<GameObject>();

    public static QuestPanel Instance { get; private set; }

    private void Awake() {
        Assert.IsNull(Instance);
        Instance = this;

        Assert.IsNotNull(containerForQuests);
        
        ClearQuests();
    }

    public void ClearQuests() {
        questsDisplayed.Clear();
        foreach (Transform child in containerForQuests) {
            GameObject.Destroy(child.gameObject);
        }
    }

    internal void UpdateQuests(ResourceCollectionQuestSO[] allQuestsToShow) {
        ClearQuests();
        foreach(ResourceCollectionQuestSO quest in allQuestsToShow) {
            if (quest.prefabForDisplayingMissionUI != null) {
                GameObject questUIObj = GameObject.Instantiate(quest.prefabForDisplayingMissionUI, containerForQuests, false);
                questUIObj.GetComponent<QuestPanelItem>().Initialize(quest);
                questsDisplayed.Add(questUIObj);
            } else {
                Debug.Log("QuestPanel given a quest without a UI prefab to instantiate for it");
            }
        }
    }
}
