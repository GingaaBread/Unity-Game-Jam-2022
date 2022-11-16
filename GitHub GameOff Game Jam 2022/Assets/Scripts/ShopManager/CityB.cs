using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
        for (int i = 0; i < alwaysAvailable.Count; i++) 
        {
            queue.Enqueue(alwaysAvailable[i]);
        }
    }




}
