using TMPro;
using UnityEngine;

public class UncompletedQuestPanel : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private TMP_Text questTitleText;
    [SerializeField] private TMP_Text questProgressText;
    [SerializeField] private TMP_Text questCompletionRewardText;
    [SerializeField] private QuestPromptContainer[] questPromptContainers;

    public void Display(BaseQuest quest)
    {
        questTitleText.text = quest.questName;
        questProgressText.text = $"Step {quest.GetCurrentStep()} of 5";
        questCompletionRewardText.text = "Reward: <i>" + quest.finalReward + "</i>";

        var goals = quest.GetGoals();
        for (int i = 0; i < questPromptContainers.Length; i++)
        {
            int reward = -1;
            if (i == 0) reward = 5;
            else if (i == 1) reward = 10;
            else if (i == 2) reward = 20;
            else if (i == 3) reward = 30;
            else if (i == 4) reward = 50;

            if (quest.GetCurrentStep() - 1 > i)
            {
                questPromptContainers[i].ToggleOverlay(true);
            }
            else
            {
                questPromptContainers[i].ToggleOverlay(false);
                questPromptContainers[i].DisplayPrompt(goals[i].textPrompt.Replace("<gold>", "<sprite=1>"), $"<sprite=1>{reward}");
            }
        }
    }
}
