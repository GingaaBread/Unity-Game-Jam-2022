using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopDisplayer : MonoBehaviour
{

    public Image resourceIcon;

    public CityDemandSO city;

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
        textMesh.text = city.cityName;

        resource = city.RandomResource();

        resourceIcon.sprite = resource.iconSprite;
        resourceIcon.color = Color.white;
    }

}
