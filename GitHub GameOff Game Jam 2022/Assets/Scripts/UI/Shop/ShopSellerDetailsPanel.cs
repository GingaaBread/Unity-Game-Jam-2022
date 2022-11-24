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
    [SerializeField] Sprite[] SpriteDefault;
    [SerializeField] Sprite[] SpriteAfterPurchase;
    [SerializeField] string[] CharacterName;
    [SerializeField] string[] CharacterSpeechDefault;
    [SerializeField] string[] CharacterSpeechAfterPurchase;

    private int _shopIndex;

    private void Awake() {
        Assert.IsNotNull(resourcePanelAObj);
        Assert.IsNotNull(resourcePanelBObj);
        Assert.IsNotNull(characterImageObj);
        Assert.IsNotNull(characterSpeechObj);
        Assert.IsNotNull(characterNameObj);
        Assert.IsNotNull(shopPanelObj);

        Assert.IsNotNull(SpriteDefault);
        Assert.IsNotNull(SpriteAfterPurchase);
        Assert.IsNotNull(CharacterName);
        Assert.IsNotNull(CharacterSpeechDefault);
        Assert.IsNotNull(CharacterSpeechAfterPurchase);

        Assert.IsTrue(SpriteDefault.Length == 4);
        Assert.IsTrue(SpriteAfterPurchase.Length == 4);
        Assert.IsTrue(CharacterName.Length == 4);
        Assert.IsTrue(CharacterSpeechDefault.Length == 4);
        Assert.IsTrue(CharacterSpeechAfterPurchase.Length == 4);

        _shopIndex = 0;
        this.gameObject.SetActive(false);
    }

    public void ShowShopDetails(int shopIndex) {
        Assert.IsTrue(shopIndex >= 0 && shopIndex <= 3);
        _shopIndex = shopIndex;

        // update resource buttons ui
        resourcePanelAObj.SetSelected(true);
        resourcePanelBObj.SetSelected(false);
        if (shopIndex == 3) {
            resourcePanelBObj.gameObject.SetActive(true);
        } else {
            resourcePanelBObj.gameObject.SetActive(false);
        }

        // update character image, name, speech
        characterImageObj.sprite = SpriteDefault[shopIndex];
        characterSpeechObj.text = CharacterSpeechDefault[shopIndex];
        characterNameObj.text = CharacterName[shopIndex];

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
        characterImageObj.sprite = SpriteAfterPurchase[_shopIndex];
        characterSpeechObj.text = CharacterSpeechAfterPurchase[_shopIndex];

        // figure out what resource selected
        ResourceSO resourceBeingSold = resourcePanelAObj.IsSelected ? resourcePanelAObj.Resource : resourcePanelBObj.Resource;

        // propagate the sell command up to the shop panel, passing the resource sold
        shopPanelObj.AttemptToSell(resourceBeingSold, 1);
    }

}
