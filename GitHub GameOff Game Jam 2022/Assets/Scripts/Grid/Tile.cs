using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TimeManagement;

// Author: Rohaid 
// Purpose: Stores the state of the tile in play and updates the physical appearance based on its state.
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
        GetComponent<SpriteRenderer>().sprite = currType.picture;
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
            currSprite.sprite = building.buildingSprite;
            if (transform.childCount > 0)
            {
                Destroy(transform.GetChild(0).gameObject);
            }
            GameObject tileForegroundObj = Instantiate(TileForeground, transform);
            tileForegroundObj.GetComponent<TileForeground>().Initialize(currBuilding, _tileRowNum, false, SeasonType.SPRING);
            isBuild = true;
        }

    }

    public void ApplyCropTile(SeedCard crop)
    {
        if (isBuild && !isSeed)
        {
            if (currBuilding.buildingType == BuildingManagement.BuildingType.ACRE)
            {
                currSprite.sprite = crop.buildingSprite;

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
                currSprite.sprite = animal.buildingSprite;
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


}


