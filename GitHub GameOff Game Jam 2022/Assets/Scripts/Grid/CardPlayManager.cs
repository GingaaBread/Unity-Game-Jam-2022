using System.Collections;
using System.Collections.Generic;
using UnityEngine;

     /// <author> Ro <author>
    ///<summary> Keeps track of what card was clicked and what tile was clicked
      ///  in order to perform the necessary build/plant/livestock action 
        /// </summary>
public class CardPlayManager : MonoBehaviour
{
   
    [HideInInspector] public BuildingCard currBuildingBeingPlayed;
    [HideInInspector] public SeedCard currSeedBeingPlayed;
    [HideInInspector] public LivestockCard currAnimalBeingPlayed;

    private UICardPanel currentUIPanel;
    
    //TODO: Add seperate methods for building and planting/livestock
    public void AddCurCard(BuildingCard card, UICardPanel uiPanel)
    {
        if(card==null) return;
        currBuildingBeingPlayed = card;
        currentUIPanel = uiPanel;
    }

    public void AddCurCard(SeedCard card, UICardPanel uiPanel)
    {
        if(card==null) return;
        currSeedBeingPlayed = card;
        currentUIPanel = uiPanel;
    }
    public void AddCurCard(LivestockCard card, UICardPanel uiPanel)
    {
        if(card==null) return;
        currAnimalBeingPlayed = card;
        currentUIPanel = uiPanel;
    }
    
    private void RemoveCardAndUpdateQuests()
    {
        CardManager.Instance.RemoveCardOnUse(currentUIPanel);
        QuestManager.Instance.NotifyOfTilePlaced(currentUIPanel.CardToDisplay);
    }

    public void AddPlayToTile(Tile curr)
    {
        if (currBuildingBeingPlayed != null)
        {
            bool success = curr.ApplyBuildTile(currBuildingBeingPlayed);

            if (success)
            {
                RemoveCardAndUpdateQuests();
            }

            currBuildingBeingPlayed = null;
            currentUIPanel = null;
        }
        else if (currSeedBeingPlayed != null)
        {
            bool success = curr.ApplyCropTile(currSeedBeingPlayed);

            if (success)
            {
                RemoveCardAndUpdateQuests();
            }

            currSeedBeingPlayed = null;
            currentUIPanel = null;
        }
        else if (currAnimalBeingPlayed != null)
        {
            bool success = curr.ApplyLivestockTile(currAnimalBeingPlayed);

            if (success)
            {
                RemoveCardAndUpdateQuests();
            }

            currAnimalBeingPlayed = null;
            currentUIPanel = null;
        }
        else
        {
            return; // TODO:: can have some sort of UI message here since it means a card wasn't selected
        }

    }
}
