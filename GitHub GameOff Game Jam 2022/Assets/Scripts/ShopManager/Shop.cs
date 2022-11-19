using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// CROSTZARD (author)
/// <summary>
/// Handles shops. Holds the current resource the shop is buying.
/// </summary>
/// 
public class Shop : MonoBehaviour
{

    public CityDemandSO city;
    public ShopDisplayer displayer;

    private PlayerData.ResourceSO resource;

    public PlayerData.ResourceSO Resource { get { return resource; } }
    public CityDemandSO City { get { return city; } }

    private float costMultiplier = 1;
    private int amountSold;

    public float CostMultiplier { get { return costMultiplier; } }

    private void Start()
    {

    }

    // Gets called everytime an item is sold in this shop
    public void SoldItem() 
    {
        amountSold += 1;
        // City D's buying price decreases by 50% after the second buy
        if (city.name == "CityD" && amountSold >= 1) costMultiplier = 0.5f;
    }

    // Gets called by ShopManager everytime the shop is updated.
    public void UpdateResource() 
    {
        resource = city.RandomResource();

        displayer.UpdateShopDisplay();
        costMultiplier = 1;
        amountSold = 0;

    }

}
