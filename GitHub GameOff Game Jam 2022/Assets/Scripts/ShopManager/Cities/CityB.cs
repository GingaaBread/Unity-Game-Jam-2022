using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerData;

/// CROSTZARD (author)
/// <summary>
/// Each city needs to have their own logic on how they handle certain things (in this case how they randomly choose a resource to buy). 
/// </summary>


[CreateAssetMenu(fileName = "NewCityB", menuName = "City/ CityB")]
public class CityB : CityDemandSO
{

    // CITY B buys rare crops. They are put in a queue so you always get to sell them in a year.

    Queue<ResourceSO> queue = new Queue<ResourceSO>();
    ResourceSO lastResource;

    public override ResourceSO RandomResource()
    {
        if(queue.Count == 0) 
        {
            AddToQueue();    
        }

        ResourceSO resource = queue.Dequeue();

        return resource;
    }

    private void AddToQueue() 
    { 
        for (int i = 0; i < availableResources.Count; i++) 
        {
            int rand = Random.Range(0, availableResources.Count);

            ResourceSO randomRes = (lastResource != availableResources[rand]) ? availableResources[rand] : availableResources[(rand + 1) % availableResources.Count];
            queue.Enqueue(randomRes);
            lastResource = randomRes;
        }
    }
}
