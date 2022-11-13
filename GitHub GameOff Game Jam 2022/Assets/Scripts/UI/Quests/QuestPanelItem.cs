using UnityEngine;
using TMPro;

public class QuestPanelItem : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI questText;

    private ResourceCollectionQuestSO quest;

    public void Initialize(ResourceCollectionQuestSO quest) {
        this.quest = quest;
        quest.OnUpdate.AddListener(UpdateUI);
        UpdateUI();
    }

    public void UpdateUI() {
        questText.text = quest.GetQuestAsSentence() + " (currently " + quest.GetStatusAsSentence() + ")";
    }

}
