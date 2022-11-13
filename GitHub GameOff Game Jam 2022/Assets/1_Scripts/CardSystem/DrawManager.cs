using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

public class DrawManager : MonoBehaviour, IPointerDownHandler
{


    // CROSTZARD

    //Summary: Handles all things related to the deck you draw cards from (as Im writing this its just a button).
    // E.g : Giving cards at the start of the match, or when the player draws from the deck.

    public static DrawManager Instance; // Singleton

    public ActionCardSO[] cardList;

    public DeckManager manager;
    public GameObject cardPrefab;

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

    float cooldown = 0.2f;
    float timer;

    [HideInInspector] public List<GameObject> pooledCards = new List<GameObject>();

    private List<ActionCardSO> cropCards = new List<ActionCardSO>();
    private List<ActionCardSO> buildCards = new List<ActionCardSO>();
    private List<ActionCardSO> liveStockCards = new List<ActionCardSO>();

    private List<ActionCardSO> shuffledCards = new List<ActionCardSO>();
    private List<ActionCardSO> piledCards = new List<ActionCardSO>();

    public void Start()
    {
        CheckNull();
        GetCardTypes();
        AddBalancedCards();

        if (Instance == null) Instance = this;
        else 
        {
            Destroy(this);
            Debug.LogError("Tried to set a second Instance of DrawManager");
        }

    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            Debug.Log("balanced cards!");
            AddBalancedCards();
        }

        timer += Time.deltaTime;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (timer < cooldown) return;
        if (manager.CardsOnDeck >= 5) return;

        GiveCard();

        timer = 0;
    }

    /// <summary>
    /// Gives the player an amount of cards.
    /// </summary>
    /// <param name="amount"> amount given </param>

    public void GiveCard(int amount = 1) 
    {

        if(cardsDrawn >= deckSize) 
        {
            Debug.LogError("Drawing deck is out of cards!");
            return;
        }

        for (int i = 0; i < amount; i++)
        { 
            GameObject card = pooledCards.Count > 0 ? GetPooledCard() : Instantiate(cardPrefab, manager.transform);

            AssignCardData(card.transform);

            manager.AddCardToList(card.transform.GetComponent<CardScript>());
            
            cardsDrawn += 1;

        }
    }

    private void AssignCardData(Transform card) 
    {
        CardScript cardScript = card.GetComponent<CardScript>();
        if (cardScript == null) return;

        cardScript.Card = shuffledCards[cardsDrawn];
        Debug.Log($"given card: {shuffledCards[cardsDrawn].name} index: {cardsDrawn}");
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
            else if (i >= x && i < x + z )
            {
                // Livestock cards
                int rand = Random.Range(0, liveStockCards.Count);
                piledCards.Add(liveStockCards[rand]);
            }
            else
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
        Assert.IsNotNull(cardPrefab, $"{ GetType().Name} missing required editor input 'cardPrefab'");
        Assert.IsNotNull(manager, $"{ GetType().Name} missing required editor input 'manager'");
        Assert.IsNotNull(cardList, $"{ GetType().Name} missing required editor input 'availableCards'");
    }

}
