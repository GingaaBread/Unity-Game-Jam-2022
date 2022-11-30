using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CardBonus/TileBonus")]
public class TileBonus: ScriptableObject{
    public BonusType bonus;
    public BuildingManagement.TileType TileType;
    public int BonusAmount = 0;

}