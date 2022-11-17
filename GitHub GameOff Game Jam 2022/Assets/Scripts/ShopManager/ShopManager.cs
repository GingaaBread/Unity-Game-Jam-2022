using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{



    private int costBonus = 2;

    void Start()
    {
        
    }

    void Update()
    {
        
    }


    public void SellResource(Shop shop) 
    {
        Debug.Log($"Sold 1 {shop.Resource} to {shop.City.cityName}!");
    }
}
