using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "NewCity", menuName = "City")]
public class CityDemandSO : ScriptableObject
{

    public string cityName;

    public List<PlayerData.ResourceSO> winter = new List<PlayerData.ResourceSO>();


}
