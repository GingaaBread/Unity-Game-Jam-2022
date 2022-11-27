using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using TMPro;
using PlayerData;

public class ShopSellerDetailsPanel : MonoBehaviour
{
    [Header("references to other gameobjects")]
    [SerializeField] public ShopResourcePanel resourcePanelAObj;
    [SerializeField] public ShopResourcePanel resourcePanelBObj;
    [SerializeField] private Image characterImageObj;
    [SerializeField] private TextMeshProUGUI characterSpeechObj;
    [SerializeField] private TextMeshProUGUI characterNameObj;
    [SerializeField] private ShopPanel shopPanelObj;

    [Header("sprites to use for character")]
    [SerializeField] Shop_BuyerSO[] Buyers;

    private int _shopIndex;

    private void Awake() {
        Assert.IsNotNull(resourcePanelAObj);
        Assert.IsNotNull(resourcePanelBObj);
        Assert.IsNotNull(characterImageObj);
        Assert.IsNotNull(characterSpeechObj);
        Assert.IsNotNull(characterNameObj);
        Assert.IsNotNull(shopPanelObj);
        Assert.IsNotNull(Buyers);
        Assert.IsTrue(Buyers.Length == 4);

        _shopIndex = 0;
    }

    public void ShowShopDetails(int shopIndex) {
        Assert.IsTrue(shopIndex >= 0 && shopIndex <= 3);
        _shopIndex = shopIndex;

        // update resource buttons ui
        resourcePanelAObj.SetSelected(true);
        resourcePanelBObj.SetSelected(false);
        resourcePanelAObj.SetPriceVisible(true);
        resourcePanelBObj.SetPriceVisible(true);
        resourcePanelAObj.SetClickable(true);
        resourcePanelBObj.SetClickable(true);
        resourcePanelAObj.SetResource(Buyers[_shopIndex].resourceA);
        resourcePanelBObj.SetSelected(Buyers[_shopIndex].resourceB);
        if (Buyers[_shopIndex].resourceB != null) {
            resourcePanelBObj.gameObject.SetActive(true);
        } else {
            resourcePanelBObj.gameObject.SetActive(false);
        }

        // update character image, name, speech
        characterImageObj.sprite = Buyers[_shopIndex].CharacterImage_Detail;
        characterSpeechObj.text = Buyers[_shopIndex].CharacterSpeech_Detail;
        characterNameObj.text = Buyers[_shopIndex].CharacterName;
    }

    public void OnSelectResourceA() {
        resourcePanelAObj.SetSelected(true);
        resourcePanelBObj.SetSelected(false);
    }

    public void OnSelectResourceB() {
        resourcePanelAObj.SetSelected(false);
        resourcePanelBObj.SetSelected(true);
    }

    public void OnPressSell() {
        Assert.IsTrue(resourcePanelAObj.IsSelected || resourcePanelBObj.IsSelected, "never expected to have no resource selected in shop details screen");

        // update character image, speech
        characterImageObj.sprite = Buyers[_shopIndex].CharacterImage_DetailAfterPurchase;
        characterSpeechObj.text = Buyers[_shopIndex].CharacterSpeech_DetailAfterPurchase;

        // figure out what resource selected
        ResourceSO resourceBeingSold = resourcePanelAObj.IsSelected ? resourcePanelAObj.Resource : resourcePanelBObj.Resource;

        // propagate the sell command up to the shop panel, passing the resource sold
        shopPanelObj.AttemptToSell(resourceBeingSold, 1);
    }

}
