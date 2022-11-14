using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;

/// <summary>
/// Handles all things related to the deck you draw cards from (as Im writing this its just a button).
/// E.g : Giving cards at the start of the match, or when the player draws from the deck.
/// </summary>
/// <author>CROSTZARD + Gino</author>
public class CardManager : MonoBehaviour
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

    [Header("Drawing deck")]

    /// <summary>
    /// Max amount of cards on a drawing deck.
    /// </summary>
    public int deckSize = 240;
    /// <summary>
    /// Amount of cards added to the drawing deck each turn.
    /// </summary>
    public int pileSize = 30;

    private int cardsDrawn;

    [HideInInspector] public List<GameObject> pooledCards = new List<GameObject>();
    private List<ActionCardSO> playerHandcards = new List<ActionCardSO>();

    private List<ActionCardSO> cropCards = new List<ActionCardSO>();
    private List<ActionCardSO> buildCards = new List<ActionCardSO>();
    private List<ActionCardSO> liveStockCards = new List<ActionCardSO>();

    private List<ActionCardSO> shuffledCards = new List<ActionCardSO>();
    private List<ActionCardSO> piledCards = new List<ActionCardSO>();

    private void Awake()
    {
        Assert.IsNull(_instance, "CardManager singleton is already set. (check there is only one CardManager in the scene)");
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
        {
            Debug.LogError("Drawing deck is out of cards!");
            return;
        }

        for (int i = 0; i < amount; i++)
        { 
            //GameObject card = pooledCards.Count > 0 ? GetPooledCard() : Instantiate(cardPrefab, cardContainer.transform);

            UIMainPanel.Instance.DisplayCard(shuffledCards[cardsDrawn]);
                        
            cardsDrawn++;
        }
    }

    private GameObject GetPooledCard() 
    {
        GameObject card = pooledCards[0];
        pooledCards.RemoveAt(0);

        card.SetActive(true);

        return card;    
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
        foreach (ActionCardSO c in cardList)
        {
            if (c is BuildingCard)
            {
                buildCards.Add(c);
                continue;
            }
            if (c is LivestockCard)
            {
                liveStockCards.Add(c);
                continue;
            }
            if (c is SeedCard)
            {
                cropCards.Add(c);
                continue;
            }
        }
    }

    private void CheckNull() 
    {
        Assert.IsNotNull(cardList, $"{ GetType().Name} missing required editor input 'availableCards'");
    }

}
