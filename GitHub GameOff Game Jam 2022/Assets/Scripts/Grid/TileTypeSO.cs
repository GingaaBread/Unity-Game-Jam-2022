using UnityEngine;
using BuildingManagement;

[CreateAssetMenu(menuName = "Tile/New Type")]
public class TileTypeSO : ScriptableObject
{
    public TileType type;
    public Sprite[] seasonSprites = new Sprite[4]; // 0 spring, 1 Summer, 2 Fall, 3 Winter
    public GameObject tileForegroundChildPrefab;

}