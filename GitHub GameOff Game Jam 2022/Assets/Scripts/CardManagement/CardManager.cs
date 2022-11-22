using System;
using System.Collections.Generic;
using TimeManagement;
using UIManagement;
using UnityEngine;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;

/// <summary>
/// Handles all things related to the deck you draw cards from (as Im writing this its just a button).
/// E.g : Giving cards at the start of the match, or when the player draws from the deck.
/// </summary>
/// <author>CROSTZARD + Gino</author>
public class CardManager : ComputerPhaseStep
{
    // The singleton
    private static CardManager _instance = null;
    public static CardManager Instance
    {
        get
        {
            if (_instance == null)
                throw new Exception("CardManager singleton was called without CardManager being set up (check that CardManager is in the scene)");
            return _instance;
        }
        private set { _instance = value; }
    }

    public const int MAX_HANDCARD_AMOUNT = 5;

    public ActionCardSO[] cardList;

    [Header("Card Colour Schemes")]
    public Color buildingPrimary;
    public Color buildingSecondary;
    [Space(15)]
    public Color seedPrimary;
    public Color seedSecondary;
    [Space(15)]
    public Color livestockPrimary;
    public Color livestockSecondary;

    [Header("Discarding")]
    [SerializeField] private UIDiscardPanel discardPanel;

    [Header("Drawing deck")]

    [HideInInspector] public bool cardDiscardedThisTurn;

    /// <summary>
    /// Max amount of cards on a drawing deck.
    /// </summary>
    public int deckSize = 240;
    /// <summary>
    /// Amount of cards added to the drawing deck each turn.
    /// </summary>
    public int pileSize = 30;

    private int cardsDrawn, consideredDiscardIndex = -1;
   
    private List<ActionCardSO> playerHandcards = new List<ActionCardSO>();

    private List<ActionCardSO> cropCards = new List<ActionCardSO>();
    private List<ActionCardSO> buildCards = new List<ActionCardSO>();
    private List<ActionCardSO> liveStockCards = new List<ActionCardSO>();

    private List<ActionCardSO> shuffledCards = new List<ActionCardSO>();
    private List<ActionCardSO> piledCards = new List<ActionCardSO>();

    private new void Awake()
    {
        Assert.IsNull(_instance, "CardManager singleton is already set. (check there is only one CardManager in the scene)");
        Assert.IsNotNull(discardPanel, "Discard panel is not set. (check there is a DiscardPanel set in the inspector)");

        Instance = this;

        CheckNull();
        GetCardTypes();
        AddBalancedCards();
    }

    /// <summary>
    /// Gives the player an amount of cards.
    /// </summary>
    /// <param name="amount"> amount given </param>
    public void GiveCard(int amount = 1) 
    {
        if (cardsDrawn >= deckSize) 
            throw new ApplicationException("Drawing deck is out of cards!");

        for (int i = 0; i < amount; i++)
        {
            var drawnCard = shuffledCards[cardsDrawn];

            playerHandcards.Add(drawnCard);
            cardsDrawn++;
            UIMainPanel.Instance.DisplayCard(drawnCard);
        }
    }
    
    /// <summary>
    /// Assigns the card index and opens the discard UI panel
    /// </summary>
    /// <param name="cardIndex">The index of the card that may be discarded soon</param>
    public void ConsiderCardDiscard(int cardIndex)
    {
        if (cardIndex >= MAX_HANDCARD_AMOUNT)
            throw new ApplicationException("The selected card has an index bigger than the maximum allowed index.");
        else if (cardDiscardedThisTurn)
            throw new ApplicationException("A card has already been discarded this turn. This should be checked before" +
                "calling the function!");

        consideredDiscardIndex = cardIndex;

        var consideredCard = playerHandcards[cardIndex];

        FeedbackPanelManager.Instance.EnqueueDiscardCardInstantly(discardPanel, consideredCard);
        FeedbackPanelManager.Instance.InitiateInstantDisplayQueue();
    }
    
    /// <summary>
    /// If a card has been considered to be discarded, it will now discard it
    /// </summary>
    public void ConfirmDiscard()
    {
        if (consideredDiscardIndex == -1)
            throw new ApplicationException("Trying to confirm the discard action before the system is ready.");

        playerHandcards.RemoveAt(consideredDiscardIndex);
        discardPanel.HandleUIDiscard(consideredDiscardIndex);        

        cardDiscardedThisTurn = true;
        consideredDiscardIndex = -1;
    }
        
    // IMPORTANT: 

    // ADD THIS FUNCTION (ShuffleCards) EVERY TIME A TURN ENDS 
    /// <summary>
    /// Adds and shuffles cards appropriately to the deck
    /// </summary>
    /// 
    public void AddBalancedCards() 
    {
        if (shuffledCards.Count >= deckSize) return;

        int addAmount = (cardsDrawn + pileSize > deckSize) ? deckSize - cardsDrawn : pileSize;

        int x = 24;                 // crop card amount
        int y = Random.Range(3, 5); // build card amount
        int z = 6 - y;              // livestock card amount

        for (int i = 0; i < addAmount; i++)
        {
            if (i < x)
            {
                // Crop cards
                int rand = Random.Range(0, cropCards.Count);
                piledCards.Add(cropCards[rand]);
            }
            else if (i >= x && i < x + z && liveStockCards.Count > 0)
            {
                // Livestock cards
                int rand = Random.Range(0, liveStockCards.Count);
                piledCards.Add(liveStockCards[rand]);
            }
            else if (buildCards.Count > 0)
            {
                // BuildCards
                int rand = Random.Range(0, buildCards.Count);
                piledCards.Add(buildCards[rand]);
            }
        }

        RandomizeList(piledCards);
    }

    public bool MaximumHandcardLimitReached() => playerHandcards.Count > MAX_HANDCARD_AMOUNT;

    private void RandomizeList(List<ActionCardSO> list) 
    {
        int startSize = list.Count;

        for (int i = 0; i < startSize; i++)
        {
            int rand = Random.Range(0, list.Count);

            ActionCardSO item = list[rand];
            list.Remove(item);
            shuffledCards.Add(item);
        }

        list.Clear();
    }

    private void GetCardTypes() 
    {
        foreach (ActionCardSO card in cardList)
        {
            if (card is BuildingCard)
            {
                buildCards.Add(card);
                continue;
            }
            if (card is LivestockCard)
            {
                liveStockCards.Add(card);
                continue;
            }
            if (card is SeedCard)
            {
                cropCards.Add(card);
                continue;
            }
        }
    }

    private void CheckNull() 
    {
        Assert.IsNotNull(cardList, $"{ GetType().Name} missing required editor input 'availableCards'");
    }

    protected override object[] CheckForMissingReferences()
    {
        return new object[] { };
    }

    public override void StartProcessingForComputerPhase(bool isComputerPhaseDuringGameInit)
    {
        if (!isComputerPhaseDuringGameInit)
        {
            cardDiscardedThisTurn = false;
            print("Now here I should unlock all handcards!");
        }

        OnFinishProcessing.Invoke(); // tell time manager that this computer phase step is done
    }
}
