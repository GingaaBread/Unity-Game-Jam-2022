using PlayerData;
using System;
using System.Collections;
using System.Collections.Generic;
using TimeManagement;
using UnityEngine;
using UnityEngine.Assertions;

public class ShopPanel : ComputerPhaseStep
{

    [SerializeField] private ShopSellerPanel sellerAPanelObj;
    [SerializeField] private ShopSellerPanel sellerBPanelObj;
    [SerializeField] private ShopSellerPanel sellerCPanelObj;
    [SerializeField] private ShopSellerPanel sellerDPanelObj;
    [SerializeField] private ShopSellerDetailsPanel sellerDetailsPanelObj;

    [Header("resource logic")]
    [SerializeField] private ResourceSO Resource_SpringBonus;
    [SerializeField] private ResourceSO Resource_SummerBonus;
    [SerializeField] private ResourceSO Resource_AutumnBonus;
    [SerializeField] private ResourceSO Resource_WinterBonus;
    [SerializeField] private ResourceSO[] Resource_RareCrops;
    [SerializeField] private ResourceSO[] Resource_Livestock;

    [SerializeField] private Shop_BuyerSO[] buyers;


    private new void Awake() {
        Assert.IsNotNull(sellerAPanelObj);
        Assert.IsNotNull(sellerBPanelObj);
        Assert.IsNotNull(sellerCPanelObj);
        Assert.IsNotNull(sellerDPanelObj);
        Assert.IsNotNull(sellerDetailsPanelObj);
        Assert.IsNotNull(Resource_SpringBonus);
        Assert.IsNotNull(Resource_SummerBonus);
        Assert.IsNotNull(Resource_AutumnBonus);
        Assert.IsNotNull(Resource_WinterBonus);
        Assert.IsNotNull(buyers);
        Assert.IsTrue(buyers.Length == 4);
        Assert.IsNotNull(Resource_RareCrops);
        Assert.IsTrue(Resource_RareCrops.Length > 0);
        Assert.IsNotNull(Resource_Livestock);
        Assert.IsTrue(Resource_Livestock.Length > 0);

        sellerDetailsPanelObj.gameObject.SetActive(false);
    }

    public void EnterShop0() => EnterShop(0);
    public void EnterShop1() => EnterShop(1);
    public void EnterShop2() => EnterShop(2);
    public void EnterShop3() => EnterShop(3);

    private void EnterShop(int shopIndex) {
        sellerAPanelObj.gameObject.SetActive(false);
        sellerBPanelObj.gameObject.SetActive(false);
        sellerCPanelObj.gameObject.SetActive(false);
        sellerDPanelObj.gameObject.SetActive(false);
        sellerDetailsPanelObj.gameObject.SetActive(true);
        sellerDetailsPanelObj.ShowShopDetails(shopIndex);
    }

    public void ExitShopDetails() {
        sellerAPanelObj.gameObject.SetActive(true);
        sellerBPanelObj.gameObject.SetActive(true);
        sellerCPanelObj.gameObject.SetActive(true);
        sellerDPanelObj.gameObject.SetActive(true);
        sellerDetailsPanelObj.gameObject.SetActive(false);
    }

    internal void AttemptToSell(ResourceSO resourceBeingSold, int amount) {
        // TODO: implement check for sell logic
        // TODO: give money?
    }

    public override void StartProcessingForComputerPhase(bool isComputerPhaseDuringGameInit) {

        if(TimeManager.Instance.CurrentTime.RoundInSeason != 1) {
            OnFinishProcessing.Invoke(); // tell time manager we're done
            return;
        }

        // set resource for seller 0
        switch (TimeManager.Instance.CurrentTime.SeasonInYear) {
            case SeasonType.SUMMER: buyers[0].resourceA = Resource_SummerBonus; break;
            case SeasonType.FALL:   buyers[0].resourceA = Resource_AutumnBonus; break;
            case SeasonType.WINTER: buyers[0].resourceA = Resource_WinterBonus; break;
            case SeasonType.SPRING: buyers[0].resourceA = Resource_SpringBonus; break;
        }
        buyers[0].resourceB = null;

        // set resource for seller 1
        if (buyers[1].resourceA == null) {
            buyers[1].resourceA = Resource_RareCrops[0];
        } else {
            // get the next resource in the list
            int newIndex = (Array.IndexOf(Resource_RareCrops, buyers[1].resourceA) + 1) % Resource_RareCrops.Length;
            buyers[1].resourceA = Resource_Livestock[newIndex];
        }
        buyers[1].resourceB = null;

        // set resource for seller 2
        if (buyers[2].resourceA == null) {
            buyers[2].resourceA = Resource_Livestock[0];
        } else {
            // get the next resource in the list
            int newIndex = (Array.IndexOf(Resource_Livestock, buyers[2].resourceA) + 1) % Resource_Livestock.Length;
            buyers[2].resourceA = Resource_Livestock[newIndex];
        }
        buyers[2].resourceB = null;

        // set resource for seller 3


        // update seller panels with resources now set in their respective seller SOs
        sellerAPanelObj.UpdateResourcesUIBasedOnBuyer();
        sellerBPanelObj.UpdateResourcesUIBasedOnBuyer();
        sellerCPanelObj.UpdateResourcesUIBasedOnBuyer();
        sellerDPanelObj.UpdateResourcesUIBasedOnBuyer();

        OnFinishProcessing.Invoke(); // tell time manager we're done
    }


    protected override object[] CheckForMissingReferences() { return null; }
}
