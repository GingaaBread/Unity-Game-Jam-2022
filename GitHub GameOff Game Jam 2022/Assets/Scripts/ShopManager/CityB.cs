using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// CROSTZARD (author)
/// <summary>
/// Each city needs to have their own logic on how they handle certain things (in this case how they randomly choose a resource to buy). 
/// </summary>


[CreateAssetMenu(fileName = "NewCityB", menuName = "City/ CityB")]
public class CityB : CityDemandSO
{

    Queue<PlayerData.ResourceSO> queue = new Queue<PlayerData.ResourceSO>();


    public override PlayerData.ResourceSO RandomResource()
    {

        if(queue.Count == 0) 
        {
            AddToQueue();    
        }

        PlayerData.ResourceSO resource = queue.Dequeue();

        return resource;

    }


    private void AddToQueue() 
    { 
        for (int i = 0; i < availableResources.Count; i++) 
        {
            queue.Enqueue(availableResources[i]);
        }
    }




}
