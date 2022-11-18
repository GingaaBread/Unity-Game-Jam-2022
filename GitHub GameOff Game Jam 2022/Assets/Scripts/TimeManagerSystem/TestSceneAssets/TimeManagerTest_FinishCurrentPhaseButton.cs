using TimeManagement;
using UnityEngine;

public class TimeManagerTest_FinishCurrentPhaseButton : MonoBehaviour {

    public void HandleClick()
    {
        TimeManager.Instance.FinishCurrentPhase();
    }

}
