using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{


    public CityDemandSO city;
    private PlayerData.ResourceSO resource;

    public PlayerData.ResourceSO Resource { get { return resource; } }
    public CityDemandSO City { get { return city; } }

    private void Awake()
    {
        resource = city.RandomResource();
    }

}
