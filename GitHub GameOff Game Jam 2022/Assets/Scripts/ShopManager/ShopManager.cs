using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerData;
using TimeManagement;


/// CROSTZARD (author)
/// <summary>
/// Handles selling items, and, in that process, calculates what price the city is going to pay for them. Is a single instance.
/// The sell button UI calls this class to sell to their current shop.
/// </summary>
/// 

public class ShopManager : MonoBehaviour
{

    public static ShopManager Instance;


    /// <summary>
    /// GameObject that holds all the cities
    /// </summary>
    public Transform cityHolder;

    private int costBonus = 2;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {

            Debug.LogError("There is more than 1 Shop Manager in the scene!!!");
        }
    }
    private void Start()
    {
        UpdateShop();

    }

    /// <summary>
    /// Sell the resource the shop is buying
    /// </summary>
    /// <param name="shop"> Shop you are selling to</param>
    public void SellResource(Shop shop)
    {

        for (int i = 0; i < shop.itemAmount; i++) 
        {
            if (shop.Resources == null) return;
            ResourceSO resource = shop.Resources[i];
            int price = GetPrice(resource, shop);

            Debug.Log($"Sold 1 {resource.name} to {shop.City.cityName}!");
            Debug.Log($"money increased by: {price} !");
            Debug.Log("-----------------");

            PlayerDataManager dataManager = PlayerDataManager.Instance;
            if (dataManager.HasItemInInventory(resource))
            {
                dataManager.DecreaseInventoryItemAmount(resource, 1);
                dataManager.IncreaseMoneyAmount(price);
                shop.SoldItem();
            }
        }
    }


    /// <summary>
    /// Get the price of a specified resource. Price varies with shops so you can also specify one.
    /// </summary>
    /// <param name="resource">resource to check.</param>
    /// <param name="shop">in what shop (leave blank if none in specific)</param>
    /// <returns></returns>
    public int GetPrice(ResourceSO resource, Shop shop = null)
    {

        int basePrice = resource.basePrice;
        float multiplier = (shop == null) ? 1 : shop.CostMultiplier;

        // season checks for bonuses, etc.

        int price = (int)(basePrice * multiplier);

        if(resource.season == TimeManager.Instance.CurrentTime.SeasonInYear) 
        { 
            if(resource.name == "Wheat" || resource.name == "Rice" || resource.name == "Flowers" || resource.name == "Oats") 
            {
                price = (int)(basePrice * multiplier + costBonus);
            }
        }

        return price;
    }

    /// <summary>
    /// Updates the whole shop (resources, price)
    /// Call this every season change or when you want the shop to display new items.
    /// </summary>
    public void UpdateShop() 
    {
        foreach (Transform t in cityHolder) 
        {
            Shop shop = t.GetComponent<Shop>();
            shop.UpdateResource();
        } 
    }
}
