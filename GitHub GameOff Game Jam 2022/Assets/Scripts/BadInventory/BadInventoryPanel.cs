using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using PlayerData;
using UnityEngine.Assertions;

public class BadInventoryPanel : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI textObj;

    /// <summary>
    /// note from CROSTZARD
    /// I've re-added the MoneyDisplayer script (Check InventoryManager script to see how it works)
    /// MoneyDisplayer is being used again for displaying the player's coin amount on the screen
    /// We can get rid of this script
    /// </summary>


    private void Awake() {
        Assert.IsNotNull(textObj);
    }

    void Update()
    {
        if (textObj == null) return;

        if (PlayerDataManager.Instance == null)
            return;

        string text = "";
        text += $"Money: {PlayerDataManager.Instance.AmountOfMoney}\n";

        foreach(ResourceSO resource in PlayerDataManager.Instance.GetResourcesContained()) {
            int amount = PlayerDataManager.Instance.GetInventoryItemAmount(resource);
            if(amount > 0)
                text += $"{resource.name.ToLower()}: {amount}\n";
        }



        textObj.text = text;
    }
}
