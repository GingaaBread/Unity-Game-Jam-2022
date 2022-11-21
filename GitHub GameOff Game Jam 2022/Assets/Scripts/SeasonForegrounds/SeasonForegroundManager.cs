using TimeManagement;
using UnityEngine;
using UnityEngine.Assertions;

public class SeasonForegroundManager : ComputerPhaseStep
{
    [Header("Objects to show/hide")]
    public GameObject springObjects;
    public GameObject summerObjects;
    public GameObject winterObjects;
    public GameObject autumnObjects;

    private new void Awake() {
        Assert.IsNotNull(springObjects);
        Assert.IsNotNull(summerObjects);
        Assert.IsNotNull(autumnObjects);
        Assert.IsNotNull(winterObjects);
    }

    public override void StartProcessingForComputerPhase(bool isComputerPhaseDuringGameInit) {

        if (TimeManager.Instance.CurrentTime.RoundInSeason != 1) {
            this.OnFinishProcessing.Invoke();
            return;
        }

        HideAll();
        switch (TimeManager.Instance.CurrentTime.SeasonInYear) {
            case SeasonType.SUMMER: summerObjects.SetActive(true); break;
            case SeasonType.FALL:   autumnObjects.SetActive(true); break;
            case SeasonType.WINTER: winterObjects.SetActive(true); break;
            case SeasonType.SPRING: springObjects.SetActive(true); break;
            default: throw new System.Exception("unexpected season " + TimeManager.Instance.CurrentTime.SeasonInYear);
        }
        this.OnFinishProcessing.Invoke();
    }

    private void HideAll() {
        springObjects.SetActive(false);
        summerObjects.SetActive(false);
        autumnObjects.SetActive(false);
        winterObjects.SetActive(false);
    }

    protected override object[] CheckForMissingReferences() { return null; }

}
