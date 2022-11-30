using PlayerData;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New Quest", menuName = "Quests/BaseQuest")]
public class BaseQuest : ScriptableObject
{
    public string questName;
    public AbstractQuestSO[] questGoals;
    public AbstractQuestSO finalGoal;
    public string finalReward;

    private int currentlyActiveGoal; // keeps track of the current goal (!) NEEDS TO BE EXPLICITLY SET TO 0

    private void Awake()
    {
        currentlyActiveGoal = 0;
    }

    private void OnEnable()
    {
        foreach (var questGoal in questGoals)
        {
            questGoal.OnCompletion.AddListener(() =>
            {
                Debug.Log($"Quest '{questName} completed!");
                //questGoals[currentlyActiveGoal].OnUpdate.RemoveAllListeners(); do this later
                currentlyActiveGoal++;
                QuestPanel.Instance.DisplayCompletionAnimation();
            });
        }
    }

    public override string ToString() => $"{questName}, at: {GetQuestStepProgress()} with index set to {currentlyActiveGoal}";

    public string GetQuestStepProgress() => $"({currentlyActiveGoal + 1}/{questGoals.Length})";

    public UnityEvent CurrentUpdateSubscription()
    {
        if (currentlyActiveGoal >= questGoals.Length) return finalGoal.OnUpdate;
        else return questGoals[currentlyActiveGoal].OnUpdate;
    }

    /// <summary>
    /// Resets the actual counters of the quest goals and the final goal
    /// </summary>
    public void ResetActualsCounters()
    {
        foreach (var questGoal in questGoals)
            questGoal.ResetActualsCounters();

        finalGoal.ResetActualsCounters();
    }

    public void NotifyOfResourceCollected(ResourceSO resource, int countCollected)
    {
        if (currentlyActiveGoal >= questGoals.Length) finalGoal.NotifyOfResourceCollected(resource, countCollected);
        else questGoals[currentlyActiveGoal].NotifyOfResourceCollected(resource, countCollected);
    }

    public void NotifyOfTilePlaced(ActionCardSO card)
    {
        if (currentlyActiveGoal >= questGoals.Length) finalGoal.NotifyOfTilePlaced(card);
        else questGoals[currentlyActiveGoal].NotifyOfTilePlaced(card);
    }

    public void NotifyOfResourceSale(ResourceSO resource, int moneyEarnedFromSale)
    {
        if (currentlyActiveGoal >= questGoals.Length) finalGoal.NotifyOfResourceSale(resource, moneyEarnedFromSale);
        else questGoals[currentlyActiveGoal].NotifyOfResourceSale(resource, moneyEarnedFromSale);
    }

    public string GetStatusAsSentence()
    {
        if (currentlyActiveGoal >= questGoals.Length) return finalGoal.GetStatusAsSentence();
        else return questGoals[currentlyActiveGoal].GetStatusAsSentence();
    }

    public float GetPercentageCompleted()
    {
        if (currentlyActiveGoal >= questGoals.Length) return finalGoal.GetPercentageCompleted();
        else return questGoals[currentlyActiveGoal].GetPercentageCompleted();
    }
    
    public string GetWillPrompt() => finalGoal.textPrompt.Replace("<gold>", "<sprite=1>");

    public string GetCurrentPrompt()
    {
        if (currentlyActiveGoal >= questGoals.Length) return finalGoal.textPrompt;
        else return questGoals[currentlyActiveGoal].textPrompt;
    }
}
