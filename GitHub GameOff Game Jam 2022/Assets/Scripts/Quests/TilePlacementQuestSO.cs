using PlayerData;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Assertions;

[CreateAssetMenu(fileName = "New Tile Placement SO", menuName = "Quests/TilePlacementSO")]
public class TilePlacementQuestSO : AbstractQuestSO {

    [SerializeField] public ActionCardSO[] targetCards;
    [SerializeField] public int[] targetQuantity;
    private int[] actualQuantity; // not serialized because we don't want to save these outside runtime

    public void OnEnable() {
        Assert.IsTrue(targetCards.Length == targetQuantity.Length, $"{this?.name} must have same number of targetQuantity as targetCards");
    }

    public override string GetQuestAsSentence() {
        string s = "Place";
        for(int i = 0; i < targetCards.Length; i++) {
            if (i > 0)
                s+= " and";
            s += $" {targetQuantity[i]} {targetCards[i].name.ToLower()}";
        }
        return s;
    }

    public override string GetStatusAsSentence() {
        return string.Join(", ", actualQuantity);
    }

    public override void ResetActualsCounters() {
        actualQuantity = new int[targetCards.Length];
        OnUpdate.Invoke();
    }

    public override float GetPercentageCompleted() {
        int numerator = 0;
        int denominator = 0;
        for (int i = 0; i < targetCards.Length; i++) {
            numerator += actualQuantity[i];
            denominator += targetQuantity[i];
        }
        float result = 100f * ((float)numerator/denominator);

        return result;
    }

    public override void NotifyOfResourceCollected(ResourceSO resource, int countCollected) {
        // this class isn't interested in these notifications
    }
    public override void NotifyOfResourceSale(ResourceSO resource, int MoneyEarnedFromSale) {
        // this class isn't interested in these notifications
    }

    public override void NotifyOfTilePlaced(ActionCardSO card) {
        Assert.IsNotNull(actualQuantity);
        Assert.IsTrue(targetCards.Length == actualQuantity.Length, $"{this?.name} must have same number of actualQuantity as targetCards");
        for (int i = 0; i < targetCards.Length; i++) {
            if (targetCards[i] == card) {
                actualQuantity[i] += 1;
                OnUpdate.Invoke();
                return;
            }
        }
    }

}
