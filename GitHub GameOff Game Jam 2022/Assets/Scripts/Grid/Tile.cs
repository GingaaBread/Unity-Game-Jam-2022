using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TimeManagement;
using PlayerData;

/// <Author> Author: Rohaid </Author> 
// Purpose: Stores the state of the tile in play and updates the physical appearance based on its state.

// when a sucessful tile placement 
public class Tile : MonoBehaviour
{

    [SerializeField]
    public TileTypeSO currType;
    private TileTypeSO prevType; // <- store tile type when the tile is changed 
    private SpriteRenderer currSprite;

    private bool isBuild = false;
    private BuildingCard currBuilding;
    private SeedCard currSeed;
    private LivestockCard currAnimal;
    private bool isSeed = false;
    private bool isAnimal = false;
    private CardPlayManager cardPlayManager;

    private int _tileRowNum;

    [SerializeField]
    private GameObject TileForeground;

    private float cropAge = 0;

    private PlayerDataManager playerDataManager;


    void Awake()
    {
        if (currType != null && currType.seasonSprites != null)
        {
            GetComponent<SpriteRenderer>().sprite = currType.seasonSprites[0];
        }
        else
        {
            Debug.LogError("Please put in a Tile Type Scriptable Object Please");
        }

        cardPlayManager = FindObjectOfType<CardPlayManager>();
        playerDataManager = FindObjectOfType<PlayerDataManager>();
        if (cardPlayManager == null)
        {
            Debug.LogError("There is no cardplay manager script, please place one in the scene");
        }
        currSprite = GetComponent<SpriteRenderer>();
        if (TileForeground != null && currType != null)
        {
            _tileRowNum = currSprite.sortingOrder;
            GameObject tileForegroundObj = Instantiate(TileForeground, transform);
            tileForegroundObj.GetComponent<TileForeground>().Initialize(currType, _tileRowNum, false, SeasonType.SPRING);
        }
    }

    void OnMouseDown()
    {
        if (currType != null)
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }
            cardPlayManager.AddPlayToTile(this);
        }


    }

    public void updateAppearance(TileTypeSO tile)
    {
        if (tile == null) { return; }

        prevType = currType;
        currType = tile;
        GetComponent<SpriteRenderer>().sprite = currType.seasonSprites[0];
    }

    public void undoTile()
    {
        updateAppearance(prevType);
    }

    public void ApplyBuildTile(BuildingCard building)
    {
        if (currType.type == BuildingManagement.TileType.LAKE || currType.type == BuildingManagement.TileType.FOREST
        || currType.type == BuildingManagement.TileType.MOUNTAIN)
        {
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
            
            if (transform.childCount > 0)
            {
                Destroy(transform.GetChild(0).gameObject);
            }
            GameObject tileForegroundObj = Instantiate(TileForeground, transform);
            tileForegroundObj.GetComponent<TileForeground>().Initialize(currBuilding, _tileRowNum, false, SeasonType.SPRING);
            isBuild = true;
            UpdateTileAppearance(TimeManager.Instance.CurrentTime);
        }

    }

    public void ApplyCropTile(SeedCard crop)
    {
        if (isBuild && !isSeed)
        {
            if (currBuilding.buildingType == BuildingManagement.BuildingType.ACRE)
            {
                PointInTime currSeason = TimeManager.Instance.CurrentTime;
                currSeed = crop;
                if (transform.childCount > 0)
                {
                    Destroy(transform.GetChild(0).gameObject);
                }
                GameObject tileForegroundObj = Instantiate(TileForeground, transform);
                tileForegroundObj.GetComponent<TileForeground>().Initialize(currSeed, _tileRowNum, false, SeasonType.SPRING);
                isBuild = true;
                isSeed = true;

                
            }
            else
            {
                return;
            }
        }
        else
        {
            return;
        }
    }

    public void ApplyLivestockTile(LivestockCard animal)
    {
        if (isBuild && !isAnimal)
        {
            if (currBuilding.buildingType == BuildingManagement.BuildingType.ANIMALPEN)
            {
                currAnimal = animal;
                isAnimal = true;
                if (transform.childCount > 0)
                {
                    Destroy(transform.GetChild(0).gameObject);
                }
                GameObject tileForegroundObj = Instantiate(TileForeground, transform);
                tileForegroundObj.GetComponent<TileForeground>().Initialize(animal, _tileRowNum, false, SeasonType.SPRING);
                isBuild = true;
            }
            else
            {
                return;
            }
        }
        else
        {
            return;
        }
    }

    public void SetSpriteOrderLayer(int currentRow)
    {
        _tileRowNum = currentRow * 10;
        GetComponent<SpriteRenderer>().sortingOrder = _tileRowNum;
    }
    public void SetTileAppearance(PointInTime currTime, Sprite[] sprites){
        switch(currTime.SeasonInYear){
            case SeasonType.SPRING: 
                currSprite.sprite = sprites[0];
                break;
            case SeasonType.SUMMER:
                currSprite.sprite = sprites[1];
                break;
            case SeasonType.FALL:
                currSprite.sprite = sprites[2];
                break;
            case SeasonType.WINTER:
                currSprite.sprite = sprites[3];
                break;
            default:
            break;
        }
    }

    public void UpdateTileAppearance(PointInTime currTime){
        if(transform.childCount > 0){
           TileForeground curr =  transform.GetComponentInChildren<TileForeground>();
           if(curr!= null){
            if(curr.transform.childCount>0){
                 curr.UpdateForSeasonAndAgePercentage(currTime.SeasonInYear, 0f);
            }
           
           }
        }
        if(!isBuild){
            SetTileAppearance(currTime, currType.seasonSprites);
            return;
        } else if(isBuild) {
            if(currType.type == BuildingManagement.TileType.HILLS){
                SetTileAppearance(currTime, currBuilding.hill_BuildingSprite);
            }else if(currType.type == BuildingManagement.TileType.PLAINS){
                SetTileAppearance(currTime, currBuilding.plain_BuildingSprite);
            }
            return;
        }
    }

    public void UpdateCropGrowth(SeasonType season){
        if(!isSeed){
            return;
        } else {
            cropAge++;
            float ageRatio = cropAge/((float)currSeed.cropTotalTurnsTillPayoff);
            if(ageRatio == 1f){
                playerDataManager.IncreaseInventoryItemAmount(currSeed.payoffResource, currSeed.payoffAmount);
                isSeed = false;
                cropAge = 0;
                Destroy(transform.GetChild(0).gameObject);
                return;
            }
            if(transform.childCount > 0) { // have a tile animatable object
                transform.GetComponentInChildren<TileForeground>().UpdateForSeasonAndAgePercentage(season, ageRatio);
            }
        }
    }

}


