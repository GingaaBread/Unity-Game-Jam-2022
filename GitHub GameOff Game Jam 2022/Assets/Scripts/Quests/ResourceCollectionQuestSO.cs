using PlayerData;
using UnityEngine;
using UnityEngine.Assertions;

[CreateAssetMenu(fileName = "New Quest", menuName = "Quests/ResourceCollectionQuestGoal")]
public class ResourceCollectionQuestSO : AbstractQuestSO {

    [SerializeField] public ResourceSO[] targetResources;
    [SerializeField] public int[] targetQuantity;

    private int[] actualQuantity; // not serialized because we don't want to save these outside runtime

    public void OnEnable() {
        Assert.IsTrue(targetResources.Length == targetQuantity.Length, $"{this.name} must have same number of targetQuantity as targetResources");
    }

    public override string GetStatusAsSentence() {
        return string.Join(", ", actualQuantity);
    }

    public override void ResetActualsCounters() {
        actualQuantity = new int[targetResources.Length];
        OnUpdate.Invoke();
    }

    public override float GetPercentageCompleted() {
        int numerator = 0;
        int denominator = 0;
        for (int i = 0; i < targetResources.Length; i++) {
            numerator += actualQuantity[i];
            denominator += targetQuantity[i];
        }
        float result = 100f * ((float)numerator/denominator);

        return result;
    }

    public override void NotifyOfResourceCollected(ResourceSO resource, int countCollected) {
        Assert.IsNotNull(actualQuantity);
        Assert.IsTrue(targetResources.Length == actualQuantity.Length, $"{this.name} must have same number of actualQuantity as targetResources");

        for (int i = 0; i < targetResources.Length; i++) {
            if(targetResources[i] == resource) {
                actualQuantity[i] += countCollected;

                if (actualQuantity[i] >= targetQuantity[i])
                {
                    actualQuantity[i] = targetQuantity[i];
                }

                CheckIfIsDone();
                OnUpdate.Invoke();
                return;
            }
        }
    }


    private void CheckIfIsDone()
    {
        bool isDone = true;
        for (int i = 0; i < targetResources.Length; i++)
        {
            if (actualQuantity[i] < targetQuantity[i])
            {
                isDone = false;
            }
        }

        if (isDone)
        {
            OnCompletion.Invoke();
        }
    }
    public override void NotifyOfTilePlaced(ActionCardSO card) {
        // this class isn't interested in these notifications
    }

    public override void NotifyOfResourceSale(ResourceSO resource, int MoneyEarnedFromSale) {
        // this class isn't interested in these notifications
    }
}
