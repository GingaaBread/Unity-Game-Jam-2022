using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DiscardButton : MonoBehaviour, IPointerDownHandler
{

    // Crostzard

    //Summary: If the UI this script is attached to gets left clicked, discard the selected card.

    private DeckManager deckManager;
    private CardScript card;

    private void Start()
    {
        if (deckManager == null) deckManager = transform.parent.parent.GetComponent<DeckManager>();
        card = transform.parent.GetComponent<CardScript>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (deckManager == null) Debug.LogError("DiscardButton script needs a deckManager variable");

        deckManager.DiscardCard(card);

    }
}
