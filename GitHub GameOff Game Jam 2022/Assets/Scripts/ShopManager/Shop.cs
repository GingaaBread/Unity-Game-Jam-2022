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

        resource = city.RandomResource(); // choose a random resource.
        displayer.UpdateShopDisplay(); // calls displayer to display the shop UI. Needs to be called here because it needs the resource to be set.

        // Add UpdateShop() to the ShopManager event that gets called everytime the shop updates (every season or so)
        ShopManager.Instance.OnShopUpdate += UpdateShop;
        // Event subscription after the ShopManager singleton is set.
    }

    // Gets called everytime an item is sold in this shop
    public void SoldItem() 
    {
        amountSold += 1;
        // City D's buying price decreases by 50% after the second buy
        if (city.name == "CityD" && amountSold >= 1) costMultiplier = 0.5f;
    }

    // Gets called by ShopManager everytime the shop is updated.
    private void UpdateShop() 
    {
        resource = city.RandomResource();

        displayer.UpdateShopDisplay();
        costMultiplier = 1;
        amountSold = 0;

    }

}
