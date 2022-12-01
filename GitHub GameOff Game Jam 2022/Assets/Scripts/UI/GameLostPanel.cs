using UnityEngine;
using UnityEngine.Assertions;

public class GameLostPanel : MonoBehaviour {

    [SerializeField] private GameObject ObjToShow;

    public static GameLostPanel Instance { get; private set; }
    
    private void Awake() {
        Assert.IsNull(Instance);
        Instance = this;

        Assert.IsNotNull(ObjToShow);
        ObjToShow.SetActive(false);
    }

    public void Show() {
        ObjToShow.SetActive(true);
        AmbienceTrigger.Instance.TriggerGameOverSound(false);
    }

}
