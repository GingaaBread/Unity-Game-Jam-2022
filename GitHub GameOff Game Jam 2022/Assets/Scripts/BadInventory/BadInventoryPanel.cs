using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using PlayerData;
using UnityEngine.Assertions;

public class BadInventoryPanel : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI textObj;

    private void Awake() {
        Assert.IsNotNull(textObj);
    }

    void Update()
    {
        if (PlayerDataManager.Instance == null)
            return;

        string text = "";
        text += $"Money: {PlayerDataManager.Instance.AmountOfMoney}\n";

        foreach(ResourceSO resource in PlayerDataManager.Instance.GetResourcesContained()) {
            text += $"{resource.name.ToLower()}: {PlayerDataManager.Instance.GetInventoryItemAmount(resource)}\n";
        }



        textObj.text = text;
    }
}
