using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class DeckManager : MonoBehaviour
{
    // CROSTZARD

    //Summary: Handles all things related to your OWN deck.
    //E.g: Playing cards or selecting cards.



    public static DeckManager Instance;

    // Card events
    public delegate void CardEvents();
    public event CardEvents OnDeckUpdate;

    // So, the player in order to play a card has to click somewhere on the screen, except inside this transform.
    public RectTransform deckUIBounds;

    // Where do cards start appearing from? I'll position cards according to this position.
    public Transform deckStartPoint;

    /// <summary>
    /// Amount of cards on hand
    /// </summary>
    public int CardsOnDeck { get { return currentCards.Count; } }

    // Selected card
    CardScript selected;
    public CardScript Selected { get { return selected; } set { selected = value; } }

    // Current cards on my hand. I use this for positioning cards
    [HideInInspector]
    private List<CardScript> currentCards = new List<CardScript>();

    float playCooldown = 0.3f;
    float timer;


    private void Start()
    {
        CheckNull();

        if (Instance == null) Instance = this;
        else
        {
            Destroy(this);
            Debug.LogError("Tried to set a second Instance of DeckManager");
        }

        EventSubscription();
    }

    void Update()
    {
        
        PlayCard();
    }

    private void PlayCard()
    {
        timer += Time.deltaTime;

        if (Input.GetMouseButtonDown(0))
        {

            if (timer < playCooldown) return;
            if (deckUIBounds == null) return;
            if (selected == null) return;

            Vector3 pos = Input.mousePosition;
            pos.z = 0;

            if (!RectTransformUtility.RectangleContainsScreenPoint(deckUIBounds, pos))
            {
                selected.Card.Action();
                OnDeckUpdate.Invoke();
            }
            else DeselectCard();


        }
    }

    public void SelectCard(CardScript card) 
    {
        timer = 0;
        DeselectCard();

        if (card.Animator == null) return;

        selected = card;
        card.Animator.Play("Card_Selected");
    }   

    public void DeselectCard() 
    {
        if (selected != null)
        {
            selected.Animator.Play("Card_Deselected");
            selected = null;
        }
    }

    public void DiscardCard(CardScript card) 
    {
        card.gameObject.SetActive(false);
        currentCards.Remove(card);
        DrawManager.Instance.pooledCards.Add(card.gameObject);

        OnDeckUpdate.Invoke();
    }

    // Gets called everytime the deck is updated or altered in some way. Like when a card is drawn or played.
    // dictated by the event OnCardUpdate
    private void CardPositioning() 
    { 
        for (int i = 0; i < currentCards.Count; i++) 
        {

            Vector3 startPos = deckStartPoint.localPosition;

            float Xpos = startPos.x + (i * 60);

            Vector3 pos = startPos;
            pos.x = Xpos;

            currentCards[i].transform.localPosition = pos;
        }
    }




    public void AddCardToList(CardScript card) 
    {
        currentCards.Add(card);
        CardPositioning();
    }

    private void EventSubscription() 
    {
        OnDeckUpdate += CardPositioning;
    }

    private void CheckNull()
    {
        Assert.IsNotNull(deckUIBounds, $"{ GetType().Name} missing required editor input 'deckUIBounds'");
        Assert.IsNotNull(deckStartPoint, $"{ GetType().Name} missing required editor input 'deckStartPoint'");


    }


}
