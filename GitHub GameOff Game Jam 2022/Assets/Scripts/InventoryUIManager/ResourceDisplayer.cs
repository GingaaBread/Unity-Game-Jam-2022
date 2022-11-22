using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PlayerData;

public class ResourceDisplayer : MonoBehaviour
{

    public Image icon;
    public TextMeshProUGUI amount;
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

    }

}
