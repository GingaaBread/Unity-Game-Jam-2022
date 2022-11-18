using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public abstract class CityDemandSO : ScriptableObject
{

    public string cityName;

    public List<PlayerData.ResourceSO> availableResources = new List<PlayerData.ResourceSO>();


    public abstract PlayerData.ResourceSO RandomResource();

}
