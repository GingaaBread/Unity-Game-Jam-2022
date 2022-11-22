using System.Collections;
using System.Collections.Generic;
using TimeManagement;
using UnityEngine;
using UIManagement;
using PlayerData;

public class GameSetupStep : ComputerPhaseStep
{
    [SerializeField] [Range(0, 9999)] private int StartingMoney;

    public override void StartProcessingForComputerPhase(bool isComputerPhaseDuringGameInit) {

        if (!isComputerPhaseDuringGameInit) {
            OnFinishProcessing.Invoke();
            return; // do nothing
        }

        // give money, and enqueue money reception
        PlayerDataManager.Instance.IncreaseMoneyAmount(StartingMoney);
        FeedbackPanelManager.Instance.EnqueueMoneyReception(StartingMoney, false);

        // TODO: give building, and enqueue building reception
        // TODO: get quests

        // get cards
        CardManager.Instance.GiveCard(5);

        // foreach(Tile A in CropHarvest) {
            // ALT call method in Tile to do stuff 
          // PlaySound (EFX); <- there's probably code. 
          // Animation (here)  
        //}

        OnFinishProcessing.Invoke();
    }

    protected override object[] CheckForMissingReferences() {
        return new object[0];
    }

}
