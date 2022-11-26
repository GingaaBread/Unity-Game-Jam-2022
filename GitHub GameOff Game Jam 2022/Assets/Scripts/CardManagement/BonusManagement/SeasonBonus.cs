using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "TileBonus/SeasonBonus")]
public class  SeasonBonus : ScriptableObject{
    public BonusType bonus;
    public StageType stage;
    public TimeManagement.SeasonType Season;
    public int BonusAmount = 0;

}
