using UnityEngine;

public class WillSigniature : MonoBehaviour {

    public void OnSigniatureAnimationComplete() {
        WillPanel.Instance.OnPlayerAcceptsWill();
    }

}
