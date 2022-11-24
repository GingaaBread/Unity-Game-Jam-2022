using UnityEngine;
using UnityEngine.EventSystems;
using TimeManagement;
using PlayerData;
using FMODUnity;
using UIManagement;

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

    private void OnMouseOver()
    {
        if (CardPlayManager.Instance.PlayIsInProgress())
        {
            GetComponent<SpriteRenderer>().color = CardPlayManager.Instance.hoverTint;
        }
    }

    private void OnMouseExit()
    {
        ResetTileColour();
    }
    
    public void ResetTileColour() => GetComponent<SpriteRenderer>().color = Color.white;

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

    public bool ApplyBuildTile(BuildingCard building)
    {
        if (currType.type == BuildingManagement.TileType.LAKE || currType.type == BuildingManagement.TileType.FOREST
        || currType.type == BuildingManagement.TileType.MOUNTAIN)
        {
            return false;
        }

        if (building == null)
        {
            Debug.LogError("Building being passed in is null.");
            return false;
        }

        if (!isBuild)
        {
            currBuilding = building;

            DeleteForegroundAnim();
            PlayTileSFX(building.buildingType);

            GameObject tileForegroundObj = Instantiate(tileForeground, transform);
            tileForegroundObj.GetComponent<TileForeground>().Initialize(currBuilding, _tileRowNum, false, TimeManager.Instance.CurrentTime.SeasonInYear);
            isBuild = true;
            UpdateTileAppearance(TimeManager.Instance.CurrentTime.SeasonInYear);

            return true;
        }

        return false;
    }

    public bool ApplyCropTile(SeedCard crop)
    {
        if (isBuild && !isSeed)
        {
            if (currBuilding.buildingType == BuildingManagement.BuildingType.ACRE)
            {
                currSeed = crop;
                PointInTime currSeason = TimeManager.Instance.CurrentTime;
                DeleteForegroundAnim();
                GameObject tileForegroundObj = Instantiate(tileForeground, transform);
                tileForegroundObj.GetComponent<TileForeground>().Initialize(currSeed, _tileRowNum, false, SeasonType.SPRING);
                isBuild = true;
                isSeed = true;
                PlayTileSFX(crop.buildingType);
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    public bool ApplyLivestockTile(LivestockCard animal)
    {
        if (isBuild && !isAnimal)
        {
            if (currBuilding.buildingType == BuildingManagement.BuildingType.ANIMALPEN)
            {
                currAnimal = animal;
                isAnimal = true;
                DeleteForegroundAnim();
                GameObject tileForegroundObj = Instantiate(tileForeground, transform);
                tileForegroundObj.GetComponent<TileForeground>().Initialize(animal, _tileRowNum, false, SeasonType.SPRING);
                isBuild = true;
                PlayTileSFX(animal.animalType);
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    public void SetSpriteOrderLayer(int currentRow)
    {
        _tileRowNum = currentRow * 10;
        GetComponent<SpriteRenderer>().sortingOrder = _tileRowNum;
    }

    public void SetTileSeasonAppearance(SeasonType currSeason, Sprite[] sprites)
    {
        switch (currSeason)
        {
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

    public void UpdateTileAppearance(SeasonType currTime)
    {

        UpdateTileAnimatableAppearance(currTime);

        if (!isBuild)
        {
            SetTileSeasonAppearance(currTime, currType.seasonSprites);
            return;
        }
        else if (isBuild)
        {
            if (currType.type == BuildingManagement.TileType.HILLS)
            {
                SetTileSeasonAppearance(currTime, currBuilding.hill_BuildingSprite);
            }
            else if (currType.type == BuildingManagement.TileType.PLAINS)
            {
                SetTileSeasonAppearance(currTime, currBuilding.plain_BuildingSprite);
            }
            return;
        }
    }

    public void UpdateTileAnimatableAppearance(SeasonType currTime)
    {
        if (transform.childCount > 0)
        { // check if there is a tile animatable object
            TileForeground currTileAnim = transform.GetComponentInChildren<TileForeground>();
            if (currTileAnim != null)
            {
                // IMPORTANT: Every Tile Object has a TileAnim Object as a child 
                //but not every  TileAnim object has a TileAnimChild object.
                if (currTileAnim.transform.childCount > 0)
                {
                    currTileAnim.UpdateForSeasonAndAgePercentage(currTime, 0f);
                }

            }
        }
    }

    public void UpdateCropGrowth(SeasonType season)
    {
        if (!isSeed)
        {
            return;
        }
        else
        {
            cropAge++;
            float ageRatio = cropAge / currSeed.cropTotalTurnsTillPayoff;

            if (ageRatio == 1f)
            { // if done growing
                playerDataManager.IncreaseInventoryItemAmount(currSeed.payoffResource, currSeed.payoffAmount);
                isSeed = false;
                cropAge = 0;

                FeedbackPanelManager.Instance.EnqueueGenericMessage(false, 
                    $"{currSeed.payoffAmount} {currSeed.payoffResource.name.ToLower()} harvested!");

                GameObject tileAnim = transform.GetChild(0).gameObject;
                if (tileAnim != null)
                {
                    Destroy(transform.GetChild(0).gameObject); // destroy TileAnim object
                }

                return;

            }
            else if (transform.childCount > 0)
            { // have a tile animatable object
                TileForeground currTileAnim = transform.GetComponentInChildren<TileForeground>();
                if (currTileAnim != null)
                {
                    currTileAnim.UpdateForSeasonAndAgePercentage(season, ageRatio);
                }

            }
        }
    }

    public void UpdateLivestockGrowth(SeasonType season)
    {
        if (!isAnimal)
        {
            return;
        }
        else
        {
            animalAge++;
            if (animalAge == currAnimal.turnsTillLivestockPayoff)
            {
                playerDataManager.IncreaseInventoryItemAmount(currAnimal.payoffResource, currAnimal.payoffAmount);
                animalAge = 0;

                FeedbackPanelManager.Instance.EnqueueGenericMessage(false,
                    $"{currAnimal.payoffAmount} {currAnimal.payoffResource.name.ToLower()} harvested!");
            }

        }

    }

    private void PlayTileSFX(string SFX)
    {
        RuntimeManager.PlayOneShot($"event:/{SFX}");
    }

    private void PlayTileSFX(BuildingManagement.BuildingType a)
    {
        switch (a)
        {
            case BuildingManagement.BuildingType.ACRE:
                PlayTileSFX("SFX/Placements/Play_SoilPlacement");
                break;
            case BuildingManagement.BuildingType.ANIMALPEN:
                PlayTileSFX("SFX/Placements/Play_BuildingPlacement");
                break;
            default:
                break;
        }
    }
    private void PlayTileSFX(BuildingManagement.SeedType a)
    {
        PlayTileSFX("SFX/Crops/Play_CropPlacement");
    }

    private void PlayTileSFX(BuildingManagement.AnimalType a)
    {
        switch (a)
        {
            case BuildingManagement.AnimalType.COW:
                PlayTileSFX("SFX/Placements/Animal Placement/Play_AnimalPlacement_Cow");
                break;
            case BuildingManagement.AnimalType.PIG:
                PlayTileSFX("SFX/Placements/Animal Placement/Play_AnimalPlacement_Pig");
                break;
            case BuildingManagement.AnimalType.FISH:
                PlayTileSFX("SFX/Placements/Animal Placement/Play_AnimalPlacement_Fish");
                break;
            case BuildingManagement.AnimalType.BEES:
                PlayTileSFX("SFX/Placements/Animal Placement/Play_AnimalPlacement_Bee");
                break;
            case BuildingManagement.AnimalType.SHEEP:
                PlayTileSFX("SFX/Placements/Animal Placement/Play_AnimalPlacement_Sheep");
                break;
            default:
                break;
        }
    }

    private void DeleteForegroundAnim()
    {
        if (transform.childCount > 0) // clear out current TileForegroundAnim
        {
            Destroy(transform.GetChild(0).gameObject);
        }
    }
}


