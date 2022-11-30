using System;
using TimeManagement;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using FMODUnity;

public class UICardPanel : ComputerPhaseStep, IPointerEnterHandler, IPointerDownHandler
{
    public ActionCardSO CardToDisplay { get; set; }
    public bool isDetailedCard = false, isPreviewCard = false;

    [Header("Main UI Components")]
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private Image iconImage;
    [SerializeField] private TMP_Text summaryText;
    [SerializeField] private Button discardButton;
    [SerializeField] private TMP_Text costText;

    [Header("Audio")]
    [SerializeField] private StudioEventEmitter audioEmitter;

    [Header("NewUI references")]
    [SerializeField] private Image CardBackground;
    [SerializeField] private Image ImagePreviewBackground;
    [SerializeField] private Image ImagePreviewForeground;
    [SerializeField] private Image SummaryBackground;

    private Color selectionBorderColour;

    /// <summary>
    /// Sets up all UI components, rendering the selected CardToDisplay
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    /// <exception cref="MissingReferenceException"></exception>
    public void Render()
    {
        if (CardToDisplay == null)
            throw new MissingReferenceException("You need to define the CardToDisplay before rendering a card");


        CardBackground.sprite = CardToDisplay.Background;
        ImagePreviewBackground.sprite = CardToDisplay.PreviewBackground;
        ImagePreviewForeground.sprite = CardToDisplay.cardSprite;
        SummaryBackground.sprite = CardToDisplay.summaryBackground;

        // Set the texts
        titleText.text = CardToDisplay.cardTitle;
        summaryText.text = CardToDisplay.cardSummary;
        costText.text = CardToDisplay.cardCost.ToString();
        AssertLegalEffectSetup();

        if (CardManager.Instance.cardDiscardedThisTurn)
        {
            LockDiscardButton();
        }
        else
        {
            UnlockDiscardButton();
        }
    }

    private void ApplyColourScheme(Color prm, Color drk)
    {
        // Apply the darker colours
        selectionBorderColour = drk;
    }

    private void AssertLegalEffectSetup()
    {
        Assert.IsTrue
        (
            CardToDisplay.cardEffectKeys == null && CardToDisplay.cardEffectValues == null
            ||
            CardToDisplay.cardEffectKeys.Length == CardToDisplay.cardEffectValues.Length
        );
    }

    public int GetCardIndex() => transform.GetSiblingIndex();

    public void DiscardCard() => CardManager.Instance.ConsiderCardDiscard(GetCardIndex());

    public void LockDiscardButton() => discardButton.gameObject.SetActive(false);
    public void UnlockDiscardButton() => discardButton.gameObject.SetActive(true);

    // Time Event Management

    protected override object[] CheckForMissingReferences() => new object[]
    {
        titleText, iconImage, summaryText, costText
    };

    public override void StartProcessingForComputerPhase(bool isComputerPhaseDuringGameInit)
    {
        if (!isComputerPhaseDuringGameInit)
        { 
            UnlockDiscardButton();
        }
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isDetailedCard && !isPreviewCard && !CardPlayManager.Instance.PlayIsInProgress())
            UIMainPanel.Instance.DisplayDetailedCard(this, GetCardIndex());
    }

    public void OnPointerDown(PointerEventData E)
    {
        if (isDetailedCard && CardPlayManager.Instance.PlayIsInProgress())
        {
            CardPlayManager.Instance.ResetCurrentPlay();
        }
        else if (!isPreviewCard)
        {
            audioEmitter.Play();
            UIMainPanel.Instance.PlaySelectionAnimation(selectionBorderColour);

            var smallPanel = UIMainPanel.Instance.GetDetailHandcardPanel();
            CardToDisplay.Action(smallPanel);
        }
    }
}
