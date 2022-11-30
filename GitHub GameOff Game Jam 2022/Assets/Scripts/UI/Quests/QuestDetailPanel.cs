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

    public void Open() => root.SetActive(true);

    private void Awake()
    {
        Assert.IsNull(Instance, "There already is a quest detail panel instance.");
        Instance = this;
    }

}
