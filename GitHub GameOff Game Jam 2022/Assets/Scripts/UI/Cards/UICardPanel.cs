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
    [SerializeField] private Image discardIconImage;
    [SerializeField] private TMP_Text discardText;
    [SerializeField] private TMP_Text summaryText;
    [SerializeField] private Button discardButton;
    [SerializeField] private TMP_Text[] effectKeyTexts;
    [SerializeField] private TMP_Text[] effectValueTexts;
    [SerializeField] private TMP_Text costText;

    [Header("Recolourable Panels")]
    [SerializeField] private Image headerPanelImage;
    [SerializeField] private Image iconPanelImage;
    [SerializeField] private Image summaryPanelImage;
    [SerializeField] private Image effectPanelImage;
    [SerializeField] private Image effectSeparatorPanelImage;
    [SerializeField] private Image actionButtonPanelImage;

    [Header("Audio")]
    [SerializeField] private StudioEventEmitter audioEmitter;

    /// <summary>
    /// Sets up all UI components, rendering the selected CardToDisplay
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    /// <exception cref="MissingReferenceException"></exception>
    public void Render()
    {
        if (CardToDisplay == null)
            throw new MissingReferenceException("You need to define the CardToDisplay before rendering a card");

        switch (CardToDisplay)
        {
            case BuildingCard b:
                ApplyColourScheme
                (
                    CardManager.Instance.buildingPrimary,
                    CardManager.Instance.buildingSecondary
                );
                break;
            case SeedCard s:
                ApplyColourScheme
                (
                    CardManager.Instance.seedPrimary,
                    CardManager.Instance.seedSecondary
                );
                break;
            case LivestockCard l:
                ApplyColourScheme
                (
                    CardManager.Instance.livestockPrimary,
                    CardManager.Instance.livestockSecondary
                );
                break;
            default: throw new NotImplementedException("Card type is not yet implemented: " + CardToDisplay.GetType());
        }

        // Set the texts
        titleText.text = CardToDisplay.cardTitle;
        summaryText.text = CardToDisplay.cardSummary;
        costText.text = CardToDisplay.cardCost.ToString();
        AssertLegalEffectSetup();
        for (int i = 0; i < CardToDisplay.cardEffectKeys.Length; i++)
        {
            effectKeyTexts[i].text = CardToDisplay.cardEffectKeys[i];
            effectValueTexts[i].text = CardToDisplay.cardEffectValues[i];
        }

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
        // Apply the primary colours
        headerPanelImage.color = prm;
        effectSeparatorPanelImage.color = prm;
        costText.color = prm;
        discardIconImage.color = prm;
        discardText.color = prm;
        actionButtonPanelImage.color = prm;

        // Apply the darker colours
        iconPanelImage.color = drk;
        summaryPanelImage.color = drk;
        effectPanelImage.color = drk;
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
        titleText, iconImage, summaryText, effectKeyTexts, effectValueTexts, costText, discardIconImage,
        headerPanelImage, iconPanelImage, summaryPanelImage, effectPanelImage, effectSeparatorPanelImage,
        actionButtonPanelImage, discardText
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
        if (!isDetailedCard && !isPreviewCard)
            UIMainPanel.Instance.DisplayDetailedCard(this, GetCardIndex());
    }

    public void OnPointerDown(PointerEventData E)
    {
        if (!isPreviewCard)
        {
            audioEmitter.Play();
            CardManager.Instance.RemoveCardOnUse(UIMainPanel.Instance.GetDetailHandcardIndex());
            QuestManager.Instance.NotifyOfTilePlaced(CardToDisplay);
            CardToDisplay.Action();
            UIMainPanel.Instance.HideDetailedCard();
        }
    }
}
