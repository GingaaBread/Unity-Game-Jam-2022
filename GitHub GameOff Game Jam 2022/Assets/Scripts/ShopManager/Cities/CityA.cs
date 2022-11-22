using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TimeManagement;
using PlayerData;

/// CROSTZARD (author)
/// <summary>
/// Each city needs to have their own logic on how they handle certain things (in this case how they randomly choose a resource to buy). 
/// </summary>

[CreateAssetMenu(fileName = "NewCityA", menuName = "City/ CityA")]
public class CityA : CityDemandSO
{

    // CITY A always buys the seasonal resource. Wheat in the winter, oats in spring, etc.

    public override ResourceSO RandomResource() 
    {
        Debug.Log($"Trying to access time {TimeManager.Instance} Gino wins");
        Debug.Log($"Trying to access time {TimeManager.Instance.CurrentTime}");
        Debug.Log($"Trying to access time {TimeManager.Instance.CurrentTime.SeasonInYear}");

        foreach (ResourceSO res in availableResources) 
        {
            if (res.seasonBonus == ShopManager.Instance.SeasonInBonus) return res;
        }

        return null;

    }

}
