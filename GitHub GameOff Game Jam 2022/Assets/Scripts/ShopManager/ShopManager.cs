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

    private int lastCost = 100;
    // Predetermined number. Checking if the crop has a bonus in a certain season is a long process. So I set this value every time player buys something
    // So next time the script doesn't have to process anything (This value resets every time the shop is updated)

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {

            Debug.LogError("There is more than 1 instance of Shop Manager");
        }
    }
    private void Start()
    {
        UpdateShop();

    }

    void Update()
    {

    }


    public void SellResource(Shop shop)
    {
        if (shop.Resource == null) return; 

        Debug.Log($"Sold 1 {shop.Resource.name} to {shop.City.cityName}!");
        Debug.Log($"money increased by: {GetPrice(shop)} !");
        Debug.Log("-----------------");

        PlayerDataManager dataManager = PlayerDataManager.Instance;
        if (dataManager.HasItemInInventory(shop.Resource)) 
        {
            dataManager.DecreaseInventoryItemAmount(shop.Resource, 1);
            dataManager.IncreaseMoneyAmount(GetPrice(shop));
        }

        shop.SoldItem();
    }

    public int GetPrice(Shop shop)
    {

        int basePrice = shop.Resource.basePrice;
        ResourceSO resource = shop.Resource;
        float multiplier = shop.CostMultiplier;

        int price = 0;
        // season checks for bonuses, etc.

        if (lastCost != 100) return lastCost; // read lastCost above for more info.

        price = (int)(basePrice * multiplier);

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
        Debug.Log("shop updated!");
        lastCost = 100;
    }
}
