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

    public bool IsSelected { get; private set; }
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
        pricePanel.setText(Resource != null ? Resource.basePrice.ToString() : "?");
    }

    public void SetResource(ResourceSO resource) {
        this.Resource = resource;
        UpdateUIBasedOnState();
    }

    public void SetSelected(bool isSelected) {
        this.IsSelected = isSelected;
        UpdateUIBasedOnState();
    }

}
