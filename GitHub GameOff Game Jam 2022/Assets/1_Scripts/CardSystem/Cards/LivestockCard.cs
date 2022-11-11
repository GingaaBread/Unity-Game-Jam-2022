using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BuildingManagement;

[CreateAssetMenu(fileName = "NewLivestockCard", menuName = "Cards/LivestockCard")]
public class LivestockCard : ActionCardSO
{
    private CardPlayManager cardPlayManager;
    public AnimalType animalType;
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
