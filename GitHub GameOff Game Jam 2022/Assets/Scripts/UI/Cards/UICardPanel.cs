using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class UICardPanel : InspectorReferenceChecker
{
    public ActionCardSO CardToDisplay { private get; set; }
    public int handCardIndex;

    [Header("Main UI Components")]
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private Image iconImage;
    [SerializeField] private TMP_Text summaryText;
    [SerializeField] private TMP_Text[] effectKeyTexts;
    [SerializeField] private TMP_Text[] effectValueTexts;
    [SerializeField] private TMP_Text costText;
    [SerializeField] private TMP_Text actionButtonText;

    [Header("Recolourable Panels")]
    [SerializeField] private Image headerPanelImage;
    [SerializeField] private Image iconPanelImage;
    [SerializeField] private Image summaryPanelImage;
    [SerializeField] private Image effectPanelImage;
    [SerializeField] private Image effectSeparatorPanelImage;
    [SerializeField] private Image actionButtonPanelImage;
        
    private readonly Color SEED_PRIMARY = new Color(173, 236, 168);
    private readonly Color SEED_DARKER = new Color(173, 220, 150);

    public void Render()
    {
        switch (CardToDisplay)
        {
            case BuildingCard b:
                ApplyColourScheme
                (
                    CardManager.Instance.buildingPrimary, 
                    CardManager.Instance.buildingSecondary
                );
                actionButtonText.text = "build";
                break;
            case SeedCard s:
                ApplyColourScheme
                (
                    CardManager.Instance.seedPrimary,
                    CardManager.Instance.seedSecondary
                );
                actionButtonText.text = "plant";
                break;
            case LivestockCard l:
                ApplyColourScheme
                (
                    CardManager.Instance.seedPrimary,
                    CardManager.Instance.seedSecondary
                );
                actionButtonText.text = "place";
                break;
            default: throw new System.NotImplementedException("Card type is not yet implemented: " + CardToDisplay.GetType());
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
    }
    
    private void ApplyColourScheme(Color prm, Color drk)
    {
        // Apply the primary colours
        headerPanelImage.color = prm;
        effectSeparatorPanelImage.color = prm;
        costText.color = prm;
        actionButtonText.color = prm;
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

    public void PerformCardAction()
    {

    }

    public void DiscardCard() => CardManager.Instance.ConsiderCardDiscard(handCardIndex);

    protected override object[] CheckForMissingReferences() => new object[]
    {
        titleText, iconImage, summaryText, effectKeyTexts, effectValueTexts, costText, actionButtonText,
        headerPanelImage, iconPanelImage, summaryPanelImage, effectPanelImage, effectSeparatorPanelImage,
        actionButtonPanelImage
    };
}
