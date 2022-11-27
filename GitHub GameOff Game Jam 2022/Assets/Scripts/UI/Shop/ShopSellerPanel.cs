using PlayerData;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class ShopSellerPanel : MonoBehaviour {

    [Header("References")]
    [SerializeField] private Shop_BuyerSO buyer;
    [SerializeField] private Image characterImageObj;
    [SerializeField] private TextMeshProUGUI characterSpeechObj;
    [SerializeField] private TextMeshProUGUI characterNameObj;
    [SerializeField] private TextMeshProUGUI characterCityObj;
    [SerializeField] private ShopResourcePanel resourceAObj;
    [SerializeField] private ShopResourcePanel resourceBObj;

    // todo: resource icons

    private void Awake() {
        Assert.IsNotNull(buyer);
        Assert.IsNotNull(characterImageObj);
        Assert.IsNotNull(characterSpeechObj);
        Assert.IsNotNull(characterNameObj);
        Assert.IsNotNull(characterCityObj);
        Assert.IsNotNull(resourceAObj);
        Assert.IsNotNull(resourceBObj);

        characterImageObj.sprite = buyer.CharacterImage_Summary;
        characterSpeechObj.text = buyer.CharacterSpeech_Summary;
        characterNameObj.text = buyer.CharacterName;
        characterCityObj.text = buyer.CharacterCity;

        // set up resources
        resourceAObj.SetClickable(false);
        resourceAObj.SetClickable(false);
    }

    public void UpdateResourcesUIBasedOnBuyer() {
        resourceAObj.SetResource(buyer.resourceA);

        if (buyer.resourceB != null)
            resourceBObj.SetResource(buyer.resourceB);
        else
            resourceBObj.gameObject.SetActive(false);
    }

}

