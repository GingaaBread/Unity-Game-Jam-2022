using FMODUnity;
using PlayerData;
using System;
using System.Collections;
using System.Collections.Generic;
using TimeManagement;
using UIManagement;
using UnityEngine;
using UnityEngine.Assertions;

public class ShopPanel : ComputerPhaseStep
{

    [Header("debugging")]
    [SerializeField] private bool DebugMode;

    [Header("references to other objects")]
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
    [SerializeField] private ResourceSO[] Resource_BuyerD;

    [SerializeField] private Shop_BuyerSO[] buyers;

    [Header("FMOD Event References")]
    [SerializeField] private EventReference saleFailedFMODEvent;

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
        Assert.IsNotNull(Resource_BuyerD);
        Assert.IsTrue(Resource_BuyerD.Length > 0);

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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="resourceBeingSold">resource to be sold</param>
    /// <param name="amountBeingSold"> number of resources to be sold</param>
    /// <returns>true if sale succeeded. false otherwise (if not enough inventory)</returns>
    internal bool AttemptToSell(ResourceSO resourceBeingSold, int amountBeingSold, int price) {

        if (DebugMode) { Debug.Log($"attempted to sell {amountBeingSold} {resourceBeingSold}"); }

        // don't sell if player doesn't have enough in inventory
        if (PlayerDataManager.Instance.GetInventoryItemAmount(resourceBeingSold) < amountBeingSold) {
            if (!saleFailedFMODEvent.IsNull) {
                RuntimeManager.PlayOneShot(saleFailedFMODEvent);
            }
            return false; 
        }

        PlayerDataManager.Instance.DecreaseInventoryItemAmount(resourceBeingSold, amountBeingSold);
        PlayerDataManager.Instance.IncreaseMoneyAmount(price);
        QuestManager.Instance.NotifyOfResourceSale(resourceBeingSold, price);
        FeedbackPanelManager.Instance.EnqueueMoneyReception(price, true);
        FeedbackPanelManager.Instance.InitiateInstantDisplayQueue();

        return true;
    }

    public override void StartProcessingForComputerPhase(bool isComputerPhaseDuringGameInit) {

        if (TimeManager.Instance.CurrentTime.RoundInSeason != 1) {
            OnFinishProcessing.Invoke(); // tell time manager we're done
            return;
        }

        // update each buyer SO with their current resources
        UpdateResourcesForBuyerA();
        UpdateResourcesForBuyerB();
        UpdateResourcesForBuyerC();
        UpdateResourcesForBuyerD();

        // update seller panels with resources now set in their respective seller SOs
        sellerAPanelObj.UpdateResourcesUIBasedOnBuyer();
        sellerBPanelObj.UpdateResourcesUIBasedOnBuyer();
        sellerCPanelObj.UpdateResourcesUIBasedOnBuyer();
        sellerDPanelObj.UpdateResourcesUIBasedOnBuyer();

        OnFinishProcessing.Invoke(); // tell time manager we're done
    }

    private void UpdateResourcesForBuyerA() {
        switch (TimeManager.Instance.CurrentTime.SeasonInYear) {
            case SeasonType.SUMMER: buyers[0].resourceA = Resource_SummerBonus; break;
            case SeasonType.FALL: buyers[0].resourceA = Resource_AutumnBonus; break;
            case SeasonType.WINTER: buyers[0].resourceA = Resource_WinterBonus; break;
            case SeasonType.SPRING: buyers[0].resourceA = Resource_SpringBonus; break;
        }
        buyers[0].resourceB = null;
    }
    
    private void UpdateResourcesForBuyerB() {
        if (buyers[1].resourceA == null) {
            buyers[1].resourceA = Resource_RareCrops[0];
        } else {
            // get the next resource in the list
            int newIndex = (Array.IndexOf(Resource_RareCrops, buyers[1].resourceA) + 1) % Resource_RareCrops.Length;
            buyers[1].resourceA = Resource_RareCrops[newIndex];
        }
        buyers[1].resourceB = null;
    }

    private void UpdateResourcesForBuyerC() {
        if (buyers[2].resourceA == null) {
            buyers[2].resourceA = Resource_Livestock[0];
        } else {
            // get the next resource in the list
            int newIndex = (Array.IndexOf(Resource_Livestock, buyers[2].resourceA) + 1) % Resource_Livestock.Length;
            buyers[2].resourceA = Resource_Livestock[newIndex];
        }
        buyers[2].resourceB = null;
    }

    private void UpdateResourcesForBuyerD() {
        List<ResourceSO> acceptablePool = new List<ResourceSO>(Resource_BuyerD);
        acceptablePool.Remove(buyers[0].resourceA); // exclude item on special from pool of options
        if (buyers[3].resourceA == null) {
            buyers[3].resourceA = Resource_BuyerD[0];
            buyers[3].resourceB = Resource_BuyerD[1];
        } else {
            // get the next resource in the list
            int newIndex = (Array.IndexOf(Resource_BuyerD, buyers[3].resourceA) + 2) % Resource_BuyerD.Length;
            buyers[3].resourceA = Resource_BuyerD[newIndex];
            newIndex = (Array.IndexOf(Resource_BuyerD, buyers[3].resourceB) + 2) % Resource_BuyerD.Length;
            buyers[3].resourceB = Resource_BuyerD[newIndex];
        }
    }


    protected override object[] CheckForMissingReferences() { return null; }
}
