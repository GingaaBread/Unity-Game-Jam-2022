using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIInspectorPanel : MonoBehaviour
{
    [Header("Card UI Components")]
    [SerializeField] private Image backgroundPanelImage;
    [SerializeField] private Image cardObjectImage;
    [SerializeField] private Image cardDescriptionPanelImage;
    [SerializeField] private Image cardDetailPanelImage;
    [SerializeField] private Image[] textPanelImages;
    [SerializeField] private TMP_Text cardCostText;
    [SerializeField] private TMP_Text cardDescriptionText;
    [SerializeField] private TMP_Text maturingText;
    [SerializeField] private TMP_Text resourceText;
    [SerializeField] private TMP_Text resourceResellText;
    [SerializeField] private GameObject locationTextContainer;
    [SerializeField] private GameObject maturingContainer;
    [SerializeField] private GameObject resourcesContainer;
    [SerializeField] private GameObject bonusContainer;
    [SerializeField] private GameObject livestockLocationContainer;
    [SerializeField] private RectTransform detailPanel;

    [Header("Detail UI Components")]
    [SerializeField] private TMP_Text cardTitleText;
    [SerializeField] private TMP_Text cardSubtitleText;
    [SerializeField] private TMP_Text placementText;
    [SerializeField] private TMP_Text costText;

    [Header("Arrow Buttons")]
    [SerializeField] private Button priorButton;
    [SerializeField] private Button nextButton;

    private int currentIndex;

    public void Render(ActionCardSO cardToInspect, int index)
    {
        this.currentIndex = index;
        SetupArrowButtons();

        maturingContainer.SetActive(false);
        resourcesContainer.SetActive(false);
        bonusContainer.SetActive(false);
        locationTextContainer.SetActive(false);
        livestockLocationContainer.SetActive(false);

        if (cardToInspect is SeedCard card)
        {
            detailPanel.sizeDelta = new Vector3(detailPanel.sizeDelta.x, 620f);

            backgroundPanelImage.sprite = UIMainPanel.Instance.seedBackgroundSprite;
            cardDetailPanelImage.sprite = UIMainPanel.Instance.seedDetailSprite;
            cardDescriptionPanelImage.color = UIMainPanel.Instance.seedCardColour;

            placementText.text = "Cost of planting";
            cardDescriptionText.color = Color.white;

            maturingContainer.SetActive(true);
            maturingText.text = $"Takes {(int) card.cropTotalTurnsTillPayoff} turns to mature";

            resourcesContainer.SetActive(true);
            resourceText.text = $"Once cultivated, yields {card.payoffAmount}x {card.cardTitle.ToLower()}";
            resourceResellText.text = $"- 1 {card.cardTitle.ToLower()} can be sold for {card.payoffResource.basePrice} <sprite=1>";

            bonusContainer.SetActive(true);

            foreach (var textPanel in textPanelImages)
            {
                textPanel.sprite = UIMainPanel.Instance.seedTextSprite;
            }
        }
        else if (cardToInspect is BuildingCard buildingCard)
        {
            detailPanel.sizeDelta = new Vector3(detailPanel.sizeDelta.x, 360f);

            locationTextContainer.SetActive(true);

            backgroundPanelImage.sprite = UIMainPanel.Instance.buildingBackgroundSprite;
            cardDetailPanelImage.sprite = UIMainPanel.Instance.buildingDetailSprite;
            cardDescriptionPanelImage.color = UIMainPanel.Instance.buildingCardColour;

            placementText.text = "Cost of building";
            cardDescriptionText.color = Color.white;

            foreach (var textPanel in textPanelImages)
            {
                textPanel.sprite = UIMainPanel.Instance.buildingTextSprite;
            }
        }
        else if (cardToInspect is LivestockCard livestockCard)
        {
            detailPanel.sizeDelta = new Vector3(detailPanel.sizeDelta.x, 500f);

            livestockLocationContainer.SetActive(true);

            backgroundPanelImage.sprite = UIMainPanel.Instance.livestockBackgroundSprite;
            cardDetailPanelImage.sprite = UIMainPanel.Instance.livestockDetailSprite;
            cardDescriptionPanelImage.color = UIMainPanel.Instance.livestockCardColour;

            placementText.text = "Cost of placing";
            cardDescriptionText.color = Color.black;

            resourcesContainer.SetActive(true);
            resourceText.text = $"Every {livestockCard.turnsTillLivestockPayoff} turns, yields {livestockCard.payoffAmount}x {livestockCard.payoffResource.name.ToLower()}";
            resourceResellText.text = $"- 1 {livestockCard.payoffResource.name.ToLower()} can be sold for {livestockCard.payoffResource.basePrice} <sprite=1>";


            foreach (var textPanel in textPanelImages)
            {
                textPanel.sprite = UIMainPanel.Instance.livestockTextSprite;
            }
        }
        else throw new System.NotImplementedException($"Card type '{cardToInspect.GetType()}' is not yet implemented!");

        cardObjectImage.sprite = cardToInspect.cardSprite;
        cardCostText.text = cardToInspect.cardCost.ToString();
        costText.text = cardToInspect.cardCost.ToString();
        cardDescriptionText.text = cardToInspect.cardSummary;
        cardTitleText.text = cardToInspect.cardTitle;
        cardSubtitleText.text = cardToInspect.cardSubtitle;

    }

    public void SelectNext()
    {
        currentIndex++;
        SetupArrowButtons();
        Render(CardManager.Instance.GetHandcardAt(currentIndex), currentIndex);
    }

    public void SelectPrior()
    {
        currentIndex--;
        SetupArrowButtons();
        Render(CardManager.Instance.GetHandcardAt(currentIndex), currentIndex);
    }

    private void SetupArrowButtons()
    {
        priorButton.interactable = currentIndex > 0;
        nextButton.interactable = currentIndex < CardManager.Instance.GetHandcardSize() - 1;
    }
}
