using TimeManagement;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class EndTurnPanel : MonoBehaviour
{
    public Button EndTurnButton;

    private void Awake() {
        Assert.IsNotNull(EndTurnButton);
        TimeManager.OnStartPlayerTurn.AddListener(HandlePlayerStartTurn);
    }

    private void Start() {
        UpdateButtonStateBasedOnPhase();
    }

    public void HandleEndTurnButtonClicked() {
        //TimeManager.Instance.FinishCurrentPhase();
        UpdateButtonStateBasedOnPhase();
    }

    public void HandlePlayerStartTurn() {
        UpdateButtonStateBasedOnPhase();
    }

    public void UpdateButtonStateBasedOnPhase() {
        if(TimeManager.Instance.CurrentPhase == TimeManager.Phase.PlayerTurn) {
            EndTurnButton.interactable = true;
        } else {
            EndTurnButton.interactable = false;
        }

    }

}
