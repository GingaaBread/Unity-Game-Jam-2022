using PlayerData;
using TimeManagement;
using UIManagement;
using UnityEditor;
using UnityEngine;

public class EditorDropdowns : EditorWindow
{
    // Logging

    [MenuItem("Gamejam/Logging/LogCurrentTimeAndPhase", false, 0)] static void LogCurrentTimeAndPhase() {
        if (!EditorApplication.isPlaying) { return; }
        Debug.Log($"Time: {TimeManager.Instance.CurrentTime} and Phase: {TimeManager.Instance.CurrentPhase}");
    }

    // Resource Test

    [MenuItem("Gamejam/Resource/TestAdd", false, 0)] static void AddTestResource() {
        if (!EditorApplication.isPlaying) { return; }
        ResourceSO testResource = 
            (ResourceSO)AssetDatabase.LoadAssetAtPath("Assets/Scripts/PlayerData/ResourceSOs/Wheat.asset", 
            typeof(ResourceSO));

        Debug.Log($"New item amount: {PlayerDataManager.Instance.GetInventoryItemAmount(testResource)}. Exp. 0");
        
        PlayerDataManager.Instance.IncreaseInventoryItemAmount(testResource, 5);
        Debug.Log($"New item amount: {PlayerDataManager.Instance.GetInventoryItemAmount(testResource)}. Exp. 5");
    }

    // UI

    [MenuItem("Gamejam/UI/Directly Display All Test Notifications")] static void DirectlyDisplayAllTestNotification() {
        if (!EditorApplication.isPlaying) { return; }

        FeedbackPanelManager.Instance.EnqueueMoneyReception(5, true);
        FeedbackPanelManager.Instance.EnqueueBuildingReception(BuildingManagement.BuildingType.ACRE, true);
        FeedbackPanelManager.Instance.EnqueueCardReception(new FeedbackPanelManager.Card(), true);
        FeedbackPanelManager.Instance.EnqueueMoneyReception(12, true);

        FeedbackPanelManager.Instance.InitiateInstantDisplayQueue();
    }

    [MenuItem("Gamejam/UI/Enqueue All Test Notifications")] static void EnqueueAllTestNotification() {
        if (!EditorApplication.isPlaying) { return; }
        FeedbackPanelManager.Instance.EnqueueMoneyReception(5, false);
        FeedbackPanelManager.Instance.EnqueueBuildingReception(BuildingManagement.BuildingType.ACRE, false);
        FeedbackPanelManager.Instance.EnqueueCardReception(new FeedbackPanelManager.Card(), false);
        FeedbackPanelManager.Instance.EnqueueMoneyReception(12, false);
    }

    [MenuItem("Gamejam/UI/Enqueue Test Money Notification")] static void EnqueueTestMoneyNotification() {
        if (!EditorApplication.isPlaying) { return; }
        FeedbackPanelManager.Instance.EnqueueMoneyReception(5, false);
    }

    [MenuItem("Gamejam/UI/Enqueue Test Building Notification")] static void EnqueueTestBuildingNotification() {
        if (!EditorApplication.isPlaying) { return; }
        FeedbackPanelManager.Instance.EnqueueBuildingReception(BuildingManagement.BuildingType.ACRE, false);
    }

    [MenuItem("Gamejam/UI/Enqueue Test Card Notification")] static void EnqueueTestCardNotification() {
        if (!EditorApplication.isPlaying) { return; }
        FeedbackPanelManager.Instance.EnqueueCardReception(new FeedbackPanelManager.Card(), false); // TODO: Replace by actual card class
    }

    // Quests

    [MenuItem("Gamejam/Quest/TellQuestManagerWeGot5Wheat")] static void TellQuestManagerWeGot5Wheat() {
        if (!EditorApplication.isPlaying) { return; }
        string assetPath = "Assets/Scripts/PlayerData/ResourceSOs/Wheat.asset";
        ResourceSO wheatResource = (ResourceSO)AssetDatabase.LoadAssetAtPath(assetPath, typeof(ResourceSO));
        if (wheatResource == null) Debug.LogError("TellQuestManagerWeGot5Wheat failed to fine asset "+assetPath);
        QuestManager.Instance.NotifyOfResourceCollected(wheatResource, 5);
    }
    [MenuItem("Gamejam/Quest/TellQuestManagerWeSoldWheat")] static void TellQuestManagerWeSoldWheat() {
        if (!EditorApplication.isPlaying) { return; }
        string assetPath = "Assets/Scripts/PlayerData/ResourceSOs/Wheat.asset";
        ResourceSO wheatResource = (ResourceSO)AssetDatabase.LoadAssetAtPath(assetPath, typeof(ResourceSO));
        if (wheatResource == null) Debug.LogError("TellQuestManagerWeSoldWheat failed to fine asset "+assetPath);
        QuestManager.Instance.NotifyOfResourceSale(wheatResource, 750);
    }
    [MenuItem("Gamejam/Quest/TellQuestManagerWeBuiltCow")] static void TellQuestManagerWeBuiltCow() {
        if (!EditorApplication.isPlaying) { return; }
        string assetPath = "Assets/Scripts/CardData/LivestockCardSO/CowCard.asset";
        ActionCardSO card = (ActionCardSO)AssetDatabase.LoadAssetAtPath(assetPath, typeof(ActionCardSO));
        if (card == null) Debug.LogError("TellQuestManagerWeBuiltCow failed to fine asset "+assetPath);
        QuestManager.Instance.NotifyOfTilePlaced(card);
    }

    // Game Won Panel

    [MenuItem("Gamejam/GameWonPanel/Show")]
    static void ShowGameWonPanel() {
        if (!EditorApplication.isPlaying) { return; }
        GameWonPanel.Instance.Show();
    }

    // Game Lost Panel

    [MenuItem("Gamejam/GameLostPanel/Show")]
    static void ShowGameLostPanel() {
        if (!EditorApplication.isPlaying) { return; }
        GameLostPanel.Instance.Show();
    }

}
