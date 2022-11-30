using PlayerData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Buyer SO", menuName ="Shop/NewShop/BuyerSO")]
public class Shop_BuyerSO : ScriptableObject {

    public ResourceSO resourceA;
    public ResourceSO resourceB;

    public Sprite CharacterImage_Summary;
    public Sprite CharacterImage_Detail;
    public Sprite CharacterImage_DetailAfterPurchase;

    public string CharacterName;
    public string CharacterCity;
    public string CharacterSpeech_Summary;
    public string CharacterSpeech_Detail;
    public string CharacterSpeech_DetailAfterPurchase;

}
