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
    List<ResourceSO> unseasonalResources = new List<ResourceSO>();
    ResourceSO lastResource;

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
            ResourceSO res = availableResources[i];
            if (res.seasonBonus != ShopManager.Instance.SeasonInBonus)
            {
                unseasonalResources.Add(res);
            }

        }
        if (lastResource != null && unseasonalResources.Contains(lastResource)) unseasonalResources.Remove(lastResource);

        for (int i = 0; i < unseasonalResources.Count; i++)
        {
            int rand = Random.Range(0, unseasonalResources.Count);

            ResourceSO randomRes = (lastResource != unseasonalResources[rand]) ? unseasonalResources[rand] : unseasonalResources[(rand + 1) % unseasonalResources.Count];
            queue.Enqueue(randomRes);
            lastResource = randomRes;
        }
        unseasonalResources.Clear();
    }

}
