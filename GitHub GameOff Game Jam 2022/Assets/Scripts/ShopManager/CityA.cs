using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewCityA", menuName = "City/ CityA")]
public class CityA : CityDemandSO
{


    public override PlayerData.ResourceSO RandomResource() 
    {

        int i = Random.Range(0, alwaysAvailable.Count);

        PlayerData.ResourceSO resource = alwaysAvailable[i];

        return resource;

    }

}
