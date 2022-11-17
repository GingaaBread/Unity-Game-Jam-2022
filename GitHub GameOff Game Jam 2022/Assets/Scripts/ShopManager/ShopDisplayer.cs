using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopDisplayer : MonoBehaviour
{

    public Image resourceIcon;

    public Shop shop;

    TextMeshProUGUI textMesh;

    PlayerData.ResourceSO resource;

    void Start()
    {
        textMesh = GetComponentInChildren<TextMeshProUGUI>();

        UpdateShopDisplay();
    }

    void Update()
    {
        
    }

    public void UpdateShopDisplay() 
    {
        textMesh.text = shop.City.cityName;

        resource = shop.Resource;

        resourceIcon.sprite = resource.iconSprite;
        resourceIcon.color = Color.white;
    }

}
