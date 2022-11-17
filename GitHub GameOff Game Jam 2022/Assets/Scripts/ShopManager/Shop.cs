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

    private void Awake()
    {
        resource = city.RandomResource();
        
    }

    private void Start()
    {
        // Add UpdateShop() to the ShopManager event that gets called everytime the shop updates (every turn or so)
        ShopManager.Instance.OnShopUpdate += UpdateShop;
        // Event subscription after the ShopManager singleton is set.
    }

    private void UpdateShop() 
    {
        resource = city.RandomResource();

        displayer.UpdateShopDisplay();

    }

}
