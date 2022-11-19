using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerData;


/// <summary>
/// 
/// </summary>


public class InventoryManager : MonoBehaviour
{

    public static InventoryManager Instance;

    public GameObject resourcePrefab;
    public Transform resourceHolder;
    public Transform resourceStartPosition;

    private Vector3 startPosition;

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
        startPosition = resourceStartPosition.localPosition;
    }

    void Update()
    {
        
    }

    /// <summary>
    /// Update the icons shown in the shop UI that represent the items in your inventory.
    /// </summary>

    public void UpdateInventoryIcons(ResourceSO resource) 
    { 
        // to do: delete icons when we no longer have an item
        // update amount after something is added
        

    }

    /// <summary>
    /// Adds a PlayerDataManager inventory item to the graphical inventory UI as an icon
    /// </summary>
    /// <param name="resource">Resource added to the graphical UI</param>
    public void AddInventoryIcon(ResourceSO resource) 
    {
        GameObject icon = Instantiate(resourcePrefab, resourceHolder);

        icon.transform.GetComponent<ResourceDisplayer>().SetUpResourceIcon(resource);
        PositionIcon(icon.transform);


    }

    private void PositionIcon(Transform icon) 
    {
        int count = resourceHolder.childCount;

        Vector3 pos = startPosition;
        pos.y *= count;
        icon.transform.localPosition = pos;

    }

}
