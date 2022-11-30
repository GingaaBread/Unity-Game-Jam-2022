using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestPromptContainer : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private TMP_Text promptText;
    [SerializeField] private TMP_Text rewardText;
    [SerializeField] private GameObject overlay;

    public void ToggleOverlay(bool show) => overlay.SetActive(show);

    public void DisplayPrompt(string prompt, string reward)
    {
        promptText.text = "•  " + prompt;
        rewardText.text = "Reward: " + reward;
    }
}
