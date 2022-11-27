using TimeManagement;
using UnityEngine;
using UnityEngine.UI;

public class UIEndTurnButton : ComputerPhaseStep
{
    private Button endOfTurnButton;
    private Animator endOfTurnButtonAnimator;

    // Start is called before the first frame update
    void Start()
    {
        endOfTurnButton = GetComponent<Button>();
        endOfTurnButtonAnimator = GetComponent<Animator>();
        Lock();
    }

    public void Unlock()
    {
        endOfTurnButton.interactable = true;
    }

    public void Lock()
    {
        endOfTurnButton.interactable = false;
        endOfTurnButtonAnimator.Play("LockEndTurnButton");
    }

    private void HandlePlayerTurnStart()
    {
        Unlock();
        endOfTurnButtonAnimator.Play("Idle");
    }

    public override void StartProcessingForComputerPhase(bool isComputerPhaseDuringGameInit)
    {
        if (isComputerPhaseDuringGameInit)
        {
            TimeManager.OnStartPlayerTurn.AddListener(HandlePlayerTurnStart);
            endOfTurnButton.interactable = false;
        }
    }

    protected override object[] CheckForMissingReferences()
    {
        return new object[0];
    }
}
