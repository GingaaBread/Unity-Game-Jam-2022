using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class UICardPanel : InspectorReferenceChecker
{
    public ActionCardSO CardToDisplay { private get; set; }

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

    /// <summary>
    /// Card Colour Schemes
    /// </summary>
    private readonly Color PRIMARY = new (255, 143, 143);
    private readonly Color DARKER = new(255, 105, 105);

    public void Render()
    {
        // TODO: Change colour schemes depending on card type
        if (CardToDisplay is SeedCard seedCard)
        {
            ApplyColourScheme(PRIMARY, DARKER);
            actionButtonText.text = "plant";
        }

        // Set the texts
        titleText.text = CardToDisplay.cardTitle;
        summaryText.text = CardToDisplay.cardSummary;
        costText.text = CardToDisplay.cardCost.ToString();
        AssertLegalEffectSetup();
        for (int i = 0; i < effectKeyTexts.Length; i++)
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

        // Apply the darker colours
        iconPanelImage.color = drk;
        summaryPanelImage.color = drk;
        effectPanelImage.color = drk;
        actionButtonPanelImage.color = drk;
    }

    private void AssertLegalEffectSetup()
    {
        Assert.IsTrue
        (
            CardToDisplay.cardEffectKeys == null && CardToDisplay.cardEffectValues == null 
            ||
            effectKeyTexts.Length == CardToDisplay.cardEffectKeys.Length &&
            effectValueTexts.Length == CardToDisplay.cardEffectValues.Length
        );
    }

    public void PerformCardAction()
    {

    }

    public void DiscardCard()
    {

    }

    protected override object[] CheckForMissingReferences() => new object[]
    {
        titleText, iconImage, summaryText, effectKeyTexts, effectValueTexts, costText, actionButtonText,
        headerPanelImage, iconPanelImage, summaryPanelImage, effectPanelImage, effectSeparatorPanelImage,
        actionButtonPanelImage
    };
}
