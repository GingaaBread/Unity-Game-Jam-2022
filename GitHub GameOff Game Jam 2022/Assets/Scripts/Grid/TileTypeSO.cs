using UnityEngine;
using BuildingManagement;

[CreateAssetMenu(menuName = "Tile/New Type")]
public class TileTypeSO : ScriptableObject
{
    public TileType type;
    public Sprite picture;
    public GameObject tileForegroundChildPrefab;

}