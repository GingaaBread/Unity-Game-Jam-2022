using FMODUnity;
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

    [Header("Debug")]
    [SerializeField] private bool DebugMode;

    [Header("Audio")]
    [SerializeField] private EventReference cardReceivedSoundEvent;

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
    [SerializeField] public ActionCardSO[] requiredCardsInStartingHand;

    [Header("Drawing deck")]
    [HideInInspector] public bool cardDiscardedThisTurn;


    /// <summary>
    /// Max amount of cards on a drawing deck.
    /// </summary>
    public int deckSize = 240;

    private int consideredDiscardIndex = -1;
   
    private List<ActionCardSO> playerHandcards = new List<ActionCardSO>();
    private List<UICardPanel> cardPanels = new List<UICardPanel>();

    private List<ActionCardSO> cropCards = new List<ActionCardSO>();
    private List<ActionCardSO> buildCards = new List<ActionCardSO>();
    private List<ActionCardSO> liveStockCards = new List<ActionCardSO>();

    private Queue<ActionCardSO> deck;

    private new void Awake()
    {
        Assert.IsNull(_instance, "CardManager singleton is already set. (check there is only one CardManager in the scene)");
        Assert.IsNotNull(discardPanel, "Discard panel is not set. (check there is a DiscardPanel set in the inspector)");

        Instance = this;

        CheckNull();
        GetCardTypes();
    }

    /// <summary>
    /// Gives the player an amount of cards.
    /// </summary>
    /// <param name="amountOfCardsToGive"> amount given </param>
    public void GiveCard(int amountOfCardsToGive = 1) 
    {
        Assert.IsNotNull(deck, "can't give player cards before card manager has been initialized");

        if (deck.Count < amountOfCardsToGive) 
            throw new ApplicationException("Drawing deck is out of cards!");

        for (int i = 0; i < amountOfCardsToGive; i++)
        {
            ActionCardSO drawnCard = deck.Dequeue();
            playerHandcards.Add(drawnCard);

            UICardPanel newCard = UIMainPanel.Instance.DisplayCard(drawnCard);
            cardPanels.Add(newCard);

            RuntimeManager.PlayOneShot(cardReceivedSoundEvent);
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
            throw new ApplicationException("A card has already been discarded this turn. This should be checked before calling the function!");
        
        CardPlayManager.Instance.ResetCurrentPlay();

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
        cardPanels.RemoveAt(consideredDiscardIndex);
        
        discardPanel.HandleUIDiscard(consideredDiscardIndex);        

        cardDiscardedThisTurn = true;
        consideredDiscardIndex = -1;
    }

    public void RemoveCardOnUse(int cardIndex)
    {
        playerHandcards.RemoveAt(cardIndex);
        cardPanels.RemoveAt(cardIndex);
        UIMainPanel.Instance.DestroyCard(cardIndex);
    }

    public void RemoveCardOnUse(UICardPanel currentUIPanel)
    {
        Assert.IsFalse(currentUIPanel.isDetailedCard);

        UIMainPanel.Instance.HideDetailedCard();
        playerHandcards.Remove(currentUIPanel.CardToDisplay);
        cardPanels.Remove(currentUIPanel);
        UIMainPanel.Instance.DestroyCard(currentUIPanel.GetCardIndex());
    }
        
    /// <summary>
    /// Adds and shuffles cards appropriately to the deck
    /// </summary>
    /// 
    private void InitializeDeckWithCards() {
        List<ActionCardSO> tempDeck = new List<ActionCardSO>();

        if (requiredCardsInStartingHand!= null) {
            tempDeck.AddRange(requiredCardsInStartingHand);
        }

        while (tempDeck.Count < deckSize) {
            List<ActionCardSO> balancedPile = GeneratePileOfCards();
            List<ActionCardSO> randomizedBalancedPile = MakeRandomizedCopy(balancedPile);
            tempDeck.AddRange(randomizedBalancedPile);

            if (DebugMode) {
                Debug.Log("########## Generated pile: ");
                Debug.Log("BalancedPile: " + GenerateCollectionOfCardTypeLogString(balancedPile));
                Debug.Log("RandomizedTo: " + GenerateCollectionOfCardTypeLogString(randomizedBalancedPile));
            }
        }

        deck = new Queue<ActionCardSO>(tempDeck);
        if (DebugMode) Debug.Log($"########## Finished making deck of {deck.Count} cards");
    }

    private string GenerateCollectionOfCardTypeLogString(List<ActionCardSO> cards) {
        string s = "";
        foreach(ActionCardSO card in cards) {
            if (card is LivestockCard)  s += "L";
            if (card is SeedCard)       s += "S";
            if (card is BuildingCard)   s += "B";
        }
        return s;
    }

    private List<ActionCardSO> GeneratePileOfCards() {

        List<ActionCardSO> pile = new List<ActionCardSO>();

        // Ratios per pile
        int buildCardsToAdd = 6;
        int livestockCardsToAdd = Random.Range(2, 3);
        int cropCardsToAdd = 24 - livestockCardsToAdd;

        pile.AddRange(MakeRandomSelectionFromList(cropCards, cropCardsToAdd));
        pile.AddRange(MakeRandomSelectionFromList(buildCards, buildCardsToAdd));
        pile.AddRange(MakeRandomSelectionFromList(liveStockCards, livestockCardsToAdd));

        return pile;
    }

    public bool MaximumHandcardLimitReached() => playerHandcards.Count > MAX_HANDCARD_AMOUNT;

    private static List<ActionCardSO> MakeRandomSelectionFromList(List<ActionCardSO> listToSelectFrom, int desiredListSize) {
        List<ActionCardSO> selection = new List<ActionCardSO>();
        while (desiredListSize > 0) { 
            selection.Add(listToSelectFrom[Random.Range(0, listToSelectFrom.Count)]);
            desiredListSize--; 
        }
        return selection;
    }

    private static List<ActionCardSO> MakeRandomizedCopy(List<ActionCardSO> list) 
    {
        List<ActionCardSO> inputListCopy = new List<ActionCardSO>(list);
        List<ActionCardSO> randomizedList = new List<ActionCardSO>();

        while (inputListCopy.Count > 0) {
            ActionCardSO randomCard = inputListCopy[Random.Range(0, inputListCopy.Count)];
            randomizedList.Add(randomCard);
            inputListCopy.Remove(randomCard);
        }
        return randomizedList;
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
        cardDiscardedThisTurn = false;

        if (isComputerPhaseDuringGameInit) {
            InitializeDeckWithCards();
        } else {
            foreach (var cardPanel in cardPanels)
                cardPanel.UnlockDiscardButton();
            GiveCard(5 - playerHandcards.Count);
        }

        OnFinishProcessing.Invoke(); // tell time manager that this computer phase step is done
    }
}
