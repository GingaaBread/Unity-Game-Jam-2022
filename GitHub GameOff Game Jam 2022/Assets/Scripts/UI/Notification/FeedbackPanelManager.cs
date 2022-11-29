namespace UIManagement
{
    using BuildingManagement;
    using FMODUnity;
    using System;
    using System.Collections.Generic;
    using TimeManagement;
    using TMPro;
    using UnityEngine;
    using UnityEngine.Assertions;
    using UnityEngine.UI;

    /// <summary>
    /// The FeedbackPanelManager class is used to enqueue UI feedback panels like notifications 
    /// or confirmation panels, which will be displayed at the beginning of the turn to the 
    /// new-turn queue, and UI panels that should be displayed instantly to the instant-display queue. 
    /// After all panels have been displayed, the player's actions are unlocked and can be used.
    /// </summary>
    /// <author>Gino</author>
    public class FeedbackPanelManager : MonoBehaviour
    {
        // The UIActionPanel's singleton
        private static FeedbackPanelManager _instance = null;
        public static FeedbackPanelManager Instance
        {
            get
            {
                if (_instance == null)
                    throw new Exception("UIPanelQueue singleton was called without " +
                        "UIPanelQueue being set up (check that UIPanelQueue is in the scene)");
                return _instance;
            }
            private set { _instance = value; }
        }

        [Header("Debug Flags")]
        public bool doDebugPrints;



        [Header("UI Notification Panel")]
        // The base GameObject of the notification panel
        [SerializeField] private GameObject notificationPanel;

        // The background panel image of the notification panel
        [SerializeField] private Image notificationPanelImage;

        // The icon image of the notification
        [SerializeField] private Image notificationPanelIconImage;

        // The animator of the notification panel used for triggering the animations
        [SerializeField] private Animator notificationPanelAnimator;

        // The text component of the notification panel displaying the actual notification string
        [SerializeField] private TMP_Text notificationPanelText;



        [Header("UI Confirmation Panel")]
        // The base GameObject of the confirmation panel
        [SerializeField] private GameObject confirmationPanel;

        // The text component of the confirmation panel describing what the confirmation reason is
        [SerializeField] private TMP_Text confirmationTitleText;



        [Header("UI Notification Icons")]
        [SerializeField] private Sprite moneyNotificationIcon; 
        [SerializeField] private Sprite buildingNotificationIcon;



        [Header("Sounds")]
        [SerializeField] private EventReference moneySoundEvent;
        [SerializeField] private EventReference buildingSoundEvent;


        // In the beginning of each turn, all panels will be dequeued and displayed
        private Queue<UIPanel> uiPanelTurnStartQueue;
        
        // At any point in time, panels can be enqueued and the queue be displayed
        private Queue<UIPanel> uiPanelInstantDisplayQueue;

        // TODO: Use the correct class from Crostzard
        public class Card { }

        /// <summary>
        /// The base class of all UI panels
        /// </summary>
        private abstract class UIPanel { }

        /// <summary>
        /// Determines which queue element is being displayed at the moment
        /// -1: none is displayed
        /// 0: The turn start queue is being displayed
        /// 1: The instant display queue is being displayed
        /// </summary>
        private int displayState;
        private const int DISPLAY_STATE_NONE = -1;
        private const int DISPLAY_STATE_NEW_TURN = 0;
        private const int DISPLAY_STATE_INSTANT_DISPLAY = 1;

        /// <summary>
        /// A short notification displaying the received amount of money
        /// </summary>
        private class MoneyReceptionUIPanel : UIPanel
        {
            public int ReceivedMoneyAmount { get; set; }
        }

        private class GenericMessageUIPanel : UIPanel
        {
            public string message;
            public GenericMessageUIPanel(string message) { this.message = message; }
        }


        /// <summary>
        /// A short notification displaying the received building
        /// </summary>
        private class BuildingReceptionUIPanel : UIPanel
        {
            public BuildingType ReceivedBuildingType { get; set; }
        }

        /// <summary>
        /// An entire panel displaying the received card together with a 
        /// confirmation button
        /// </summary>
        private class CardReceptionUIPanel : UIPanel
        {
            public Card ReceivedCard { get; set; }
        }

        /// <summary>
        /// On game start, setup the singleton, the queues, and add the listener
        /// </summary>
        private void Awake() 
        {
            // Sets up the singleton
            if (_instance == null) Instance = this;
            else throw new InvalidProgramException("Trying to instantiate the " +
                "UIPanelQueue singleton, but it already exists. Is there another script in the scene?");

            ValidateEditorInputs();

            uiPanelTurnStartQueue = new Queue<UIPanel>();
            uiPanelInstantDisplayQueue = new Queue<UIPanel>();
            displayState = DISPLAY_STATE_NONE;

            if (doDebugPrints)
            {
                print($"[DEBUG]: Created the queues and set the display state to {displayState}.");
            }

            TimeManager.OnStartPreTurn.AddListener(InitiateQueue);
        }

        private void ValidateEditorInputs() 
        {
            Assert.IsNotNull(notificationPanel, $"{GetType().Name} missing required editor input notificationPanel");
            Assert.IsNotNull(notificationPanelImage, $"{GetType().Name} missing required editor input notificationPanelImage");
            Assert.IsNotNull(notificationPanelIconImage, $"{GetType().Name} missing required editor input notificationPanelIconImage");
            Assert.IsNotNull(notificationPanelAnimator, $"{GetType().Name} missing required editor input notificationPanelAnimator");
            Assert.IsNotNull(notificationPanelText, $"{GetType().Name} missing required editor input notificationPanelText");
            Assert.IsNotNull(confirmationPanel, $"{GetType().Name} missing required editor input confirmationPanel");
            Assert.IsNotNull(confirmationTitleText, $"{GetType().Name} missing required editor input confirmationTitleText");
            Assert.IsNotNull(moneyNotificationIcon, $"{GetType().Name} missing required editor input moneyNotificationIcon");
            Assert.IsNotNull(buildingNotificationIcon, $"{GetType().Name} missing required editor input buildingNotificationIcon");
        }

        public void EnqueueGenericMessage(bool shouldBeEnqueuedToInstantQueue, string message) {
            var panel = new GenericMessageUIPanel(message);

            if (shouldBeEnqueuedToInstantQueue) {
                uiPanelInstantDisplayQueue.Enqueue(panel);
            } else {
                uiPanelTurnStartQueue.Enqueue(panel);
            }
        }

        /// <summary>
        /// Enqueues a panel to the FeedbackPanelManager displaying that the player has
        /// received a certain amount of money
        /// </summary>
        /// <param name="amountToDisplay">The amount of money received by the player</param>
        /// <param name="shouldBeEnqueuedToInstantQueue">Should the panel be enqueued to the instant or the new-turn queue?</param>
        public void EnqueueMoneyReception(int amountToDisplay, bool shouldBeEnqueuedToInstantQueue)
        {
            var panel = new MoneyReceptionUIPanel()
            {
                ReceivedMoneyAmount = amountToDisplay
            };

            if (shouldBeEnqueuedToInstantQueue)
            {
                uiPanelInstantDisplayQueue.Enqueue(panel);
            }
            else
            {
                uiPanelTurnStartQueue.Enqueue(panel);
            }
        }

        /// <summary>
        /// Enqueues a panel to the FeedbackPanelManager displaying that the player has
        /// received a new building that they can place in their farm
        /// </summary>
        /// <param name="buildingToDisplay">The building (acre, warehouse, etc.) received by the player</param>
        /// <param name="shouldBeEnqueuedToInstantQueue">Should the panel be enqueued to the instant or the new-turn queue?</param>
        public void EnqueueBuildingReception(BuildingType buildingToDisplay, bool shouldBeEnqueuedToInstantQueue)
        {
            var panel = new BuildingReceptionUIPanel()
            {
                ReceivedBuildingType = buildingToDisplay
            };

            if (shouldBeEnqueuedToInstantQueue)
            {
                uiPanelInstantDisplayQueue.Enqueue(panel);
            }
            else
            {
                uiPanelTurnStartQueue.Enqueue(panel);
            }
        }

        /// <summary>
        /// Enqueues a panel to the FeedbackPanelManager displaying that the player has
        /// received a new card that is added to their handcards
        /// (!) This needs to be confirmed by the player
        /// </summary>
        /// <param name="cardToDisplay">The card that should be displayed</param>
        /// <param name="shouldBeEnqueuedToInstantQueue">Should the panel be enqueued to the instant or the new-turn queue?</param>
        public void EnqueueCardReception(Card cardToDisplay, bool shouldBeEnqueuedToInstantQueue)
        {
            var panel = new CardReceptionUIPanel()
            {
                ReceivedCard = cardToDisplay
            };

            if (shouldBeEnqueuedToInstantQueue)
            {
                uiPanelInstantDisplayQueue.Enqueue(panel);
            }
            else
            {
                uiPanelTurnStartQueue.Enqueue(panel);
            }
        }

        /// <summary>
        /// In the beginning of each round, all messages of the queue are displayed
        /// one after the other. After the final message was displayed, unlocks the 
        /// player's actions.
        /// </summary>
        private void InitiateQueue()
        {
            CheckIfQueueIsAlreadyBeingDisplayed();

            // LockActions(); TODO: Implement

            displayState = DISPLAY_STATE_NEW_TURN;

            if (doDebugPrints)
            {
                print($"[DEBUG]: Locked player actions and set the display state to {displayState}.");
            }

            InitiateNextPanel();
        }

        /// <summary>
        /// This method is called after instant UI notifications or confirmations
        /// were enqueued that should be displayed when this method is called, without
        /// having to wait for the new turn
        /// </summary>
        public void InitiateInstantDisplayQueue()
        {
            if (displayState == DISPLAY_STATE_NONE)
            {
                // LockActions(); TODO: Implement
                    displayState = DISPLAY_STATE_INSTANT_DISPLAY;

                if (doDebugPrints)
                {
                    print($"[DEBUG]: Locked player actions and set the display state to {displayState}.");
                }

                InitiateNextPanel();
            }
        }

        /// <summary>
        /// When all queue elements have been displayed, unlock the player's actions
        /// </summary>
        /// <param name="ofTheNewTurnQueue">Which queue end should be processed?</param>
        private void InitiateQueueDisplayEnd(bool ofTheNewTurnQueue)
        {
            // UnlockActions(); TODO: Implement

            displayState = DISPLAY_STATE_NONE;

            if (doDebugPrints)
            {
                print($"[DEBUG]: Unlocked player actions and set the display state to {displayState}.");
            }

            if (ofTheNewTurnQueue)
            {
                TimeManager.Instance.FinishPreTurnPhase();
            }
        }

        /// <summary>
        /// Used to throw an InvalidProgramException when attempting to initiate a new
        /// queue whilst another queue is currently already being displayed
        /// </summary>
        private void CheckIfQueueIsAlreadyBeingDisplayed()
        {
            if (displayState != DISPLAY_STATE_NONE)
                throw new InvalidProgramException("You cannot initiate a new queue whilst another " +
                    $"is already being displayed! Display state: {displayState}");
        }

        /// <summary>
        /// Depending on the current display state, initiates either the turn start or 
        /// instant display queue
        /// </summary>
        public void InitiateNextPanel()
        {
            if (doDebugPrints)
            {
                print($"[DEBUG]: Initiating the next panel with the display state set to {displayState}.");
            }

            if (displayState == DISPLAY_STATE_INSTANT_DISPLAY)
            {
                HandlePanel(false);
            }
            else if (displayState == DISPLAY_STATE_NEW_TURN)
            {
                HandlePanel(true);
            }
            else
            {
                throw new InvalidProgramException($"The display state is not matching either queue," +
                    $"so the next panel cannot be initiated. State: {displayState}");
            }
        }

        /// <summary>
        /// First, deactivates all panel GameObjects (to prevent visual bugs)
        /// Then displays the current panel of the queue if it exists, or 
        /// initiates the end of the queue display if the queue is empty
        /// </summary>
        /// <param name="ofTheTurnStartQueue"></param>
        private void HandlePanel(bool ofTheTurnStartQueue)
        {
            DeactivateAllPanels();

            if (UIPanelsAreEnqueued(ofTheTurnStartQueue))
            {
                if (doDebugPrints)
                {
                    print($"[DEBUG]: There are still panels enqueued, therefore displaying the current.");
                }

                DisplayCurrentPanel(ofTheTurnStartQueue);
            }
            else
            {
                if (doDebugPrints)
                {
                    print($"[DEBUG]: No more panels enqueued, therefore initiating the end.");
                }

                InitiateQueueDisplayEnd(ofTheTurnStartQueue);
            }
        }
                
        /// <summary>
        /// Deactivates the GameObjects of the notification and confirmation panels
        /// </summary>
        private void DeactivateAllPanels()
        {
            notificationPanel.SetActive(false);
            confirmationPanel.SetActive(false);
        }

        /// <summary>
        /// Activates the panel GameObject, sets its panel colour and plays the animation
        /// </summary>
        /// <param name="panelColour">The background colour of the notification panel</param>
        private void DisplayNotification(Color panelColour, Sprite notificationIcon)
        {
            notificationPanel.SetActive(true);
            notificationPanelImage.color = panelColour;
            if (notificationIcon != null) {
                notificationPanelIconImage.sprite = notificationIcon;
                notificationPanelIconImage.enabled = true;
            } else {
                notificationPanelIconImage.sprite = null;
                notificationPanelIconImage.enabled = false;
            }

            notificationPanelAnimator.Play("UINotificationPanelDisplay");
        }

        /// <summary>
        /// Displays a panel showing a title, a card, and a confirmation button. This can be
        /// used for scenarios like 'You have received this card' or 'An earthquake destroyed
        /// this card', etc. Note that this only for confirming choices, not rejecting them
        /// </summary>
        /// <param name="confirmationTitle">What text should be displayed</param>
        /// <param name="cardToDisplay">What card should be displayed</param>
        private void DisplayCardConfirmation(string confirmationTitle, Card cardToDisplay)
        {
            // TODO: Set the card image

            confirmationPanel.SetActive(true);
            confirmationTitleText.text = confirmationTitle;
        }

        /// <summary>
        /// Depending on the current panel type, sets the UI text, icons and plays 
        /// the respective animation
        /// </summary>
        /// <param name="ofTheTurnStartQueue">Should the current panel of the new-turn or instant queue be displayed?</param>
        private void DisplayCurrentPanel(bool ofTheTurnStartQueue)
        {
            UIPanel currentPanel;
            if (ofTheTurnStartQueue)
            {
                currentPanel = uiPanelTurnStartQueue.Dequeue();
            }
            else
            {
                currentPanel = uiPanelInstantDisplayQueue.Dequeue();
            }

            notificationPanelText.text = "";

            if (currentPanel is MoneyReceptionUIPanel moneyPanel)
            {
                RuntimeManager.PlayOneShot(moneySoundEvent);
                notificationPanelText.text = $"Received {moneyPanel.ReceivedMoneyAmount}$!";
                DisplayNotification(Color.yellow, moneyNotificationIcon);
            }
            else if (currentPanel is BuildingReceptionUIPanel buildingPanel)
            {
                RuntimeManager.PlayOneShot(buildingSoundEvent);
                notificationPanelText.text = $"Received {buildingPanel.ReceivedBuildingType}!";
                DisplayNotification(Color.red, buildingNotificationIcon);
            }
            else if (currentPanel is CardReceptionUIPanel cardPanel)
            {
                notificationPanelText.text = $"Received Card: {cardPanel.ReceivedCard}!";
                DisplayCardConfirmation("New Card Received:", cardPanel.ReceivedCard);
            }
            else if (currentPanel is GenericMessageUIPanel genericMessageUIPanel)
            {
                notificationPanelText.text = genericMessageUIPanel.message;
                DisplayNotification(Color.yellow, null);
            } 
            else throw new NotImplementedException($"The UI panel '{currentPanel}' is not yet implemented!");
        }

        /// <summary>
        /// Determines whether there are still UI panels in the queue (true) 
        /// or if the queue is empty (false)
        /// </summary>
        /// <returns>true: The queue is not empty, false: The queue is empty</returns>
        private bool UIPanelsAreEnqueued(bool inTheTurnStartQueue)
        {
            if (inTheTurnStartQueue)
            {
                return uiPanelTurnStartQueue.Count != 0;
            }
            else
            {
                return uiPanelInstantDisplayQueue.Count != 0;
            }
        }
    }
}
