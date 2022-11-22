using PlayerData;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Assertions;

[CreateAssetMenu(fileName = "New Money From Sales SO", menuName = "Quests/MoneyFromSalesSO")]
public class MoneyFromSalesQuestSO : AbstractQuestSO {

    [SerializeField] public ResourceSO[] targetSaleItems;
    [SerializeField] [Range(1, 9999)] public int targetTotalMoney;
    private int actualTotalMoney; // not serialized because we don't want to save these outside runtime

    public override string GetQuestAsSentence() {
        string s = $"Earn {targetTotalMoney} by selling";
        for(int i = 0; i < targetSaleItems.Length; i++) {
            if (i > 0)
                s += ",";
            if (i == targetSaleItems.Length-1)
                s += " and";
            s += $" {targetSaleItems[i].name.ToLower()}";
        }
        return s;
    }

    public override string GetStatusAsSentence() {
        return actualTotalMoney.ToString();
    }

    public override void ResetActualsCounters() {
        actualTotalMoney = 0;
        OnUpdate.Invoke();
    }

    public override float GetPercentageCompleted() {
        int numerator = actualTotalMoney;
        int denominator = targetTotalMoney;
        float result = 100f * ((float)numerator/denominator);
        return result;
    }

    public override void NotifyOfResourceCollected(ResourceSO resource, int countCollected) {
        // this class isn't interested in these notifications
    }

    public override void NotifyOfTilePlaced(ActionCardSO card) {
        // this class isn't interested in these notifications
    }

    public override void NotifyOfResourceSale(ResourceSO resource, int MoneyEarnedFromSale) {
        for (int i = 0; i < targetSaleItems.Length; i++) {
            if (targetSaleItems[i] == resource) {
                actualTotalMoney += MoneyEarnedFromSale;
                OnUpdate.Invoke();
                return;
            }
        }
    }
}
