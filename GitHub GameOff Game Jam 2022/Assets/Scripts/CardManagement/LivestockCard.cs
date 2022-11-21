using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BuildingManagement;
using PlayerData;

[CreateAssetMenu(fileName = "NewLivestockCard", menuName = "Cards/LivestockCard")]
public class LivestockCard : ActionCardSO
{
    private CardPlayManager cardPlayManager;
    public AnimalType animalType;
    public GameObject tileForegroundChildPrefab;

    public Sprite[] hill_LiveStockSprite = new Sprite[4];
    public Sprite[] plains_LiveStockSprite = new Sprite[4];

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
