using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Assertions;

public class ShopPricePanel : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI textObj;

    private void Awake() {
        Assert.IsNotNull(textObj);
    }

    public void setText(string s) => textObj.text = s;

}
