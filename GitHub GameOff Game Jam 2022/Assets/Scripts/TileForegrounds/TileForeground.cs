using BuildingManagement;
using System.Collections;
using System.Collections.Generic;
using TimeManagement;
using UnityEngine;
using UnityEngine.Assertions;

public class TileForeground : MonoBehaviour {

    [Header("Debug options for testing only")]
    [Tooltip("When set, the object will auto initialize on Awake and use the state settings already set against the object.")]
    [SerializeField] private bool _debugMode_AutoInit;

    [Header("State")]
    [SerializeField] private ForegroundType _foregroundType;
    [SerializeField] private TileType   _tileType;
    [SerializeField] private AnimalType _animalType;
    [SerializeField] private SeedType   _seedType;
    [SerializeField] private SeasonType _season;

    [Header("Mappings to UI elements")]
    [SerializeField] private GameObject forestPrefab;
    [SerializeField] private GameObject cowPrefab;
    [SerializeField] private GameObject pigPrefab;
    [SerializeField] private GameObject beesPrefab;
    [SerializeField] private GameObject lakePrefab;
    [SerializeField] private GameObject forrestPrefab;
    [SerializeField] private GameObject wheatPrefab;
    [SerializeField] private GameObject oatsPrefab;
    [SerializeField] private GameObject flowersPrefab;

    private enum ForegroundType { TILE, ANIMAL, SEED }
    private bool _isInitialized;
    private GameObject _child;

    private void Awake() {
        if (_debugMode_AutoInit) {
            Initialize(
                transform.parent.GetComponent<SpriteRenderer>().sortingOrder,
                true,
                SeasonType.SUMMER);
        }
    }

    public void Initialize(int tileSortOrder, bool alwaysAnimate, SeasonType season) {
        Assert.IsFalse(_isInitialized, $"TileForeground {this.name} never expected to be initialized twice. Check that debug flag is off? Check Initialize() isn't called twice somehow?");

        // isntantiate the right prefab under the foreground object based on the initialize params
        if (!_debugMode_AutoInit) {
            // set _tileType based on inputs
            // set _animalType based on inputs
            // set _seedType based on inputs
            _season = season;
        }

        // instantiate the child prefab
        switch (_foregroundType) {
            case ForegroundType.TILE:
                switch (_tileType) {
                    case BuildingManagement.TileType.LAKE: _child = Instantiate(lakePrefab, transform, false); break;
                    case BuildingManagement.TileType.FOREST: _child = Instantiate(forrestPrefab, transform, false); break;
                    case BuildingManagement.TileType.MOUNTAIN: break;
                    case BuildingManagement.TileType.PLAINS: break;
                    case BuildingManagement.TileType.HILLS: break;
                    default: Debug.LogError($"TileForeground {this.name} never expected _tileType of {_tileType}?"); break;
                }
                break;
            case ForegroundType.ANIMAL:
                switch (_animalType) {
                    case BuildingManagement.AnimalType.COW: _child = Instantiate(cowPrefab, transform, false); break;
                    case BuildingManagement.AnimalType.PIG: _child = Instantiate(pigPrefab, transform, false); break;
                    case BuildingManagement.AnimalType.BEES: _child = Instantiate(beesPrefab, transform, false); break;
                    default: Debug.LogError($"TileForeground {this.name} never expected _animalType of {_animalType}?"); break;
                }
                break;
            case ForegroundType.SEED:
                switch (_seedType) {
                    case BuildingManagement.SeedType.WHEAT: _child = Instantiate(wheatPrefab, transform, false); break;
                    case BuildingManagement.SeedType.OATS: _child = Instantiate(oatsPrefab, transform, false); break;
                    case BuildingManagement.SeedType.FLOWERS: _child = Instantiate(flowersPrefab, transform, false); break;
                    default: Debug.LogError($"TileForeground {this.name} never expected _seedType of {_seedType}?"); break;
                }
                break;
            default: Debug.LogError($"TileForeground {this.name} never expected _foregroundType of {_foregroundType}?"); break;
        }

        // initialize the prefab
        if (_child != null) {
            _child.GetComponent<TileForegroundChild>().PositionSpritesBasedOnTileSortOrder(tileSortOrder);
            _child.GetComponent<TileForegroundChild>().UpdateSpritesBasedOnParams(_season);
        }

        _isInitialized = true;
    }

    public void UpdateBasedOnParams(SeasonType season) {
        _child.GetComponent<TileForegroundChild>().UpdateSpritesBasedOnParams(_season);
    }
}
