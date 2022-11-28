using TimeManagement;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

public class WillPanel : ComputerPhaseStep {

    [SerializeField] [TextArea(5, 20)] private string page3TextBeforeMissions;
    [SerializeField] [TextArea(5, 20)] private string page3TextAfterMissions;

    [Header("references to child objects")]
    [SerializeField] private GameObject ObjToShow;
    [SerializeField] private GameObject page1Obj;
    [SerializeField] private GameObject page2Obj;
    [SerializeField] private GameObject page3Obj;
    [SerializeField] private TextMeshProUGUI page3TextObj;

    public static WillPanel Instance { get; private set; }

    private new void Awake() {
        Assert.IsNull(Instance);
        Instance = this;

        Assert.IsNotNull(ObjToShow);
        Assert.IsNotNull(page1Obj);
        Assert.IsNotNull(page2Obj);
        Assert.IsNotNull(page3Obj);
        Assert.IsNotNull(page3TextObj);

        page1Obj.SetActive(true);
        page2Obj.SetActive(false);
        page3Obj.SetActive(false);
        ObjToShow.SetActive(false);
    }

    public override void StartProcessingForComputerPhase(bool isComputerPhaseDuringGameInit) {
        if (isComputerPhaseDuringGameInit) {

            page3TextObj.text = "";
            page3TextObj.text += page3TextBeforeMissions;

            string[] questStrings = QuestManager.Instance.GetQuestTextForWill();
            foreach(string questString in questStrings) {
                page3TextObj.text += " - " + questString + "\n";
            }

            page3TextObj.text += page3TextAfterMissions;

            ObjToShow.SetActive(true);
        } else {
            OnFinishProcessing.Invoke();
        }
    }

    public void OnPlayerAcceptsWill() {
        AmbienceTrigger.Instance.PlayFarmAmbience();
        ObjToShow.SetActive(false);
        OnFinishProcessing.Invoke();
    }

    protected override object[] CheckForMissingReferences() {
        return null;
    }
}
