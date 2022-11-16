using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public abstract class CityDemandSO : ScriptableObject
{

    public string cityName;

    public List<PlayerData.ResourceSO> alwaysAvailable = new List<PlayerData.ResourceSO>();

    public List<PlayerData.ResourceSO> spring = new List<PlayerData.ResourceSO>();
    public List<PlayerData.ResourceSO> summer = new List<PlayerData.ResourceSO>();
    public List<PlayerData.ResourceSO> autumn = new List<PlayerData.ResourceSO>();
    public List<PlayerData.ResourceSO> winter = new List<PlayerData.ResourceSO>();


    public abstract PlayerData.ResourceSO RandomResource();

}
