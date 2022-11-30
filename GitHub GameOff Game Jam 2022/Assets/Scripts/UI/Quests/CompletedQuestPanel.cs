using TMPro;
using UnityEngine;

public class CompletedQuestPanel : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private TMP_Text questTitleText;
    [SerializeField] private TMP_Text questCompletionRewardText;

    public void Display(string questTitle, string questCompletionReward)
    {
        questTitleText.text = questTitle;
        questCompletionRewardText.text = questCompletionReward;
    }
}
