using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManagerTest_InitializeButton : MonoBehaviour {

    public SOEvent_Void EventToFire;

    public void HandleClick() {
        TimeManager.OnInitGame.Invoke();
    }
}
