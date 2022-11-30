using PlayerData;
using System;
using UnityEngine;
using UnityEngine.Assertions;

[CreateAssetMenu(fileName = "New Quest", menuName = "Quests/TilePlacementQuestGoal")]
public class TilePlacementQuestSO : AbstractQuestSO {

    public ActionCardSO[] targetCards;
    public int[] targetQuantity;
    public OperatorType operatorType;

    private int[] actualQuantity; // not serialized because we don't want to save these outside runtime

    public void OnEnable() {
        Assert.IsNotNull(targetCards);
        Assert.IsTrue(targetCards.Length == targetQuantity.Length, $"{this?.name} must have same number of targetQuantity as targetCards");
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
            if (targetCards[i] == card && targetQuantity[i] > actualQuantity[i]) {
                actualQuantity[i] += 1;

                if (QuestIsCompleted())
                {
                    OnCompletion.Invoke();
                }

                OnUpdate.Invoke();
                return;
            }
        }
    }

    private bool QuestIsCompleted()
    {
        for (int i = 0; i < targetQuantity.Length; i++)
        {
            if (targetQuantity[i] > actualQuantity[i])
                return false;
        }

        return true;
    }

}
