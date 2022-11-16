using PlayerData;
using TimeManagement;
using UIManagement;
using UnityEditor;
using UnityEngine;

public class EditorDropdowns : EditorWindow
{
    // Logging

    [MenuItem("Gamejam/Logging/LogCurrentTimeAndPhase", false, 0)]
    static void LogCurrentTimeAndPhase()
    {
        IgnoreIdleEditorActions();
        Debug.Log($"Time: {TimeManager.Instance.CurrentTime} and Phase: {TimeManager.Instance.CurrentPhase}");
    }

    // Time

    [MenuItem("Gamejam/Time/ForceNextPhase", false, 0)]
    static void ForceNextPhase()
    {
        IgnoreIdleEditorActions();
        TimeManager.Instance.FinishCurrentPhase();
    }

    // Card Test

    [MenuItem("Gamejam/Cards/Draw", false, 0)]
    static void DrawCard()
    {
        CardManager.Instance.GiveCard(1);
    }
    
    [MenuItem("Gamejam/Cards/AddBalancedCards", false, 0)]
    static void AddBalancedCards()
    {
        CardManager.Instance.AddBalancedCards();
    }

    // Resource Test

    [MenuItem("Gamejam/Resource/TestAdd", false, 0)]
    static void AddTestResource()
    {
        ResourceSO testResource = 
            (ResourceSO)AssetDatabase.LoadAssetAtPath("Assets/Scripts/PlayerData/ResourceSOs/Wheat.asset", 
            typeof(ResourceSO));

        Debug.Log($"New item amount: {PlayerDataManager.Instance.GetInventoryItemAmount(testResource)}. Exp. 0");
        
        PlayerDataManager.Instance.IncreaseInventoryItemAmount(testResource, 5);
        Debug.Log($"New item amount: {PlayerDataManager.Instance.GetInventoryItemAmount(testResource)}. Exp. 5");
    }

    // UI

    [MenuItem("Gamejam/UI/Directly Display All Test Notifications")]
    static void DirectlyDisplayAllTestNotification()
    {
        IgnoreIdleEditorActions();

        FeedbackPanelManager.Instance.EnqueueMoneyReception(5, true);
        FeedbackPanelManager.Instance.EnqueueBuildingReception(BuildingManagement.BuildingType.ACRE, true);
        FeedbackPanelManager.Instance.EnqueueCardReception(new FeedbackPanelManager.Card(), true);
        FeedbackPanelManager.Instance.EnqueueMoneyReception(12, true);

        FeedbackPanelManager.Instance.InitiateInstantDisplayQueue();
    }

    [MenuItem("Gamejam/UI/Enqueue All Test Notifications")]
    static void EnqueueAllTestNotification()
    {
        IgnoreIdleEditorActions();
        FeedbackPanelManager.Instance.EnqueueMoneyReception(5, false);
        FeedbackPanelManager.Instance.EnqueueBuildingReception(BuildingManagement.BuildingType.ACRE, false);
        FeedbackPanelManager.Instance.EnqueueCardReception(new FeedbackPanelManager.Card(), false);
        FeedbackPanelManager.Instance.EnqueueMoneyReception(12, false);
    }

    [MenuItem("Gamejam/UI/Enqueue Test Money Notification")]
    static void EnqueueTestMoneyNotification()
    {
        IgnoreIdleEditorActions();
        FeedbackPanelManager.Instance.EnqueueMoneyReception(5, false);
    }

    [MenuItem("Gamejam/UI/Enqueue Test Building Notification")]
    static void EnqueueTestBuildingNotification()
    {
        IgnoreIdleEditorActions();
        FeedbackPanelManager.Instance.EnqueueBuildingReception(BuildingManagement.BuildingType.ACRE, false);
    }

    [MenuItem("Gamejam/UI/Enqueue Test Card Notification")]
    static void EnqueueTestCardNotification()
    {
        IgnoreIdleEditorActions();
        FeedbackPanelManager.Instance.EnqueueCardReception(new FeedbackPanelManager.Card(), false); // TODO: Replace by actual card class
    }

    [MenuItem("Gamejam/Quest/TellQuestManagerWeGot5Wheat")]
    static void TellQuestManagerWeGot5Wheat() {
        IgnoreIdleEditorActions();

        string assetPath = "Assets/Scripts/PlayerData/ResourceSOs/Wheat.asset";
        ResourceSO wheatResource = (ResourceSO)AssetDatabase.LoadAssetAtPath(assetPath, typeof(ResourceSO));
        if (wheatResource == null) Debug.LogError("TellQuestManagerWeGot5Wheat failed to fine asset "+assetPath);
        QuestManager.Instance.NotifyOfResourceCollected(wheatResource, 5);
    }

    // Management

    private static void IgnoreIdleEditorActions()
    {
        if (!EditorApplication.isPlaying)
        {
            return;
        }
    }
}
