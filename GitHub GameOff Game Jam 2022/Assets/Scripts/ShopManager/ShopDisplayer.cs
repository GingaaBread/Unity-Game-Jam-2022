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
        cityNameText = GetComponentInChildren<TextMeshProUGUI>();
        resourcePriceText = resourceIcon.GetComponentInChildren<TextMeshProUGUI>();
    }

    void Update()
    {
        
    }

    public void UpdateShopDisplay() 
    {
        cityNameText.text = shop.City.cityName;

        resource = shop.Resource;
        resourcePriceText.text = resource.basePrice.ToString();

        if (resource == null) return;

        resourceIcon.sprite = resource.iconSprite;
        resourceIcon.color = Color.white;
    }

}
