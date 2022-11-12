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

    public int deckSize = 240;
    private int amountPerTurn = 20;

    public int pileAmount = 8;
    private int currentAmount;

    float cooldown = 0.2f;
    float timer;

    [HideInInspector] public List<GameObject> pooledCards = new List<GameObject>();

    private List<ActionCardSO> cropCards = new List<ActionCardSO>();
    private List<ActionCardSO> buildCards = new List<ActionCardSO>();
    private List<ActionCardSO> passiveCards = new List<ActionCardSO>();

    private List<ActionCardSO> shuffledCards = new List<ActionCardSO>();
    private List<ActionCardSO> piledCards = new List<ActionCardSO>();

    public void Start()
    {
        CheckNull();

        ShuffleCards();

        if (Instance == null) Instance = this;
        else 
        {
            Destroy(this);
            Debug.LogError("Tried to set a second Instance of DrawManager");
        }

    }

    public void Update()
    {
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

        if(currentAmount <= 0) 
        {
            Debug.LogError("Drawing deck is out of cards!");
            return;
        }

        for (int i = 0; i < amount; i++)
        { 
            GameObject card = pooledCards.Count > 0 ? GetPooledCard() : Instantiate(cardPrefab, manager.transform);

            AssignCardData(card.transform);

            manager.AddCardToList(card.transform.GetComponent<CardScript>());

            currentAmount -= 1;

        }
    }

    private void AssignCardData(Transform card) 
    {
        CardScript cardScript = card.GetComponent<CardScript>();
        if (cardScript == null) return;

        int randInt = Random.Range(0, shuffledCards.Count);

        cardScript.Card = shuffledCards[randInt];
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
    /// Adds and shuffles cards appropiately to the deck
    /// </summary>
    public void ShuffleCards() 
    {

        currentAmount += amountPerTurn;

        GetCardTypes();


        int x = 24;                 // crop card amount
        int y = Random.Range(3, 5); // build card amount
        int z = 6 - y;              // livestock card amount

        for (int i = 0; i < 30; i++)
        {
            if (i < x)
            {
                // Crop cards
                int rand = Random.Range(0, cropCards.Count);
                shuffledCards.Add(cropCards[rand]);
            }
            else if (i >= x && i < x + z )
            {
                // Livestock cards
                int rand = Random.Range(0, passiveCards.Count);
                shuffledCards.Add(passiveCards[rand]);
            }
            else
            {
                // BuildCards
                int rand = Random.Range(0, buildCards.Count);
                shuffledCards.Add(buildCards[rand]);
            }
            

        }
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
            if (c is PassiveCard)
            {
                passiveCards.Add(c);
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
