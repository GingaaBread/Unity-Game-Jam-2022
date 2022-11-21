using BuildingManagement;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBuildingCard", menuName = "Cards/BuildingCard")]
public class BuildingCard : ActionCardSO
{
    private CardPlayManager cardPlayManager;
    public BuildingType buildingType;
    public Sprite buildingSprite;
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
