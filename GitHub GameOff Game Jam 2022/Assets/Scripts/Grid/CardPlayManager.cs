using System;
using UnityEngine;

/// <author> Ro <author>
///<summary> Keeps track of what card was clicked and what tile was clicked
///  in order to perform the necessary build/plant/livestock action 
/// </summary>
public class CardPlayManager : MonoBehaviour
{
    // The singleton
    private static CardPlayManager _instance = null;
    public static CardPlayManager Instance
    {
        get
        {
            if (_instance == null)
                throw new Exception("CardPlayManager singleton was called without CardPlayManager being set up (check that CardPlayManager is in the scene)");
            return _instance;
        }
        private set { _instance = value; }
    }

    [HideInInspector] public BuildingCard currBuildingBeingPlayed;
    [HideInInspector] public SeedCard currSeedBeingPlayed;
    [HideInInspector] public LivestockCard currAnimalBeingPlayed;

    private UICardPanel currentUIPanel;

    private void Awake()
    {
        Instance = this;
    }

    public void ResetCurrentPlay()
    {
        currAnimalBeingPlayed = null;
        currSeedBeingPlayed = null;
        currAnimalBeingPlayed = null;
        currentUIPanel = null;

        UIMainPanel.Instance.HideDetailedCard();
    }

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

    public bool PlayIsInProgress() => currentUIPanel != null;

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

            ResetCurrentPlay();
        }
        else if (currSeedBeingPlayed != null)
        {
            bool success = curr.ApplyCropTile(currSeedBeingPlayed);

            if (success)
            {
                RemoveCardAndUpdateQuests();
            }

            ResetCurrentPlay();
        }
        else if (currAnimalBeingPlayed != null)
        {
            bool success = curr.ApplyLivestockTile(currAnimalBeingPlayed);

            if (success)
            {
                RemoveCardAndUpdateQuests();
            }

            ResetCurrentPlay();
        }
        else
        {
            return; // TODO:: can have some sort of UI message here since it means a card wasn't selected
        }

    }
}
