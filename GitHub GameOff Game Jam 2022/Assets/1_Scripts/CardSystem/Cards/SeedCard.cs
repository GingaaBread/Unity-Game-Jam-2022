using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BuildingManagement;

[CreateAssetMenu(fileName = "NewBuildingCard", menuName = "Cards/SeedCard")]
public class SeedCard : ActionCardSO
{
    private CardPlayManager cardPlayManager;
    public SeedType buildingType;
    public GameObject tileForegroundChildPrefab;

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
