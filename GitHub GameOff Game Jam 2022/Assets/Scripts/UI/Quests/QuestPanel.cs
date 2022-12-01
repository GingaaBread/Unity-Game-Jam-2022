using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class QuestPanel : MonoBehaviour {

    [SerializeField] private Transform containerForQuests;
    [SerializeField] private Animator panelAnimator;

    private List<GameObject> questsDisplayed = new List<GameObject>();

    public static QuestPanel Instance { get; private set; }

    private void Awake() {
        Assert.IsNull(Instance);
        Instance = this;

        Assert.IsNotNull(containerForQuests);
        
        ClearQuests();
    }

    public void DisplayCompletionAnimation() => panelAnimator.Play("QuestCompleted");

    public void OpenDetailMenu() => QuestDetailPanel.Instance.Open();

    public void ClearQuests() {
        questsDisplayed.Clear();
        foreach (Transform child in containerForQuests) {
            Destroy(child.gameObject);
        }
    }

    public void SetupPanel(List<BaseQuest> allQuestsToShow) {

        UpdateSelf(allQuestsToShow);
    }

    private void UpdateSelf(List<BaseQuest> allQuestsToShow)
    {
        ClearQuests();

        var questUIPrefab = QuestManager.Instance.questUIPrefab;

        foreach (BaseQuest quest in allQuestsToShow)
        {
            if (questUIPrefab != null)
            {
                GameObject questUIObj = Instantiate(questUIPrefab, containerForQuests, false);
                questUIObj.GetComponent<QuestPanelItem>().Initialize(quest);
                questsDisplayed.Add(questUIObj);
            }
            else
            {
                Debug.Log("QuestPanel given a quest without a UI prefab to instantiate for it");
            }
        }
    }
}
