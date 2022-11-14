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

    [SerializeField] private GameObject[] handcardBank;
    [SerializeField] private UICardPanel[] handcards;

    private int poolIndex;

    private void Awake()
    {
        Assert.IsNull(_instance, "UIMainPanel singleton is already set. (check there is only one UIMainPanel in the scene)");
        Instance = this;
    }

    public void DisplayCard(ActionCardSO cardToDisplay)
    {
        print($"Displaying Card: {cardToDisplay.cardTitle}");

        if (poolIndex >= CardManager.MAX_HANDCARD_AMOUNT)
            throw new ArgumentException("Trying to display a card despite having reached the maximum handcard limit");

        handcardBank[poolIndex].SetActive(true);
        handcards[poolIndex].CardToDisplay = cardToDisplay;
        handcards[poolIndex].Render();

        poolIndex++;
    }
}
