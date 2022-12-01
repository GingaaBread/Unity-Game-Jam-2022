using PlayerData;
using System;
using UIManagement;
using UnityEngine;

[CreateAssetMenu(fileName = "New Reward", menuName = "Quests/Reward")]
public class QuestReward : ScriptableObject
{
    public int moneyReward;
    public ResourceSO resourceToAward;
    public int resourceAmountReward;

    public override string ToString()
    {
        if (moneyReward != 0 && resourceToAward != null)
            return $"<sprite=1>{moneyReward} and {resourceAmountReward}x {resourceToAward.name.ToLower()}";
        else if (moneyReward != 0)
            return $"<sprite=1>{moneyReward}";
        else if (resourceToAward != null)
            return $"{resourceAmountReward}x {resourceToAward.name.ToLower()}";
        else throw new ApplicationException($"Unlikely that quest reward {this} is setup correctly. No money reward nor resource award has been set up");
    }

    public void GiveOut()
    {
        if (moneyReward != 0)
        {
            PlayerDataManager.Instance.IncreaseMoneyAmount(moneyReward);
            FeedbackPanelManager.Instance.EnqueueMoneyReception(moneyReward, true);
            FeedbackPanelManager.Instance.InitiateInstantDisplayQueue();
        }

        if (resourceToAward != null)
        {
            PlayerDataManager.Instance.IncreaseInventoryItemAmount(resourceToAward, resourceAmountReward);
            FeedbackPanelManager.Instance.EnqueueGenericMessage(true, $"Quest reward: {resourceAmountReward}x {resourceToAward.name.ToLower()}!", 
                FeedbackPanelManager.Instance.itemReceiveEvent);
            FeedbackPanelManager.Instance.InitiateInstantDisplayQueue();
        }
    }
}
