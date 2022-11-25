using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 public enum BonusType{NoBonus, TurnBonus, CropBonus}
 public enum StageType{NoStage, PlantedBonus, GrowingBonus, HarvestBonus}

[CreateAssetMenu(menuName = "TileBonus/TileBonus")]

public class TileBonus : ScriptableObject{
    public bool isSeasonBonus = true;
    public bool isLocationBonus = true;
    public SeasonBonus seasonBonus;
    public LocationBonus locationBonus;
}



