using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerData;
using TMPro;

public class MoneyDisplayer : MonoBehaviour
{

    public TextMeshProUGUI amountText;


    public void UpdateMoneyDisplay() 
    {
        if (amountText == null) return;
        if (PlayerDataManager.Instance != null) amountText.text = PlayerDataManager.Instance.AmountOfMoney.ToString();
    }

}
