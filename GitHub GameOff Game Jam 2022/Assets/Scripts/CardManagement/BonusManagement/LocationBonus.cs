using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "TileBonus/TileTypeBonus")]
public class LocationBonus: ScriptableObject{
    public BonusType bonus;
    public BuildingManagement.TileType TileType;
    public int BonusAmount = 0;

}