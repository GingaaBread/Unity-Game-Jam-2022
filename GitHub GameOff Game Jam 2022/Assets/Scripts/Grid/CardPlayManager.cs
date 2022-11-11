using System.Collections;
using System.Collections.Generic;
using UnityEngine;

     /// <author> Ro <author>
    ///<summary> Keeps track of what card was clicked and what tile was clicked
      ///  in order to perform the necessary build/plant/livestock action 
        /// </summary>
public class CardPlayManager : MonoBehaviour
{
   
    public BuildingCard currBuildingBeingPlayed;
    public SeedCard currSeedBeingPlayed;
    public LivestockCard currAnimalBeingPlayed;
    
    //TODO: Add seperate methods for building and planting/livestock
    public void AddCurCard(BuildingCard card){
        if(card==null) return;
        currBuildingBeingPlayed = card;
    }

    public void AddCurCard(SeedCard card){
        if(card==null) return;
        currSeedBeingPlayed = card;
    }
    public void AddCurCard(LivestockCard card){
        if(card==null) return;
        currAnimalBeingPlayed = card;
    }
    public void AddPlayToTile(Tile curr){
        if(currBuildingBeingPlayed != null){
            curr.ApplyBuildTile(currBuildingBeingPlayed);
            currBuildingBeingPlayed = null;
        } else if(currSeedBeingPlayed != null) {
           curr.ApplyCropTile(currSeedBeingPlayed);
           currSeedBeingPlayed = null;
        } else if(currAnimalBeingPlayed != null) {
            curr.ApplyLivestockTile(currAnimalBeingPlayed);
            currAnimalBeingPlayed = null;
        } else {
             return; // TODO:: can have some sort of UI message here since it means a card wasn't selected
        }
     
    }
}
