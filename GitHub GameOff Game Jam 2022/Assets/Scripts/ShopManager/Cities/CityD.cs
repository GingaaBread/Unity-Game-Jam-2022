using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerData;
using TimeManagement;

[CreateAssetMenu(fileName = "NewCityD", menuName = "City/ CityD")]
public class CityD : CityDemandSO
{
    // CITY D buys random crops that are NOT in season. They are put in a queue so you always get to sell them in a year.

    Queue<ResourceSO> queue = new Queue<ResourceSO>();


    public override ResourceSO RandomResource()
    {

        if (queue.Count == 0)
        {
            AddToQueue();
            if (queue.Count == 0) return null;
        }

        ResourceSO resource = queue.Dequeue();

        return resource;

    }


    private void AddToQueue()
    {
        for (int i = 0; i < availableResources.Count; i++)
        {
            ResourceSO resource = availableResources[i];
            if (resource.seasonBonus != ShopManager.Instance.SeasonInBonus) 
            {
                queue.Enqueue(availableResources[i]);
            }
        }
    }

}
