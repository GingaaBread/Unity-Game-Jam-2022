using TimeManagement;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

public class WillPanel : ComputerPhaseStep {

    [Header("Inputs from Game Designers")]
    [SerializeField] [TextArea(10, 20)] private string page1Text;
    [SerializeField] [TextArea(10, 20)] private string page2Text;
    [SerializeField] [TextArea(10, 20)] private string page3Text;

    [Header("references to child objects")]
    [SerializeField] private GameObject ObjToShow;
    [SerializeField] private GameObject page1Obj;
    [SerializeField] private GameObject page2Obj;
    [SerializeField] private GameObject page3Obj;
    [SerializeField] private TextMeshProUGUI page1TextObj;
    [SerializeField] private TextMeshProUGUI page2TextObj;
    [SerializeField] private TextMeshProUGUI page3TextObj;
    public static WillPanel Instance { get; private set; }

    private new void Awake() {
        Assert.IsNull(Instance);
        Instance = this;

        Assert.IsNotNull(ObjToShow);
        Assert.IsNotNull(page1Obj);
        Assert.IsNotNull(page2Obj);
        Assert.IsNotNull(page3Obj);
        Assert.IsNotNull(page1TextObj);
        Assert.IsNotNull(page2TextObj);
        Assert.IsNotNull(page3TextObj);

        page1TextObj.text = page1Text;
        page2TextObj.text = page2Text;
        page3TextObj.text = page3Text;
        page1Obj.SetActive(true);
        page2Obj.SetActive(false);
        page3Obj.SetActive(false);
        ObjToShow.SetActive(false);
    }

    public override void StartProcessingForComputerPhase(bool isComputerPhaseDuringGameInit) {
        if (isComputerPhaseDuringGameInit) {
            ObjToShow.SetActive(true);
        } else {
            OnFinishProcessing.Invoke();
        }
    }

    public void OnPlayerAcceptsWill() {
        ObjToShow.SetActive(false);
        OnFinishProcessing.Invoke();
    }

    protected override object[] CheckForMissingReferences() {
        return null;
    }
}
