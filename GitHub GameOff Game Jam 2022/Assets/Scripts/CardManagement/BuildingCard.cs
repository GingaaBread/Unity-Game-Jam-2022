using BuildingManagement;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBuildingCard", menuName = "Cards/BuildingCard")]
public class BuildingCard : ActionCardSO
{
    private CardPlayManager cardPlayManager;
    public BuildingType buildingType;
    public Sprite[] hill_BuildingSprite = new Sprite[4]; // 0 = Spring, 1 = Summer, 2 = Autumn, 3 = Winter.
    public Sprite[] plain_BuildingSprite = new Sprite[4]; // 0 = Spring, 1 = Summer, 2 = Autumn, 3 = Winter.
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
