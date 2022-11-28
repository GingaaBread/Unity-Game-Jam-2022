using PlayerData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class ShopResourcePanel : MonoBehaviour
{

    [SerializeField] private Image resourceImageObj;
    [SerializeField] private GameObject highlightObj;
    [SerializeField] private Button buttonObj;
    [SerializeField] private ShopPricePanel pricePanel;
    [SerializeField] private bool showPricePanel;

    public bool IsSelected { get; private set; }
    public bool IsClickable { get; private set; }
    public bool IsPriceVisible { get; private set; }
    public int Price { get; private set; }
    public ResourceSO Resource { get; private set; }



    private void Awake() {
        Assert.IsNotNull(resourceImageObj);
        Assert.IsNotNull(highlightObj);
        Assert.IsNotNull(buttonObj);
        Assert.IsNotNull(pricePanel);

        IsSelected = false;

        UpdateUIBasedOnState();
    }

    private void UpdateUIBasedOnState() {
        resourceImageObj.sprite = Resource != null ? Resource.iconSprite : null;
        
        highlightObj.SetActive(IsSelected);

        buttonObj.enabled = IsClickable;

        if (IsPriceVisible) {
            pricePanel.setText(Price.ToString());
            pricePanel.gameObject.SetActive(true);
        } else {
            pricePanel.gameObject.SetActive(false);
        }
    }

    public void SetPrice(int price) {
        this.Price = price;
        UpdateUIBasedOnState();
    }

    public void SetResource(ResourceSO resource) {
        this.Resource = resource;
        UpdateUIBasedOnState();
    }

    public void SetSelected(bool isSelected) {
        this.IsSelected = isSelected;
        UpdateUIBasedOnState();
    }

    public void SetPriceVisible(bool isVisible) {
        this.IsPriceVisible = isVisible;
        UpdateUIBasedOnState();
    }

    public void SetClickable(bool isClickable) {
        this.IsClickable = isClickable;
        UpdateUIBasedOnState();
    }
}
