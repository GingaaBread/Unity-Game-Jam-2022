using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TimeManagement;
using PlayerData;

/// <Author> Author: Rohaid </Author> 
/// <Summary>Purpose: Stores the state of the tile in play,
/// updates the physical appearance based on its state,
/// updates the ResourceManager when crops are done growing/livestock are done. </Summary>
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
    private GameObject tileForeground;

    private float cropAge = 0;
    private float animalAge = 0;

    private PlayerDataManager playerDataManager;


    void Awake()
    {
        if (currType != null && currType.seasonSprites != null)
        {
            GetComponent<SpriteRenderer>().sprite = currType.seasonSprites[0];
        }
        else
        {
            Debug.LogError("Please put in a Tile Type SO");
        }

        cardPlayManager = FindObjectOfType<CardPlayManager>();
        playerDataManager = FindObjectOfType<PlayerDataManager>();
        currSprite = GetComponent<SpriteRenderer>();

        if (cardPlayManager == null)
        {
            Debug.LogError("There is no cardplay manager script, please place one in the scene");
        }
        if (tileForeground != null && currType != null)
        {
            _tileRowNum = currSprite.sortingOrder;
            GameObject tileForegroundObj = Instantiate(tileForeground, transform);
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

    public void UpdateEditorTileAppearance(TileTypeSO tile)
    {
        if (tile == null) { return; }
        prevType = currType;
        currType = tile;
        GetComponent<SpriteRenderer>().sprite = currType.seasonSprites[0];
    }

    public void undoTile()
    {
        UpdateEditorTileAppearance(prevType);
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
            
            if (transform.childCount > 0) // clear out current TileForegroundAnim
            {
                Destroy(transform.GetChild(0).gameObject);
            }

            GameObject tileForegroundObj = Instantiate(tileForeground, transform);
            tileForegroundObj.GetComponent<TileForeground>().Initialize(currBuilding, _tileRowNum, false, TimeManager.Instance.CurrentTime.SeasonInYear);
            isBuild = true;
            UpdateTileAppearance(TimeManager.Instance.CurrentTime.SeasonInYear);
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
                GameObject tileForegroundObj = Instantiate(tileForeground, transform);
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
                GameObject tileForegroundObj = Instantiate(tileForeground, transform);
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
    
    public void SetTileSeasonAppearance(SeasonType currSeason, Sprite[] sprites){
        switch(currSeason){
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

    public void UpdateTileAppearance(SeasonType currTime){

        UpdateTileAnimatableAppearance(currTime);

        if(!isBuild){
            SetTileSeasonAppearance(currTime, currType.seasonSprites);
            return;
        } else if(isBuild) {
            if(currType.type == BuildingManagement.TileType.HILLS){
                SetTileSeasonAppearance(currTime, currBuilding.hill_BuildingSprite);
            }else if(currType.type == BuildingManagement.TileType.PLAINS){
                SetTileSeasonAppearance(currTime, currBuilding.plain_BuildingSprite);
            }
            return;
        }
    }

    public void UpdateTileAnimatableAppearance(SeasonType currTime){
        if(transform.childCount > 0){ // check if there is a tile animatable object
           TileForeground currTileAnim =  transform.GetComponentInChildren<TileForeground>();
           if(currTileAnim!= null){ 
            // IMPORTANT: Every Tile Object has a TileAnim Object as a child 
            //but not every  TileAnim object has a TileAnimChild object.
            if(currTileAnim.transform.childCount>0){ 
                 currTileAnim.UpdateForSeasonAndAgePercentage(currTime, 0f);
            }
           
           }
        }
    }

    public void UpdateCropGrowth(SeasonType season){
        if(!isSeed){
            return;
        } else {
            cropAge++;
            float ageRatio = cropAge/currSeed.cropTotalTurnsTillPayoff;

            if(ageRatio == 1f){ // if done growing
                playerDataManager.IncreaseInventoryItemAmount(currSeed.payoffResource, currSeed.payoffAmount);
                isSeed = false;
                cropAge = 0;

                GameObject tileAnim = transform.GetChild(0).gameObject;
                if(tileAnim != null){
                     Destroy(transform.GetChild(0).gameObject); // destroy TileAnim object
                }

                return;

            } else if(transform.childCount > 0) { // have a tile animatable object
                TileForeground currTileAnim =  transform.GetComponentInChildren<TileForeground>();
                if(currTileAnim != null){
                    currTileAnim.UpdateForSeasonAndAgePercentage(season, ageRatio);
                }
               
            }
        }
    }

    public void UpdateLivestockGrowth(SeasonType season){
        if(!isAnimal){
            return;
        }else{
            animalAge++;
            if(animalAge == currAnimal.turnsTillLivestockPayoff){
                playerDataManager.IncreaseInventoryItemAmount(currAnimal.payoffResource, currAnimal.payoffAmount);
                animalAge = 0;
            }

        }

    }

}


