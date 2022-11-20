using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TimeManagement;

public class TileAppearanceUpdater :  ComputerPhaseStep
{
    
    protected override object[] CheckForMissingReferences() => new object[] {};
    private void NotifyTileofTimeChange(){
        Tile[] allTiles = GetComponentsInChildren<Tile>();
        foreach(Tile t in allTiles){
            t.UpdateTileAppearance(TimeManager.Instance.CurrentTime);
        }
    }
    public override void StartProcessingForComputerPhase(bool isComputerPhaseDuringGameInit) {
            NotifyTileofTimeChange();
            OnFinishProcessing.Invoke();
    }
}
