using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// CROSTZARD (author)
/// <summary>
/// Handles selling items, and, in that process, calculates what price the city is going to pay for them. Is a single instance.
/// The sell button UI calls this class to sell to their current shop.
/// </summary>
/// 
public class ShopManager : MonoBehaviour
{

    public delegate void ShopManagerEvents();
    public event ShopManagerEvents OnShopUpdate;

    public static ShopManager Instance;


    private PlayerData.PlayerDataManager playerData;
    private int costBonus = 2;

    private int lastCost = 100;
    // Random number. Checking if the crop has a bonus in a certain season is a long process. So I set this value every time player buys something
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

    void Update()
    {

    }


    public void SellResource(Shop shop)
    {
        Debug.Log($"Sold 1 {shop.Resource.name} to {shop.City.cityName}!");
        Debug.Log($"money increased by: {GetPrice(shop.Resource.basePrice, shop.Resource)} !");

    }

    private int GetPrice(int basePrice, PlayerData.ResourceSO resource)
    {
        // season checks for bonuses, etc.

        if (lastCost != 100) return lastCost; // read lastCost above for more info.

        if(TimeManagement.TimeManager.Instance.CurrentTime.SeasonInYear == TimeManagement.SeasonType.WINTER) 
        { 
            if (resource.name == "Wheat") 
            {
                return basePrice + costBonus;
            }
        }
        else if (TimeManagement.TimeManager.Instance.CurrentTime.SeasonInYear == TimeManagement.SeasonType.SPRING)
        {
            if (resource.name == "Oats")
            {
                return basePrice + costBonus;
            }

        }
        else if (TimeManagement.TimeManager.Instance.CurrentTime.SeasonInYear == TimeManagement.SeasonType.SUMMER)
        {
            if (resource.name == "Flowers")
            {
                return basePrice + costBonus;
            }

        }
        else if (TimeManagement.TimeManager.Instance.CurrentTime.SeasonInYear == TimeManagement.SeasonType.FALL)
        {
            if (resource.name == "Rice")
            {
                return basePrice + costBonus;
            }

        }


        return basePrice;
    }


    public void UpdateShop() 
    {
        OnShopUpdate.Invoke();
    }
}
