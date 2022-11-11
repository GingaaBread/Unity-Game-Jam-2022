using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DiscardButton : MonoBehaviour, IPointerDownHandler
{

    // Crostzard

    //Summary: If the UI this script is attached to gets left clicked, discard the selected card.


    public DeckManager deckManager;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (deckManager == null) Debug.LogError("DiscardButton script needs a deckManager variable");

        if (deckManager.Selected == null) return;

        deckManager.DiscardCard(deckManager.Selected);

    }
}
