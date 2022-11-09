using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{

    [SerializeField]
    public TileTypeSO currType;
    private TileTypeSO prevType; // <- store tile type when the tile is changed 
    private SpriteRenderer currSprite;

    private bool isBuild = false;
    private BuildingCard currBuilding;
    private SeedCard currSeed;
    private bool isSeed = false;
    
    private CardPlayManager cardPlayManager;



    void Awake()
    {
        if (currType != null && currType.picture != null)
        {
            GetComponent<SpriteRenderer>().sprite = currType.picture;
        }
        else
        {
            Debug.LogError("Please put in a Tile Type Scriptable Object Please");
        }

        cardPlayManager = FindObjectOfType<CardPlayManager>();
        if(cardPlayManager == null) {
            Debug.LogError("There is no cardplay manager script, please place one in the scene");
        }
        currSprite = GetComponent<SpriteRenderer>();
    }

    void OnMouseDown()
    {
        if (currType != null)
        {
            cardPlayManager.AddPlayToTile(this);
        }


    }

    public void updateAppearance(TileTypeSO tile)
    {
        if (tile == null) { return; }

        prevType = currType;
        currType = tile;
        GetComponent<SpriteRenderer>().sprite = currType.picture;
    }

    public void undoTile()
    {
        updateAppearance(prevType);
    }

    public void ApplyBuildTile(BuildingCard building)
    {
        if(currType.type == BuildingManagement.TileType.LAKE || currType.type == BuildingManagement.TileType.FOREST 
        || currType.type == BuildingManagement.TileType.MOUNTAIN){
            return;
        }

        if (building == null)
        {
            Debug.LogError("Building being passed in is null.");
            return;
        }

        if (!isBuild)
        {
            currBuilding = building;
            currSprite.sprite = building.buildingSprite;
            isBuild = true;
        }

    }

    public void ApplyCropTile(SeedCard crop)
    {
        if(isBuild && !isSeed){
            if(currBuilding.buildingType == BuildingManagement.BuildingType.ACRE || 
            currBuilding.buildingType == BuildingManagement.BuildingType.FRUITGARDEN){
                currSprite.sprite = crop.buildingSprite;
                currSeed = crop;
                isSeed = true;
            } else {
                return;
            }
        } else {
            return;
        }
    }

}


