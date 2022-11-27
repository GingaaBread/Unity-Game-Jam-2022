using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using PlayerData;
using UnityEngine.Assertions;

public class ShopAndInventoryPanel : MonoBehaviour
{
    [SerializeField] public Canvas canvasToDisable;

    private void Awake() {
        Assert.IsNotNull(canvasToDisable);
        canvasToDisable.gameObject.SetActive(false);
    }

    public void SetShopAndInventoryVisibility(bool iSVisible) {
        canvasToDisable.gameObject.SetActive( iSVisible);
    }
}
