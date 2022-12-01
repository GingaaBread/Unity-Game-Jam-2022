using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PlayerData;
using UnityEngine.EventSystems;

public class ResourceDisplayer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public Image icon;
    public TextMeshProUGUI amount;

    private ResourceSO displayedResource;

    
    //public TextMeshProUGUI resourceName;

    /// <summary>
    /// Sets itself up. Changes their text and image according to the ResourceSO its representing.
    /// </summary>
    /// <param name="resource"></param>
    public void SetUpResourceIcon(ResourceSO resource) 
    {
        //resourceName.text = resource.name;
        amount.text = "x" + PlayerDataManager.Instance.GetInventoryItemAmount(resource).ToString();
        icon.sprite = resource.iconSprite;
        displayedResource = resource;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (InventoryManager.Instance == null) return;
        if (displayedResource == null) return;
        RectTransform rect = InventoryManager.Instance.nameRect as RectTransform;

        rect.transform.position = transform.position + Vector3.right * 125 + Vector3.up * 10;

        rect.gameObject.SetActive(true);

        TextMeshProUGUI tm = rect.GetComponentInChildren<TextMeshProUGUI>();
        if (tm != null) tm.text = displayedResource.name;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (InventoryManager.Instance == null) return;

        InventoryManager.Instance.nameRect.gameObject.SetActive(false);
    }
}
