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
    public GameObject tileForegroundChildPrefab;
    public float cropTotalTurnsTillPayoff = 2f;
 
    [Header("Resource Payoff")]
    public ResourceSO payoffResource;
    public int payoffAmount;

    [Header("Card Bonus Effect")]
    public CardBonus bonus;

    public override void Action(UICardPanel uiPanel)
    {
        cardPlayManager = FindObjectOfType<CardPlayManager>();
        if(cardPlayManager == null){
            Debug.LogError("CardPlayManager script missing!");
            return;
        }
        cardPlayManager.AddCurCard(this, uiPanel);
        // build action here
    }
}
