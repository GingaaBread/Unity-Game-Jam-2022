using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIInspectorPanel : MonoBehaviour
{
    [Header("Card UI Components")]
    [SerializeField] private Image backgroundPanelImage;
    [SerializeField] private Image cardObjectImage;
    [SerializeField] private Image cardDescriptionPanelImage;
    [SerializeField] private TMP_Text cardCostText;
    [SerializeField] private TMP_Text cardDescriptionText;
    [SerializeField] private TMP_Text maturingText;
    [SerializeField] private TMP_Text resourceText;
    [SerializeField] private TMP_Text resourceResellText;
    [SerializeField] private GameObject maturingContainer;
    [SerializeField] private GameObject resourcesContainer;

    [Header("Detail UI Components")]
    [SerializeField] private TMP_Text cardTitleText;
    [SerializeField] private TMP_Text cardSubtitleText;
    [SerializeField] private TMP_Text placementText;
    [SerializeField] private TMP_Text costText;

    public void Render(ActionCardSO cardToInspect)
    {
        maturingContainer.SetActive(false);
        resourcesContainer.SetActive(false);

        if (cardToInspect is SeedCard card)
        {
            backgroundPanelImage.sprite = UIMainPanel.Instance.seedBackgroundSprite;
            cardDescriptionPanelImage.color = UIMainPanel.Instance.seedCardColour;

            placementText.text = "Cost of planting";

            maturingContainer.SetActive(true);
            maturingText.text = $"Takes {(int) card.cropTotalTurnsTillPayoff} turns to mature";

            resourcesContainer.SetActive(true);
            resourceText.text = $"Once cultivated, yields {card.payoffAmount}x {card.cardTitle.ToLower()}";
            resourceResellText.text = $"- 1 {card.cardTitle.ToLower()} can be sold for {card.payoffResource.basePrice} <sprite=1>";
        }
        else if (cardToInspect is BuildingCard)
        {
            backgroundPanelImage.sprite = UIMainPanel.Instance.buildingBackgroundSprite;
            cardDescriptionPanelImage.color = UIMainPanel.Instance.buildingCardColour;
            placementText.text = "Cost of building";
        }
        else if (cardToInspect is LivestockCard)
        {
            backgroundPanelImage.sprite = UIMainPanel.Instance.livestockBackgroundSprite;
            cardDescriptionPanelImage.color = UIMainPanel.Instance.livestockCardColour;
            placementText.text = "Cost of placing";
        }
        else throw new System.NotImplementedException($"Card type '{cardToInspect.GetType()}' is not yet implemented!");

        cardObjectImage.sprite = cardToInspect.cardSprite;
        cardCostText.text = cardToInspect.cardCost.ToString();
        costText.text = cardToInspect.cardCost.ToString();
        cardDescriptionText.text = cardToInspect.cardSummary;
        cardTitleText.text = cardToInspect.cardTitle;
        cardSubtitleText.text = cardToInspect.cardSubtitle;

    }
}
