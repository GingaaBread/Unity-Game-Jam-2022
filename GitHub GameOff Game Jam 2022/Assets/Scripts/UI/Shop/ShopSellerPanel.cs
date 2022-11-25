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

    // todo: resource icons

    private void Awake() {
        Assert.IsNotNull(buyer);
        Assert.IsNotNull(characterImageObj);
        Assert.IsNotNull(characterSpeechObj);
        Assert.IsNotNull(characterNameObj);
        Assert.IsNotNull(characterCityObj);

        characterImageObj.sprite = buyer.CharacterImage_Summary;
        characterSpeechObj.text = buyer.CharacterSpeech_Summary;
        characterNameObj.text = buyer.CharacterName;
        characterCityObj.text = buyer.CharacterCity;
    }




}

