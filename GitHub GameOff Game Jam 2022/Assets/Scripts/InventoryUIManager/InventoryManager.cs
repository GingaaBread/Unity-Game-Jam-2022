using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerData;



public class InventoryManager : MonoBehaviour
{

    public static InventoryManager Instance;

    /// <summary>
    /// Resource Icon prefab. This gets instantiated each time a new resource icon is needed.
    /// </summary>
    public GameObject resourcePrefab;
    
    /// <summary>
    /// GameObject that holds all other icons.
    /// </summary>
    public Transform resourceHolder;

    public MoneyDisplayer[] moneyDisplayers;

    /// <summary>
    /// This dictionary is a quick way to access each individual icon. The key is just the resource's name.
    /// </summary>
    private Dictionary<string, ResourceDisplayer> resourceIcons = new Dictionary<string, ResourceDisplayer>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else 
        {
            Debug.Log("There is more than 1 InventoryManager in the scene!!!");
        }
    }

    void Start()
    {

    }

    void Update()
    {
        
    }

    /// <summary>
    /// Update the icons shown in the shop UI that represent the items in your inventory.
    /// </summary>
    /// This function gets called everytime the inventory in PlayerDataManager gets edited or altered (like when something is added or removed from it).
    public void UpdateInventoryIcons(ResourceSO resource) 
    {
        PlayerDataManager dataManager = PlayerDataManager.Instance;

        // If theres more than 0 items in the inv, update the amount to show the numbers accordingly.
        if (dataManager.GetInventoryItemAmount(resource) > 0) 
        {

            // If we already have instantiated an icon for this resource, then just activate it.
            if (resourceIcons.ContainsKey(resource.name)) 
            {
                resourceIcons[resource.name].gameObject.SetActive(true);
                resourceIcons[resource.name].SetUpResourceIcon(resource);
            }
            else // Else just create a new one and add it to the dictionary.
            {

                GameObject icon = Instantiate(resourcePrefab, resourceHolder);

                ResourceDisplayer displayer = icon.GetComponent<ResourceDisplayer>();
                displayer.SetUpResourceIcon(resource);

                resourceIcons.Add(resource.name, displayer);

            }
        }
        else 
        {
            // else, just hide the box (displayer) 
            if (resourceIcons.ContainsKey(resource.name)) 
            {
                resourceIcons[resource.name].gameObject.SetActive(false);
            }
        }
        
    }

    public void UpdateInventoryMoneyDisplay() 
    {

        if (moneyDisplayers == null) 
        {
            return;
        }

        for (int i = 0; i < moneyDisplayers.Length; i++) 
        { 
        
            moneyDisplayers[i].UpdateMoneyDisplay();
        }
    }

}
