using PlayerData;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

/// <summary>
/// 
/// </summary>
/// <author>Ben</author>
public class QuestManager : MonoBehaviour
{

    public static QuestManager Instance { get; private set; }

    [Header("Debugging")]
    [SerializeField] private bool DebugMode_AutoInitOnStart;
    [SerializeField] private AbstractQuestSO[] DebugMode_QuestsToUse;

    [Header("Pool Of ResourceCollection Quests To Choose From")]
    [SerializeField] private ResourceCollectionQuestSO[] ResourceCollectionQuests;
    [SerializeField] private TilePlacementQuestSO[]      TilePlacementQuests;
    [SerializeField] private MoneyFromSalesQuestSO[]     MoneyForActionQuests;
    private List<AbstractQuestSO> _activeQuests;

    [Header("Quest Selection Logic")]
    [SerializeField] [Range(0, 3)] private int ResourceCollectionQuestsToSelect;
    [SerializeField] [Range(0, 3)] private int TilePlacementQuestsToSelect;
    [SerializeField] [Range(0, 3)] private int MoneyFromActionQuestsToSelect;

    private void Awake() {
        Assert.IsNull(Instance);
        Instance = this;

        Assert.IsNotNull(ResourceCollectionQuests);
        Assert.IsNotNull(TilePlacementQuests);
        Assert.IsNotNull(TilePlacementQuests);
        Assert.IsTrue(ResourceCollectionQuests.Length >= ResourceCollectionQuestsToSelect, $"need pool of at least {ResourceCollectionQuestsToSelect} RC quests so we can choose {ResourceCollectionQuestsToSelect}");
        Assert.IsTrue(TilePlacementQuests.Length      >= TilePlacementQuestsToSelect, $"need pool of at least {TilePlacementQuestsToSelect} RC quests so we can choose {TilePlacementQuestsToSelect}");
        Assert.IsTrue(MoneyForActionQuests.Length     >= MoneyFromActionQuestsToSelect, $"need pool of at least {MoneyFromActionQuestsToSelect} RC quests so we can choose {MoneyFromActionQuestsToSelect}");
    }

    private void Start() {
        Assert.IsNotNull(QuestPanel.Instance, "QuestManager expects QuestPanel to also exist in the scene");
        Assert.IsNotNull(GameWonPanel.Instance, "QuestManager expects GameWonPanel to also exist in the scene");

        if (DebugMode_AutoInitOnStart) {
            OnInitializeGame();
        }
    }

    private void OnInitializeGame() {
        _activeQuests = new List<AbstractQuestSO>();

        if (!DebugMode_AutoInitOnStart) {
            _activeQuests.AddRange(RandomlyChooseQuests(ResourceCollectionQuests, ResourceCollectionQuestsToSelect));
            _activeQuests.AddRange(RandomlyChooseQuests(TilePlacementQuests, TilePlacementQuestsToSelect));
            _activeQuests.AddRange(RandomlyChooseQuests(MoneyForActionQuests, MoneyFromActionQuestsToSelect));
        } else {
            _activeQuests.AddRange(DebugMode_QuestsToUse);
        }

        foreach (AbstractQuestSO quest in _activeQuests)
            quest.ResetActualsCounters();
        QuestPanel.Instance.UpdateQuests(_activeQuests);
    }

    private static List<AbstractQuestSO> RandomlyChooseQuests(AbstractQuestSO[] questsPool, int count) {
        Assert.IsTrue(questsPool.Length >= count);

        List<AbstractQuestSO> tempList = new List<AbstractQuestSO>(questsPool);
        List<AbstractQuestSO> chosenQuests = new List<AbstractQuestSO>();

        for (int i = 0; i < count; i++) {
            AbstractQuestSO quest = tempList[UnityEngine.Random.Range(0, tempList.Count)];
            chosenQuests.Add(quest);
            tempList.Remove(quest);
        }

        return chosenQuests;
    }

    public void NotifyOfResourceCollected(ResourceSO resource, int countCollected) {
        foreach (AbstractQuestSO quest in _activeQuests) {
            quest.NotifyOfResourceCollected(resource, countCollected);
        }
        CheckIfGameIsWonAndReactAccordingly();
    }

    public void NotifyOfTilePlaced(ActionCardSO card) {
        foreach (AbstractQuestSO quest in _activeQuests) {
            quest.NotifyOfTilePlaced(card);
        }
        CheckIfGameIsWonAndReactAccordingly();
    }

    public void NotifyOfResourceSale(ResourceSO resource, int MoneyEarnedFromSale) {
        foreach (AbstractQuestSO quest in _activeQuests) {
            quest.NotifyOfResourceSale(resource, MoneyEarnedFromSale);
        }
        CheckIfGameIsWonAndReactAccordingly();
    }

    public void CheckIfGameIsWonAndReactAccordingly() {

        foreach (AbstractQuestSO quest in _activeQuests) {
            if (quest.GetPercentageCompleted() < 100)
                return;
        }

        GameWonPanel.Instance.Show();
    }

}
