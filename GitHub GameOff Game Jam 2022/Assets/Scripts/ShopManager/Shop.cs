using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerData;


/// CROSTZARD (author)
/// <summary>
/// Handles shops. Holds the current resource the shop is buying.
/// </summary>
/// 
public class Shop : MonoBehaviour
{

    public CityDemandSO city;
    public ShopDisplayer displayer;

    /// <summary>
    /// How many items does this shop buy?
    /// </summary>
    [Range(1, 5)]
    public int itemAmount = 1;

    private List<ResourceSO> resources = new List<ResourceSO>(); 
    /// <summary>
    /// Returns this shop's resources. Shops mostly have 1 so 
    /// </summary>
    public List<ResourceSO> Resources { get { return resources;} }
    public CityDemandSO City { get { return city; } }

    private float costMultiplier = 1;
    // How many items has this shop sold? Some shops (just city D) decrease their buying price based on how many times something was sold.
    private int amountSold;

    public float CostMultiplier { get { return costMultiplier; } }

    // Gets called everytime an item is sold in this shop
    public void SoldItem() 
    {
        amountSold += 1;
        // City D's buying price decreases by 50% after the second buy
        if (city.name == "CityD" && amountSold >= itemAmount) costMultiplier = 0.5f;
    }

    // Gets called by ShopManager everytime the shop is updated.
    public void UpdateResource() 
    {
        for (int i = 0; i < itemAmount; i++) 
        { 
            resources.Add(city.RandomResource());
            displayer.UpdateShopDisplay(i);
        }

        
        costMultiplier = 1;
        amountSold = 0;

    }


}
