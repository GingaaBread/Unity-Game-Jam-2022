using UnityEngine;
using UnityEngine.EventSystems;
using TimeManagement;
using PlayerData;
using FMODUnity;
using UIManagement;

/// <Author> Author: Rohaid </Author> 
/// <Summary>Purpose: Stores the state of the tile in play,
/// updates the physical appearance based on its state,
/// updates the ResourceManager when crops are done growing/livestock are done.
/// Plays SFX when tiles are placed and handles bonuses.
/// </Summary>
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
    private float turnsTillCropHarvest = 0;
    private int cropHarvestAmount = 0;
    private float animalAge = 0;

    private PlayerDataManager playerDataManager;
    private bool _bonusSeasonApplied;
    private bool _bonusTileApplied;

    void Awake(){
        if (currType != null && currType.seasonSprites != null){
            GetComponent<SpriteRenderer>().sprite = currType.seasonSprites[0];
        }
        else{
            Debug.LogError("Please put in a Tile Type SO");
        }

        cardPlayManager = FindObjectOfType<CardPlayManager>();
        Debug.Assert(cardPlayManager != null, "There is no cardplay manager script, please place one in the scene");
        playerDataManager = FindObjectOfType<PlayerDataManager>();
        Debug.Assert(playerDataManager != null, "There is no player data manager script, please place one in the scene");
        currSprite = GetComponent<SpriteRenderer>();

        if (tileForeground != null && currType != null){
            _tileRowNum = currSprite.sortingOrder;
            GameObject tileForegroundObj = Instantiate(tileForeground, transform);
            tileForegroundObj.GetComponent<TileForeground>().Initialize(currType, _tileRowNum, false, SeasonType.SPRING);
        }
    }

    private void OnMouseOver(){
        if (CardPlayManager.Instance.PlayIsInProgress()){
            GetComponent<SpriteRenderer>().color = CardPlayManager.Instance.hoverTint;
        }
    }

    private void OnMouseExit(){
        ResetTileColour();
    }

    public void ResetTileColour() => GetComponent<SpriteRenderer>().color = Color.white;

    void OnMouseDown(){
        if (currType != null){
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }
            cardPlayManager.AddPlayToTile(this);
        }


    }

    public void UpdateEditorTileAppearance(TileTypeSO tile){
        if (tile == null) { return; }
        prevType = currType;
        currType = tile;
        GetComponent<SpriteRenderer>().sprite = currType.seasonSprites[0];
    }

    public void undoTile(){
        UpdateEditorTileAppearance(prevType);
    }

    public bool ApplyBuildTile(BuildingCard building){

        if (currType.type == BuildingManagement.TileType.LAKE || currType.type == BuildingManagement.TileType.FOREST
        || currType.type == BuildingManagement.TileType.MOUNTAIN){
            return false;
        }

        if (building == null){
            Debug.LogError("Building being passed in is null.");
            return false;
        }

        if (!isBuild){
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

    public bool ApplyCropTile(SeedCard crop){
        if (isBuild && !isSeed){
            if (currBuilding.buildingType == BuildingManagement.BuildingType.ACRE){

                currSeed = crop;
                PointInTime currSeason = TimeManager.Instance.CurrentTime;
                DeleteForegroundAnim();
                GameObject tileForegroundObj = Instantiate(tileForeground, transform);
                tileForegroundObj.GetComponent<TileForeground>().Initialize(currSeed, _tileRowNum, false, SeasonType.SPRING);
                isBuild = true;
                isSeed = true;

                //variables store these values because they will be changed by effects;
                //and I don't want to change the values in the SO because those changes 
                //will persist outside of play mode.
                cropHarvestAmount = crop.payoffAmount;
                turnsTillCropHarvest = crop.cropTotalTurnsTillPayoff;
                if (crop.bonus != null){
                    ApplyBonus(crop.bonus);
                }

                PlayTileSFX(crop.buildingType);
                return true;
            }
            else{
                return false;
            }
        }
        else{
            return false;
        }
    }

    public bool ApplyLivestockTile(LivestockCard animal){
        if (isBuild && !isAnimal){
            if (currBuilding.buildingType == BuildingManagement.BuildingType.ANIMALPEN){
                currAnimal = animal;
                isAnimal = true;
                DeleteForegroundAnim();
                GameObject tileForegroundObj = Instantiate(tileForeground, transform);
                tileForegroundObj.GetComponent<TileForeground>().Initialize(animal, _tileRowNum, false, SeasonType.SPRING);
                isBuild = true;
                PlayTileSFX(animal.animalType);
                return true;
            }
            else{
                return false;
            }
        }
        else{
            return false;
        }
    }

    public void SetSpriteOrderLayer(int currentRow){
        _tileRowNum = currentRow * 10;
        GetComponent<SpriteRenderer>().sortingOrder = _tileRowNum;
    }

    public void SetTileSeasonAppearance(SeasonType currSeason, Sprite[] sprites){
        switch (currSeason){
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

        if (!isBuild){
            SetTileSeasonAppearance(currTime, currType.seasonSprites);
            return;
        }
        else if (isBuild){
            if (currType.type == BuildingManagement.TileType.HILLS){
                SetTileSeasonAppearance(currTime, currBuilding.hill_BuildingSprite);
            }
            else if (currType.type == BuildingManagement.TileType.PLAINS){
                SetTileSeasonAppearance(currTime, currBuilding.plain_BuildingSprite);
            }
            return;
        }
    }

    public void UpdateTileAnimatableAppearance(SeasonType currTime){
        if (transform.childCount > 0){ // check if there is a tile animatable object
            TileForeground currTileAnim = transform.GetComponentInChildren<TileForeground>();
            if (currTileAnim != null){
                // IMPORTANT: Every Tile Object has a TileAnim Object as a child 
                //but not every  TileAnim object has a TileAnimChild object.
                if (currTileAnim.transform.childCount > 0){
                    currTileAnim.UpdateForSeasonAndAgePercentage(currTime, 0f);
                }

            }
        }
    }

    public void UpdateCropGrowth(SeasonType season){
        if (!isSeed) return;
    
            cropAge++;
            float ageRatio = cropAge / turnsTillCropHarvest;
            if (currSeed.bonus != null){
                ApplySeasonBonus(currSeed.bonus);
            }

            if (ageRatio == 1f){ // if done growing
                playerDataManager.IncreaseInventoryItemAmount(currSeed.payoffResource, cropHarvestAmount);
                isSeed = false;
                cropAge = 0;
                int trueCropAmount = cropHarvestAmount == 0 ? currSeed.payoffAmount : cropHarvestAmount;
                FeedbackPanelManager.Instance.EnqueueGenericMessage(false,
                    $"{trueCropAmount} {currSeed.payoffResource.name.ToLower()} harvested!");

                GameObject tileAnim = transform.GetChild(0).gameObject;
                if (tileAnim != null){
                    Destroy(transform.GetChild(0).gameObject); // destroy TileAnim object
                }

                currSeed = null;
                ResetBonus();
                return;

            }
            else if (transform.childCount > 0){ // have a tile animatable object
                TileForeground currTileAnim = transform.GetComponentInChildren<TileForeground>();
                if (currTileAnim != null){
                    currTileAnim.UpdateForSeasonAndAgePercentage(season, ageRatio);
                }

        }
    }

    public void UpdateLivestockGrowth(SeasonType season){
        if (!isAnimal) return;

            animalAge++;
            if (animalAge == currAnimal.turnsTillLivestockPayoff){
                playerDataManager.IncreaseInventoryItemAmount(currAnimal.payoffResource, currAnimal.payoffAmount);
                animalAge = 0;

                FeedbackPanelManager.Instance.EnqueueGenericMessage(false,
                    $"{currAnimal.payoffAmount} {currAnimal.payoffResource.name.ToLower()} harvested!");
            }

    }

    private void PlayTileSFX(string SFX){
        RuntimeManager.PlayOneShot($"event:/{SFX}");
    }

    private void PlayTileSFX(BuildingManagement.BuildingType a){
        switch (a){
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
    private void PlayTileSFX(BuildingManagement.SeedType a){
        PlayTileSFX("SFX/Crops/Play_CropPlacement");
    }
    private void PlayTileSFX(BuildingManagement.AnimalType a){
        switch (a){
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
    private void DeleteForegroundAnim(){
        if (transform.childCount > 0){ // clear out current TileForegroundAnim
        
            Destroy(transform.GetChild(0).gameObject);
        }
    }

    private void ApplyBonus(CardBonus bonus){
        if (bonus.SeasonBonus != null) ApplySeasonBonus(bonus);
        if (bonus.TileBonus != null) ApplyTileBonus(bonus);
        if (bonus.NeighborBonus.Length != 0) ApplyNeighborBonus(bonus.NeighborBonus);

    }


    private void ApplySeasonBonus(CardBonus bonus){
        if (_bonusSeasonApplied) return;
        if (bonus.SeasonBonus == null) return;
      
            if (bonus.SeasonBonus.stage == StageType.PlantedBonus){
                if (TimeManager.Instance.CurrentTime.SeasonInYear == bonus.SeasonBonus.Season){
                    ApplyBonusEffect(bonus.SeasonBonus.bonus, bonus.SeasonBonus.BonusAmount);
                }
                _bonusSeasonApplied = true;
            }
            else if (bonus.SeasonBonus.stage == StageType.GrowingBonus){
                if (TimeManager.Instance.CurrentTime.SeasonInYear == bonus.SeasonBonus.Season){
                    ApplyBonusEffect(bonus.SeasonBonus.bonus, bonus.SeasonBonus.BonusAmount);
                    _bonusSeasonApplied = true;
                    //important to put in here because this section checks every turn the plant 
                    //is growing
                }
            }
            else if (bonus.SeasonBonus.stage == StageType.HarvestBonus && (cropAge == turnsTillCropHarvest)){
                if (TimeManager.Instance.CurrentTime.SeasonInYear == bonus.SeasonBonus.Season){
                    ApplyBonusEffect(bonus.SeasonBonus.bonus, bonus.SeasonBonus.BonusAmount);
                }
                _bonusSeasonApplied = true;
            }
            return;
    }

    private void ApplyTileBonus(CardBonus bonus){
        if (_bonusTileApplied) return;
        
        if (bonus.TileBonus.TileType == currType.type){
                ApplyBonusEffect(bonus.TileBonus.bonus, bonus.TileBonus.BonusAmount);
        }
         _bonusSeasonApplied = true;
        
    }

    private void ApplyNeighborBonus(NeighborBonus[] NeighborBonus){
        //Works with the current map but no garuntee it would work with different sized tiles.
        Vector3 adjustedCenter = new Vector3(transform.position.x, transform.position.y - .5f, transform.position.z);

        Collider2D[] NeighborTiles = Physics2D.OverlapCircleAll(adjustedCenter, 1.5f);
        bool appliedBonus = false;

        for (int f = 0; f < NeighborBonus.Length; f++){
            if (NeighborBonus[f] == null) continue;
            for (int i = 0; i < NeighborTiles.Length; i++){
                if (appliedBonus) break;
                if (NeighborTiles[i].transform.position == this.transform.position) continue; // skip Self 
                if (NeighborBonus[f].AnyAnimal){
                    if (checkHasAnimalBonus(NeighborTiles[i].GetComponent<Tile>())){
                        ApplyBonusEffect(NeighborBonus[f].Bonus, NeighborBonus[f].BonusAmount);
                        appliedBonus = true;
                        break;
                    }
                }

                if (NeighborBonus[f].AnimalBonus != BuildingManagement.AnimalType.NONE){
                    if (checkAnimalBonus(NeighborTiles[i].GetComponent<Tile>(), NeighborBonus[f].AnimalBonus)){
                        ApplyBonusEffect(NeighborBonus[f].Bonus, NeighborBonus[f].BonusAmount);
                        appliedBonus = true;
                        break;
                    }
                }

                if (NeighborBonus[f].TileBonus != BuildingManagement.TileType.NONE){
                    if (checkTileBonus(NeighborTiles[i].GetComponent<Tile>(), NeighborBonus[f].TileBonus)){
                        ApplyBonusEffect(NeighborBonus[f].Bonus, NeighborBonus[f].BonusAmount);
                        appliedBonus = true;
                        break;
                    }
                }
            }
            appliedBonus = false;
        }
    }

    private void ApplyBonusEffect(BonusType bonus, int bonusAmount){
        switch (bonus){
            case BonusType.TurnBonus:
                turnsTillCropHarvest += bonusAmount;
                Debug.Assert(turnsTillCropHarvest > 0, $"{currSeed.bonus.name} is making the number of turns negative or equal to zero.");
                return;
            case BonusType.CropBonus:
                cropHarvestAmount += bonusAmount;
                Debug.Assert(cropHarvestAmount > 0, $"{currSeed.bonus.name} is making the harvest amount negative or equal to zero.");
                return;
            case BonusType.NoBonus:
                return;
        }
    }

    private void ResetBonus(){
        _bonusSeasonApplied = false;
        _bonusTileApplied = false;
        cropHarvestAmount = 0;
        turnsTillCropHarvest = 0;
    }

    private bool checkHasAnimalBonus(Tile tile){
        return tile.hasAnimals();
    }

    private bool checkAnimalBonus(Tile tile, BuildingManagement.AnimalType animal){

        return tile.returnAnimal() == animal;
    }

    private bool checkTileBonus(Tile tile, BuildingManagement.TileType tileType){
        return tile.returnTile() == tileType;
    }

    public BuildingManagement.TileType returnTile(){
        Debug.Assert(currType != null,
            $"This current tile has no tile type. The tile is located at {transform.position}");
        Debug.Assert(currType.type != BuildingManagement.TileType.NONE,
            $"This current tile has been assigned a tile type of NONE.The tile is located at {transform.position}");

        return currType.type;
    }
    public BuildingManagement.AnimalType returnAnimal(){
        BuildingManagement.AnimalType currAnimalType =
            (currAnimal != null) ? currAnimal.animalType : BuildingManagement.AnimalType.NONE;
        return currAnimalType; 
    }
    public bool hasAnimals(){
        return isAnimal;
    }

}


