using TMPro;
using UnityEngine;

public class UncompletedQuestPanel : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private TMP_Text questTitleText;
    [SerializeField] private TMP_Text questProgressText;
    [SerializeField] private TMP_Text questCompletionRewardText;
    [SerializeField] private QuestPromptContainer[] questPromptContainers;

    public void Display(string questTitle, string quest, int currentStep, string completionReward)
    {
        questTitleText.text = questTitle;
        questProgressText.text = $"Step {currentStep} of 4";
        questCompletionRewardText.text = "Reward: <i>" + completionReward + "</i>";
    }
}
