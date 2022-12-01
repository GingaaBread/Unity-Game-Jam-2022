using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CardBonus/NeighborBonus")]
public class NeighborBonus : ScriptableObject
{
    public bool AnyAnimal = false;
    public BuildingManagement.AnimalType AnimalBonus = BuildingManagement.AnimalType.NONE;
    public BuildingManagement.TileType TileBonus = BuildingManagement.TileType.NONE;
    public BonusType Bonus = BonusType.NoBonus;
    public int BonusAmount = 0;
}
