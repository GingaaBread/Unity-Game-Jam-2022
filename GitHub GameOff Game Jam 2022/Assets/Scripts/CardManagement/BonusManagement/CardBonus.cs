using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 public enum BonusType{NoBonus, TurnBonus, CropBonus}
 public enum StageType{NoStage, PlantedBonus, GrowingBonus, HarvestBonus}

[CreateAssetMenu(menuName = "CardBonus/CardBonus")]

public class CardBonus : ScriptableObject{
    public SeasonBonus seasonBonus;
    public TileBonus tileBonus;
}



