using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Assertions;

public class QuestPanelItem : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI questTitle;
    [SerializeField] private TextMeshProUGUI questText;
    [SerializeField] private Slider percentageCompletedSlider;
    [SerializeField] private TextMeshProUGUI percentageCompletedText;

    private BaseQuest quest;

    private void Awake() {
        Assert.IsNotNull(questTitle);
        Assert.IsNotNull(questText);
        Assert.IsNotNull(percentageCompletedSlider);
        Assert.IsNotNull(percentageCompletedText);
    }

    public void Initialize(BaseQuest quest) {
        this.quest = quest;
        quest.CurrentUpdateSubscription().AddListener(UpdateUI);
        UpdateUI();
    }

    public void UpdateUI() {
        print("Updating UI!");
        questTitle.text = quest.questName +  " " + quest.GetQuestStepProgress();
        questText.text = quest.GetCurrentPrompt() + " (currently " + quest.GetStatusAsSentence() + ")";
        // progress bar
        int floorOfPercentageCompleted = Mathf.FloorToInt(quest.GetPercentageCompleted());
        percentageCompletedSlider.minValue = 0;
        percentageCompletedSlider.maxValue = 100;
        percentageCompletedSlider.value    = floorOfPercentageCompleted;
        percentageCompletedText.text = floorOfPercentageCompleted + "%";
    }

}
