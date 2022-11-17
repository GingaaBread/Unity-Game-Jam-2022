using System;
using UnityEngine;
using UnityEngine.Assertions;

public class UIMainPanel : MonoBehaviour
{
    // The singleton
    private static UIMainPanel _instance = null;
    public static UIMainPanel Instance
    {
        get
        {
            if (_instance == null)
                throw new Exception("UIMainPanel singleton was called without UIMainPanel being set up (check that UIMainPanel is in the scene)");
            return _instance;
        }
        private set { _instance = value; }
    }

    [SerializeField] private GameObject cardContainer;
    [SerializeField] private GameObject cardPrefab;

    private void Awake()
    {
        Assert.IsNull(_instance, "UIMainPanel singleton is already set. (check there is only one UIMainPanel in the scene)");
        Instance = this;
    }

    public void DisplayCard(ActionCardSO cardToDisplay)
    {
        if (CardManager.Instance.MaximumHandcardLimitReached())
            throw new ArgumentException("Trying to display a card despite having reached the maximum handcard limit.");

        var newCard = Instantiate(cardPrefab, cardContainer.transform).GetComponent<UICardPanel>();

        newCard.CardToDisplay = cardToDisplay;
        newCard.Render();
    }

    public void LockAllHandcards()
    {
        for (int i = 0; i < cardContainer.transform.childCount; i++)
        {
            var card = cardContainer.transform.GetChild(i).GetComponent<UICardPanel>();
            card.LockDiscardButton();
        }
    }

    public void DestroyCard(int cardIndex)
    {
        Destroy(cardContainer.transform.GetChild(cardIndex).gameObject);
    }
}
