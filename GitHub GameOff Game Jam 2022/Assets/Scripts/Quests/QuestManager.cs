using FMODUnity;
using PlayerData;
using System;
using System.Collections.Generic;
using TimeManagement;
using UIManagement;
using UnityEngine;
using UnityEngine.Assertions;

/// <summary>
/// 
/// </summary>
/// <author>Ben</author>
public class QuestManager : ComputerPhaseStep
{

    public static QuestManager Instance { get; private set; }

    [Header("Prefabs")]
    public GameObject questUIPrefab;

    [Header("Debugging")]
    [SerializeField] private bool DebugMode_AutoInitOnStart;
    [SerializeField] private BaseQuest[] DebugMode_QuestsToUse;

    [Header("Pool Of ResourceCollection Quests To Choose From")]
    [SerializeField] private BaseQuest[] ResourceCollectionQuests;
    [SerializeField] private BaseQuest[] TilePlacementQuests;
    [SerializeField] private BaseQuest[] MoneyForActionQuests;

    private List<BaseQuest> _activeQuests;

    [Header("Quest Selection Logic")]
    [SerializeField] [Range(0, 3)] private int ResourceCollectionQuestsToSelect;
    [SerializeField] [Range(0, 3)] private int TilePlacementQuestsToSelect;
    [SerializeField] [Range(0, 3)] private int MoneyFromActionQuestsToSelect;

    [Header("Win condition logic")]
    [SerializeField] [Range(1, 3)] private int WinRequiresNMissionsComplete;

    [Header("Loss condition logic")]
    [SerializeField] [Range(1, 20)] private int LoseWhenWeReachYearN;
    [SerializeField] private EventReference OneYearLeftFMODEventReference;

    private new void Awake() {
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
        _activeQuests = new List<BaseQuest>();

        if (!DebugMode_AutoInitOnStart) {
            _activeQuests.AddRange(RandomlyChooseQuests(ResourceCollectionQuests, ResourceCollectionQuestsToSelect));
            _activeQuests.AddRange(RandomlyChooseQuests(TilePlacementQuests, TilePlacementQuestsToSelect));
            _activeQuests.AddRange(RandomlyChooseQuests(MoneyForActionQuests, MoneyFromActionQuestsToSelect));
        } else {
            _activeQuests.AddRange(DebugMode_QuestsToUse);
        }

        foreach (BaseQuest quest in _activeQuests)
        {
            quest.ResetActualsCounters();
            print(quest);
        }

        QuestPanel.Instance.UpdateQuests(_activeQuests);
    }

    private static List<BaseQuest> RandomlyChooseQuests(BaseQuest[] questsPool, int count) {
        Assert.IsTrue(questsPool.Length >= count);

        List<BaseQuest> tempList = new List<BaseQuest>(questsPool);
        List<BaseQuest> chosenQuests = new List<BaseQuest>();

        for (int i = 0; i < count; i++) {
            BaseQuest quest = tempList[UnityEngine.Random.Range(0, tempList.Count)];
            chosenQuests.Add(quest);
            tempList.Remove(quest);
        }

        return chosenQuests;
    }

    public void NotifyOfResourceCollected(ResourceSO resource, int countCollected) {
        foreach (BaseQuest quest in _activeQuests) {
            quest.NotifyOfResourceCollected(resource, countCollected);
        }
        if (IsGameWon())
            ShowWinScreen();
    }

    public void NotifyOfTilePlaced(ActionCardSO card) {
        foreach (BaseQuest quest in _activeQuests) {
            quest.NotifyOfTilePlaced(card);
        }
        if (IsGameWon())
            ShowWinScreen();
    }

    public void NotifyOfResourceSale(ResourceSO resource, int MoneyEarnedFromSale) {
        foreach (BaseQuest quest in _activeQuests) {
            quest.NotifyOfResourceSale(resource, MoneyEarnedFromSale);
        }
        if (IsGameWon())
            ShowWinScreen();
    }

    private bool IsGameWon() {
        int questsWon = 0;

        foreach (BaseQuest quest in _activeQuests) {
            if (quest.GetPercentageCompleted() == 100)
                questsWon += 1;
        }
        
        if(questsWon >= WinRequiresNMissionsComplete) 
            return true;
        else 
            return false;
    }

    private bool IsGameLost() {
        if (TimeManager.Instance.CurrentTime.Year >= LoseWhenWeReachYearN)
            return true;
        else
            return false;
    }

    public string[] GetQuestTextForWill() {
        string[] questStrings = new string[_activeQuests.Count];

        for(int i=0; i<_activeQuests.Count; i++) {
            questStrings[i] = _activeQuests[i].GetWillPrompt();
        }

        return questStrings;
    }

    private void ShowWinScreen() {
        GameWonPanel.Instance.Show();
    }

    private void ShowLossScreen() {
        GameLostPanel.Instance.Show();
    }

    public override void StartProcessingForComputerPhase(bool isComputerPhaseDuringGameInit) {
        if (isComputerPhaseDuringGameInit) {
            OnInitializeGame();
            OnFinishProcessing.Invoke();
        } else {
            if (IsGameLost()) {
                ShowLossScreen();
            } else {

                if (IsStartOfFinalYear()) {
                    FeedbackPanelManager.Instance.EnqueueGenericMessage(false, $"Only 1 year left!", OneYearLeftFMODEventReference);
                }
                OnFinishProcessing.Invoke();
            }
        }
    }

    private bool IsStartOfFinalYear() {
        PointInTime time = TimeManager.Instance.CurrentTime;
        if (time.Year == LoseWhenWeReachYearN-1 && time.IsFirstRoundOfSeason() && time.IsFirstSeasonOfYear()) 
            return true;
        else 
            return false;
    }

    protected override object[] CheckForMissingReferences() {
        return null;
    }
}
