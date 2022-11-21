using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PlayerData;


/// CROSTZARD (author)
/// <summary>
/// ShopDisplayer makes sure to display UI propertly basing itself on the ResourceSO information AND some other stuff like what resource its buying
/// Displays items taking as reference their shop variable.
/// </summary>

public class ShopDisplayer : MonoBehaviour
{

    public Shop shop;
    /// <summary>
    /// Each shop shows the item they are buying. This is just the prefab of said icon.
    /// </summary>
    public GameObject shopIconPrefab;
    /// <summary>
    /// I use a grid layout group Component to position each icon as opposed to position them trough code. 
    /// </summary>
    public Transform shopIconHolder;

    private Image[] resourceIcons = new Image[5];

    // Do not edit this value
    private int maxDisplayAmount = 5;
    /// <summary>
    /// Returns the max amount of items that can be displayed in the shop
    /// </summary>
    public int MaxDisplayAmount { get { return maxDisplayAmount; } }

    TextMeshProUGUI cityNameText;
    TextMeshProUGUI resourcePriceText;

    public void UpdateShopDisplay(int displayNumber)
    {
        if (shopIconHolder.childCount == 0) SetUpShopIcons();
        Image icon = resourceIcons[displayNumber];
        ResourceSO resource = shop.Resources[displayNumber];

        cityNameText = GetComponentInChildren<TextMeshProUGUI>();
        resourcePriceText = icon.GetComponentInChildren<TextMeshProUGUI>();

        cityNameText.text = shop.City.cityName;

        if (resource == null) return;


        resourcePriceText.text = ShopManager.Instance.GetPrice(resource, shop).ToString() + "$";
        icon.sprite = resource.iconSprite;
        icon.color = Color.white;
    }

    private void SetUpShopIcons() 
    { 
        for(int i = 0; i < shop.itemAmount; i++) 
        {
            resourceIcons[i] = Instantiate(shopIconPrefab, shopIconHolder).GetComponent<Image>();
        }
    }

}
