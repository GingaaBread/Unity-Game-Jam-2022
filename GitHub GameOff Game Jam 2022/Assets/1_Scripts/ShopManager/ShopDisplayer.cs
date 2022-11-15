using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopDisplayer : MonoBehaviour
{

    Transform shopUI;
    CityDemandSO city;

    TextMeshProUGUI textMesh;

    void Start()
    {


        textMesh = shopUI.GetComponentInChildren<TextMeshProUGUI>();
        textMesh.text = city.name; 

        
    }

    void Update()
    {
        
    }
}
