using PlayerData;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class ShopPanel : MonoBehaviour
{

    [SerializeField] private ShopSellerDetailsPanel sellerDetailsPanelObj;

    private void Awake() {
        Assert.IsNotNull(sellerDetailsPanelObj);
    }

    public void EnterShop0() => EnterShop(0);
    public void EnterShop1() => EnterShop(1);
    public void EnterShop2() => EnterShop(2);
    public void EnterShop3() => EnterShop(3);

    private void EnterShop(int shopIndex) {
        sellerDetailsPanelObj.gameObject.SetActive(true);
        sellerDetailsPanelObj.ShowShopDetails(shopIndex);
    }

    internal void AttemptToSell(ResourceSO resourceBeingSold, int amount) {
        // TODO: implement check for sell logic
        // TODO: give money?
    }
}
