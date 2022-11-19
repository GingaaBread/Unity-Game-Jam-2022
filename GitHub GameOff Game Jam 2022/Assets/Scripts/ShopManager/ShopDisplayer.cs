using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


/// CROSTZARD (author)
/// <summary>
/// ShopDisplayer makes sure to display UI propertly basing itself on the scriptable object information AND some other stuff like what resource its buying
/// </summary>

public class ShopDisplayer : MonoBehaviour
{
    public Image resourceIcon;

    public Shop shop;

    TextMeshProUGUI cityNameText;
    TextMeshProUGUI resourcePriceText;

    PlayerData.ResourceSO resource;

    void Start()
    {
    }

    void Update()
    {
        
    }

    public void UpdateShopDisplay() 
    {

        cityNameText = GetComponentInChildren<TextMeshProUGUI>();
        resourcePriceText = resourceIcon.GetComponentInChildren<TextMeshProUGUI>();
        cityNameText.text = shop.City.cityName;

        resource = shop.Resource;
        if (resource == null) return;


        resourcePriceText.text = ShopManager.Instance.GetPrice(shop).ToString() + "$";
        resourceIcon.sprite = resource.iconSprite;
        resourceIcon.color = Color.white;
    }

}
