using UnityEngine;
using UnityEngine.Assertions;

public class GameWonPanel : MonoBehaviour {

    [SerializeField] private GameObject ObjToShow;

    public static GameWonPanel Instance { get; private set; }

    private void Awake() {
        Assert.IsNull(Instance);
        Instance = this;
    }

    public void Show() {
        ObjToShow.SetActive(true);
    }



}
