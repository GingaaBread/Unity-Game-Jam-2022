using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Assertions;

public class QuestPanelItem : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI questText;
    [SerializeField] private Slider percentageCompletedSlider;
    [SerializeField] private TextMeshProUGUI percentageCompletedText;

    private ResourceCollectionQuestSO quest;

    private void Awake() {
        Assert.IsNotNull(questText);
        Assert.IsNotNull(percentageCompletedSlider);
        Assert.IsNotNull(percentageCompletedText);
    }

    public void Initialize(ResourceCollectionQuestSO quest) {
        this.quest = quest;
        quest.OnUpdate.AddListener(UpdateUI);
        UpdateUI();
    }

    public void UpdateUI() {
        int floorOfPercentageCompleted = Mathf.FloorToInt(quest.GetPercentageCompleted());
        questText.text = quest.GetQuestAsSentence() + " (currently " + quest.GetStatusAsSentence() + ")";
        percentageCompletedSlider.minValue = 0;
        percentageCompletedSlider.maxValue = 100;
        percentageCompletedSlider.value    = floorOfPercentageCompleted;
        percentageCompletedText.text = floorOfPercentageCompleted + "%";
    }

}
