using System;
using PlayerData;
using UIManagement;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New Quest", menuName = "Quests/BaseQuest")]
public class BaseQuest : ScriptableObject
{
    public string questName;
    public AbstractQuestSO[] questGoals;
    public AbstractQuestSO finalGoal;
    public QuestReward finalReward;

    protected int currentlyActiveGoal; // keeps track of the current goal (!) NEEDS TO BE EXPLICITLY SET TO 0
    
    public AbstractQuestSO[] GetGoals() => questGoals;

    public bool IsDone() => currentlyActiveGoal > questGoals.Length;

    public int GetCurrentStep() => currentlyActiveGoal + 1;

    public override string ToString() => $"{questName}, at: {GetQuestStepProgress()} with index set to {currentlyActiveGoal}";

    public void AddUpdateListeners(UnityAction call)
    {
        foreach (var questGoal in questGoals)
        {
            questGoal.OnUpdate.AddListener(call);
        }

        finalGoal.OnUpdate.AddListener(call);
    }

    public string GetQuestStepProgress() => $"({currentlyActiveGoal + 1}/{questGoals.Length + 1})";

    public UnityEvent CurrentUpdateSubscription()
    {
        if (currentlyActiveGoal >= questGoals.Length) return finalGoal.OnUpdate;
        else return questGoals[currentlyActiveGoal].OnUpdate;
    }

    public UnityEvent CurrentCompletionSubscription()
    {
        if (currentlyActiveGoal >= questGoals.Length) return finalGoal.OnCompletion;
        else return questGoals[currentlyActiveGoal].OnCompletion;
    }

    public void UnsubscribeFromUpdate(UnityAction call)
    {
        if (currentlyActiveGoal > 0 && currentlyActiveGoal < 5)
            questGoals[currentlyActiveGoal - 1].OnUpdate.RemoveListener(call);
    }

    /// <summary>
    /// Resets the actual counters of the quest goals and the final goal
    /// </summary>
    public void ResetActualsCounters()
    {
        currentlyActiveGoal = 0;

        foreach (var questGoal in questGoals)
            questGoal.ResetActualsCounters();

        finalGoal.ResetActualsCounters();

        foreach (var questGoal in questGoals)
        {
            questGoal.OnCompletion.AddListener(OnCompletion);
        }

        finalGoal.OnCompletion.AddListener(OnCompletion);
    }

    private void OnCompletion()
    {
        FeedbackPanelManager.Instance.EnqueueGenericMessage(true, $"Quest step completed!", FeedbackPanelManager.Instance.questCompletedSound);
        FeedbackPanelManager.Instance.InitiateInstantDisplayQueue();
        currentlyActiveGoal++;
        QuestPanel.Instance.DisplayCompletionAnimation();
    }

    public void NotifyOfResourceCollected(ResourceSO resource, int countCollected)
    {
        if (!IsDone())
        {
            if (currentlyActiveGoal >= questGoals.Length) finalGoal.NotifyOfResourceCollected(resource, countCollected);
            else questGoals[currentlyActiveGoal].NotifyOfResourceCollected(resource, countCollected);
        }
    }

    public void NotifyOfTilePlaced(ActionCardSO card)
    {
        if (!IsDone())
        {
            if (currentlyActiveGoal >= questGoals.Length) finalGoal.NotifyOfTilePlaced(card);
            else questGoals[currentlyActiveGoal].NotifyOfTilePlaced(card);
        }
    }

    public void NotifyOfResourceSale(ResourceSO resource, int moneyEarnedFromSale)
    {
        Debug.Log("Notifying... " + IsDone());
        if (!IsDone())
        {
            if (currentlyActiveGoal >= questGoals.Length) finalGoal.NotifyOfResourceSale(resource, moneyEarnedFromSale);
            else questGoals[currentlyActiveGoal].NotifyOfResourceSale(resource, moneyEarnedFromSale);
        }
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
