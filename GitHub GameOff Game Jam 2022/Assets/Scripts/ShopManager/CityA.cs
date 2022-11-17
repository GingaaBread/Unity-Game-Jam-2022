using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// CROSTZARD (author)
/// <summary>
/// Each city needs to have their own logic on how they handle certain things (in this case how they randomly choose a resource to buy). 
/// </summary>

[CreateAssetMenu(fileName = "NewCityA", menuName = "City/ CityA")]
public class CityA : CityDemandSO
{

    public override PlayerData.ResourceSO RandomResource() 
    {

        int i = Random.Range(0, availableResources.Count);

        PlayerData.ResourceSO resource = availableResources[i];

        return resource;

    }

}
