using PlayerData;
using TimeManagement;
using UnityEngine;


/// CROSTZARD (author)
/// <summary>
/// Handles selling items, and, in that process, calculates what price the city is going to pay for them. Is a single instance.
/// The sell button UI calls this class to sell to their current shop.
/// </summary>
/// 

public class ShopManager : ComputerPhaseStep
{

    public static ShopManager Instance;
    /// <summary>
    /// GameObject that holds all the cities
    /// </summary>
    public Transform cityHolder;

    public ResourceSO.SeasonBonus SeasonInBonus { get { return SeasonType2SeasonBonus(TimeManager.Instance.CurrentTime.SeasonInYear); }}

    private int costBonus = 2;

    private new void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {

            Debug.LogError("There is more than 1 Shop Manager in the scene!!!");
        }
    }

    private void Start()
    {
        Invoke("UpdateShop", 2);
    }


    /// <summary>
    /// Sell the resource the shop is buying
    /// </summary>
    /// <param name="shop"> Shop you are selling to</param>
    public void SellResource(Shop shop)
    {

        for (int i = 0; i < shop.itemAmount; i++) 
        {
            if (shop.Resources == null) return;
            if (shop.Resources.Count == 0) 
            {
                Debug.LogError("Shop's resources count 0. None available for buying"); 
                return;
            }
            
            ResourceSO resource = shop.Resources[i];
            int price = GetPrice(resource, shop);

            // Notify of quest update
            QuestManager.Instance.NotifyOfResourceSale(resource, price);

            PlayerDataManager dataManager = PlayerDataManager.Instance;
            if (dataManager.HasItemInInventory(resource))
            {
                dataManager.DecreaseInventoryItemAmount(resource, 1);
                dataManager.IncreaseMoneyAmount(price);
                shop.SoldItem();
            }
        }
    }


    /// <summary>
    /// Get the price of a specified resource. Price varies with shops so you can also specify one.
    /// </summary>
    /// <param name="resource">resource to check.</param>
    /// <param name="shop">in what shop (leave blank if none in specific)</param>
    /// <returns></returns>
    public int GetPrice(ResourceSO resource, Shop shop = null)
    {

        int basePrice = resource.basePrice;
        float multiplier = (shop == null) ? 1 : shop.CostMultiplier;

        // season checks for bonuses, etc.

        int price = (int)(basePrice * multiplier);

        if(resource.seasonBonus == SeasonInBonus) 
        {
            price = (int)(basePrice * multiplier + costBonus);
        }

        return price;
    }

    /// <summary>
    /// Updates the whole shop (resources, price)
    /// Call this every season change or when you want the shop to display new items.
    /// </summary>
    public void UpdateShop() 
    {
        foreach (Transform t in cityHolder) 
        {
            Shop shop = t.GetComponent<Shop>();
            shop.UpdateResource();
        } 
    }
    /// <summary>
    /// Interprets a seasonType enum as a seasonBonus enum (seasonBonus is an enum used in the ResourdSO's to determine what season they get a bonus on)  
    /// </summary>
    /// <param name="seasonType"></param>
    /// <returns></returns>
    public ResourceSO.SeasonBonus SeasonType2SeasonBonus(SeasonType seasonType) 
    { 
        switch (seasonType) 
        {
            case SeasonType.SUMMER: return ResourceSO.SeasonBonus.SUMMER;
            case SeasonType.FALL: return ResourceSO.SeasonBonus.FALL;
            case SeasonType.WINTER: return ResourceSO.SeasonBonus.WINTER;
            case SeasonType.SPRING: return ResourceSO.SeasonBonus.SPRING;
        }
        return ResourceSO.SeasonBonus.NONE; // This never happens because SEASONTYPE is always one of the upper four.
    }

    public override void StartProcessingForComputerPhase(bool isComputerPhaseDuringGameInit)
    {
        if (isComputerPhaseDuringGameInit)
        {
            UpdateShop();
        }

        OnFinishProcessing.Invoke();
    }

    protected override object[] CheckForMissingReferences()
    {
        return new object[0];
    }
}
