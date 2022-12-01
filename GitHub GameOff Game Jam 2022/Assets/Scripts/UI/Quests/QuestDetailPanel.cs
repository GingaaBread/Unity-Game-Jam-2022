using UnityEngine;
using UnityEngine.Assertions;

public class QuestDetailPanel : MonoBehaviour
{
    public static QuestDetailPanel Instance { get; private set; }

    [Header("UI Components")]
    [SerializeField] private GameObject root;
    [SerializeField] private CompletedQuestPanel[] completedQuestPanels;
    [SerializeField] private UncompletedQuestPanel[] uncompletedQuestPanels;

    public void Close() => root.SetActive(false);

    public void Open()
    {
        root.SetActive(true);
        Display();
    }

    private void Display()
    {
        DisplayPanel(0);
        DisplayPanel(1);
        DisplayPanel(2);
    }

    private void DisplayPanel(int index)
    {
        var quests = QuestManager.Instance.GetActiveQuests();
        var quest = quests[index];

        if (quest.IsDone())
        {
            completedQuestPanels[index].gameObject.SetActive(true);
            completedQuestPanels[index].Display(quest.questName, quest.finalReward.ToString());

            uncompletedQuestPanels[index].gameObject.SetActive(false);
        }
        else
        {
            completedQuestPanels[index].gameObject.SetActive(false);

            uncompletedQuestPanels[index].gameObject.SetActive(true);
            uncompletedQuestPanels[index].Display(quest);
        }
    }

    private void Awake()
    {
        Assert.IsNull(Instance, "There already is a quest detail panel instance.");
        Instance = this;
    }

}
