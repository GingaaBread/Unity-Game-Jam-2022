using System;
using UnityEngine;
using UnityEngine.Assertions;

public class UIMainPanel : MonoBehaviour
{
    [Header("Debug Flags")]
    public bool shouldLockDiscardButtonsForCards = true;
    public float detailedCardYPosition;

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
    [SerializeField] private UICardPanel detailedCardPanel;
    [SerializeField] private RectTransform detailedCardPanelRectTransform;
    [SerializeField] private RectTransform detailedCardPaddingLeftRectTransform;
    [SerializeField] private RectTransform detailedCardPaddingRightRectTransform;

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
        if (shouldLockDiscardButtonsForCards)
        {
            for (int i = 0; i < cardContainer.transform.childCount; i++)
            {
                var card = cardContainer.transform.GetChild(i).GetComponent<UICardPanel>();
                card.LockDiscardButton();
            }
        }
    }

    public void DestroyCard(int cardIndex)
    {
        Destroy(cardContainer.transform.GetChild(cardIndex).gameObject);
    }

    public void DisplayDetailedCard(UICardPanel cardPanel, int siblingIndex)
    {
        detailedCardPanel.gameObject.SetActive(true);

        ApplyPaddingDimensions(cardContainer.transform.childCount, siblingIndex);

        detailedCardPanel.CardToDisplay = cardPanel.CardToDisplay;
        detailedCardPanel.Render();
    }

    public void HideDetailedCard()
    {
        detailedCardPanel.CardToDisplay = null;
        detailedCardPanel.gameObject.SetActive(false);
    }

    private void ApplyPaddingDimensions(int handcardAmount, int selectedIndex)
    {
        float cardWidth = 150f, gapWidth = 15f;

        int cardsLeft = selectedIndex, cardsRight = handcardAmount - (selectedIndex + 1);

        float paddingLeft = cardsLeft * cardWidth;
        if (cardsLeft - 1 >= 0)
        {
            paddingLeft += (cardsLeft - 1) * gapWidth; 
        }
        SetPaddingLeft(paddingLeft);

        float paddingRight = cardsRight * cardWidth;
        if (cardsRight - 1 >= 0)
        {
            paddingRight += (cardsRight - 1) * gapWidth;
        }
        SetPaddingRight(paddingRight);
    }

    private void SetPaddingLeft(float paddingLeft)
    {
        detailedCardPaddingLeftRectTransform.sizeDelta = new Vector2(paddingLeft, 5f);
    }

    private void SetPaddingRight(float paddingRight)
    {
        detailedCardPaddingRightRectTransform.sizeDelta = new Vector2(paddingRight, 5f);
    }
}
