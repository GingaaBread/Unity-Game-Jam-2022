using PlayerData;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quest", menuName = "Quests/MoneyFromSalesQuestGoal")]
public class MoneyFromSalesQuestSO : AbstractQuestSO {

    [SerializeField] public ResourceSO[] targetSaleItems;
    [SerializeField] [Range(1, 9999)] public int targetTotalMoney;
    [SerializeField] public bool fromAnything;
    private int actualTotalMoney; // not serialized because we don't want to save these outside runtime
    
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
        if (fromAnything)
        {
            actualTotalMoney += MoneyEarnedFromSale;

            if (targetTotalMoney <= actualTotalMoney)
            {
                OnCompletion.Invoke();
            }

            OnUpdate.Invoke();
            return;
        }
        else
        { 
            for (int i = 0; i < targetSaleItems.Length; i++) {
                if (targetSaleItems[i] == resource) {
                    actualTotalMoney += MoneyEarnedFromSale;

                    if (targetTotalMoney <= actualTotalMoney)
                    {
                        OnCompletion.Invoke();
                    }

                    OnUpdate.Invoke();
                    return;
                }
            }
        }
    }

}
