using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BuildingManagement;
using PlayerData;

[CreateAssetMenu(fileName = "NewBuildingCard", menuName = "Cards/SeedCard")]
public class SeedCard : ActionCardSO
{
    private CardPlayManager cardPlayManager;
    public SeedType buildingType;
    public Sprite[] hill_SeedSprite = new Sprite[4];
    public Sprite[] plains_SeedSprite = new Sprite[4];
    public GameObject tileForegroundChildPrefab;
    public int cropTotalTurnsTillPayoff = 1;

    [Header("Resource Payoff")]
    public ResourceSO payoffResource;
    public int payoffAmount;

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
