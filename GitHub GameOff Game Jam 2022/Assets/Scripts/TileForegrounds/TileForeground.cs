using BuildingManagement;
using System.Collections;
using System.Collections.Generic;
using TimeManagement;
using UnityEngine;
using UnityEngine.Assertions;

public class TileForeground : MonoBehaviour {

    [Header("Debug options for testing only")]
    [Tooltip("When set, the object will auto initialize on Awake and use the state settings marked here as defaults.")]
    [SerializeField] private bool          _debugMode_AutoInitUsingDefaults;
    [SerializeField] private TileType      _defaultTileType;
    [SerializeField] private BuildingCard  _defaultBuidlingCard;
    [SerializeField] private LivestockCard _defaultLivestockCard;
    [SerializeField] private SeedCard      _defaultSeedCard;
    [SerializeField] private SeasonType    _defaultSeason;
    [SerializeField] private float         _defaultAgePercentage;

    [Header("Mappings to foreground UI elements for tiles which are just a base")]
    [SerializeField] private GameObject lakePrefab;
    [SerializeField] private GameObject forestPrefab;

    private enum ForegroundType { TILE, ANIMAL, SEED }
    private bool _isInitialized;
    private GameObject _child;

    private void Awake() {

        if (_debugMode_AutoInitUsingDefaults) {
            int tileSortOrder = transform.parent.GetComponent<SpriteRenderer>().sortingOrder;
            if      (_defaultLivestockCard != null) { Initialize(_defaultLivestockCard, tileSortOrder, true, _defaultSeason); } 
            else if (_defaultSeedCard      != null) { Initialize(_defaultSeedCard,      tileSortOrder, true, _defaultSeason); } 
            else if (_defaultBuidlingCard  != null) { Initialize(_defaultBuidlingCard,  tileSortOrder, true, _defaultSeason); } 
            else {                                    Initialize(_defaultTileType,      tileSortOrder, true, _defaultSeason); }
            //UpdateForSeasonAndAgePercentage(_defaultSeason, _defaultAgePercentage);
        }

    }


    public void Initialize(TileType tileType, int tileSortOrder, bool alwaysAnimate, SeasonType season) {
        switch (tileType) {
            case TileType.LAKE: _child = Instantiate(lakePrefab, transform, false); break;
            case TileType.FOREST: _child = Instantiate(forestPrefab, transform, false); break;
            case TileType.MOUNTAIN: break;
            case TileType.PLAINS: break;
            case TileType.HILLS: break;
            default: Debug.LogError($"TileForeground {this.name} never expected _tileType of {tileType}?"); break;
        }
        ForegroundIndependentInit(tileSortOrder, alwaysAnimate, season);
    }
    public void Initialize(BuildingCard buildingCard, int tileSortOrder, bool alwaysAnimate, SeasonType season) {
        if (buildingCard.tileForegroundChildPrefab == null)
            return;
        _child = Instantiate(buildingCard.tileForegroundChildPrefab, transform, false); 
        ForegroundIndependentInit(tileSortOrder, alwaysAnimate, season);
    }
    public void Initialize(LivestockCard livestockCard, int tileSortOrder, bool alwaysAnimate, SeasonType season) {
        if (livestockCard.tileForegroundChildPrefab == null)
            return;
        _child = Instantiate(livestockCard.tileForegroundChildPrefab, transform, false);
        ForegroundIndependentInit(tileSortOrder, alwaysAnimate, season);
    }
    public void Initialize(SeedCard seedCard, int tileSortOrder, bool alwaysAnimate, SeasonType season) {
        if (seedCard.tileForegroundChildPrefab == null)
            return;
        _child = Instantiate(seedCard.tileForegroundChildPrefab, transform, false);
        ForegroundIndependentInit(tileSortOrder, alwaysAnimate, season);
    }

    private void ForegroundIndependentInit(int tileSortOrder, bool alwaysAnimate, SeasonType season) {
        Assert.IsFalse(_isInitialized, $"TileForeground {this.name} never expected to be initialized twice. Check that debug flag is off? Check Initialize() isn't called twice somehow?");
        _child.GetComponent<TileForegroundChild>().PositionSpritesBasedOnTileSortOrder(tileSortOrder);
        _child.GetComponent<TileForegroundChild>().UpdateForSeasonAndAgePercentage(season, 0);
        _isInitialized = true;
    }

    /// <summary>
    /// to be called during computer phase
    /// </summary>
    /// <param name="season"></param>
    /// <param name="agePercentage"></param>
    public void UpdateForSeasonAndAgePercentage(SeasonType season, float agePercentage) {
        _child.GetComponent<TileForegroundChild>().UpdateForSeasonAndAgePercentage(season, agePercentage);
    }
    public void StartAnimating() {
        _child.GetComponent<TileForegroundChild>().StartAnimating();
    }
    public void StopAnimating() {
        _child.GetComponent<TileForegroundChild>().StopAnimating();
    }




}
