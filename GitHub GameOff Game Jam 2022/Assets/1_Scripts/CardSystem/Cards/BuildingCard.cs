using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BuildingManagement;

[CreateAssetMenu(fileName = "NewBuildingCard", menuName = "Cards/BuildingCard")]
public class BuildingCard : ActionCardSO
{
    private CardPlayManager cardPlayManager;
    public BuildingType buildingType;
    public override void Action()
    {
        cardPlayManager = FindObjectOfType<CardPlayManager>();
        if(cardPlayManager == null){
            Debug.LogError("CardPlayManager script missing!");
            return;
        }
        cardPlayManager.AddCurCard(this);
        // build action here
    }
}
