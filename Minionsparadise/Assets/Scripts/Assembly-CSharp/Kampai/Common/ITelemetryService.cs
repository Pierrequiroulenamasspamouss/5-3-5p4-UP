namespace Kampai.Common
{
	public interface ITelemetryService : global::Kampai.Common.IIapTelemetryService
	{
		void AddTelemetrySender(global::Kampai.Common.ITelemetrySender sender);

		void AddIapTelemetryService(global::Kampai.Common.IIapTelemetryService service);

		void SharingUsage(global::Kampai.Common.ITelemetrySender sender, bool enabled);

		void GameStarted();

		int SecondsSinceGameStart();

		void COPPACompliance();

		void SharingUsageCompliance();

		void SharingUsage(bool enabled);

		bool SharingUsageEnabled();

		void LogGameEvent(global::Kampai.Common.TelemetryEvent gameEvent);

		void Send_Telemetry_EVT_GAME_ERROR_GAMEPLAY(string nameOfError, string errorDetails, bool userFacing);

		void Send_Telemetry_EVT_GAME_ERROR_CONNECTIVITY(string nameOfError, string errorDetails, bool userFacing);

		void Send_Telemetry_EVT_GAME_ERROR_CRASH(string nameOfError, string crashReason, string crashTime, string errorDetails);

		void Send_Telemetry_EVT_GAME_BUTTON_PRESSED_GENERIC(global::Kampai.Util.GameConstants.TrackedGameButton buttonName, string optionalParam2 = "", global::Kampai.Common.ParameterName param2Name = global::Kampai.Common.ParameterName.NONE);

		void Send_Telemetry_EVT_GAME_XPROMO_BUTTON_PRESSED(global::Kampai.Util.GameConstants.TrackedGameButton buttonName, bool petsInstalled);

		void Send_Telemetry_EVT_IGE_FREE_CREDITS_EARNED(int grindEarned, string eventName, bool purchasedCurrencySpent);

		void Send_Telemetry_EVT_IGE_PAID_CREDITS_EARNED(int premiumEarned, string eventName, bool purchasedCurrencySpent);

		void Send_Telemetry_EVT_IGE_FREE_CREDITS_PURCHASE_REVENUE(int grindSpent, string itemPurchased, bool purchasedCurrencySpent, string highLevel, string specific, string type);

		void Send_Telemetry_EVT_IGE_PAID_CREDITS_PURCHASE_REVENUE(int premiumSpent, string itemPurchased, bool purchasedCurrencySpent, string highLevel, string specific, string type);

		void Send_Telemetry_EVT_IGE_RESOURCE_CRAFTABLE_EARNED(int amount, string itemName, string itemType, string highLevel, string specific, string type);

		void Send_Telemetry_EVT_IGE_RESOURCE_CRAFTABLE_SPENT(int amount, string sourceName, string itemName, string highLevel, string specific, string type);

		void Send_Telemetry_EVT_IGE_STORE_VISIT(string trafficSource, string storeVisited);

		void Send_Telemetry_EVT_USER_TUTORIAL_FUNNEL_EAL(string tutorialName, string step);

		void Send_Telemetry_EVT_USER_GAME_LOAD_FUNNEL(string step, string swrveGroup, string performance);

		void Send_Telemetry_EVT_USER_GAME_DOWNLOAD_FUNNEL(string dlcName, int duration, long size, bool isNetworkWifi);

		void Send_Telemetry_EVT_GP_LEVEL_PROMOTION();

		void Send_Telemetry_EVT_GP_ACHIEVEMENTS_CHECKPOINTS_EAL(string achievementName, global::Kampai.Common.Service.Telemetry.TelemetryAchievementType type, int PartyPointsEarned, string questGiver = "");

		void Send_Telemetry_EVT_GP_ACHIEVEMENTS_CHECKPOINTS_EAL_ProceduralQuest(string achievementName, global::Kampai.Common.Service.Telemetry.ProceduralQuestEndState endState, int PartyPointsEarned);

		void Send_Telemetry_EVT_GP_ACHIEVEMENTS_STARTED_EAL(string achievementName, global::Kampai.Common.Service.Telemetry.TelemetryAchievementType type, string questGiver = "");

		void Send_Telemetry_EVT_EBISU_LOGIN_GAMECENTER(string loginLocation);

		void Send_Telemetry_EVT_EBISU_LOGIN_GOOGLEPLAY(string loginLocation);

		void Send_Telemetry_EVT_EBISU_LOGIN_FACEBOOK(string loginLocation, string loginSource);

		void Send_Telemetry_EVT_AGE_GATE_SET(int year, int month);

		void Send_TelemetryCharacterPrestiged(global::Kampai.Game.Prestige prestige);

		void Send_TelemetryOrderBoard(bool isFillingOrder, global::Kampai.Game.Transaction.TransactionDefinition transactionDef, int characterDefinitionId);

		void Send_Telemetry_EVT_IN_APP_MESSAGE_DISPLAYED(string inAppMessageName, global::Kampai.Main.HindsightCampaign.DismissType choice);

		void Send_Telemetry_EVT_USER_TRACKING_OPTOUT();

		void Send_Telemetry_EVT_MINI_GAME_PLAYED(string mignetteName, int score, float timePlayed, int xpReward);

		void Send_Telemetry_EVT_USER_DATA_AT_APP_START(int seconds, int tokenCount, int minions, string swrveGroup, string expansions);

		void Send_Telemetry_EVT_USER_DATA_AT_APP_CLOSE();

		void Send_Telemetry_EVT_STORAGE_LIMIT_HIT(int storageLimit);

		void Send_Telemetry_EVT_RATE_MY_APP(string promptType, bool? userAccepted);

		void Send_Telemetry_EVT_SOCIAL_EVENT_COMPLETION(int teamSize);

		void Send_Telemetry_EVT_SOCIAL_EVENT_CONTRIBUTION(string item, int quantity, int teamSize, int xpReward);

		void Send_Telemtry_EVT_MINI_TIER_REACHED(string mignetteName, int tier, int plays);

		void Send_Telemtry_EVT_MARKETPLACE_ITEM_LISTED(string itemName, int quantity, int price, string highLevel, string specific, string type, string other);

		void Send_Telemtry_EVT_MARKETPLACE_VIEWED(string viewType);

		void Send_Telemetry_EVT_PLAYER_TRAINING(int triggeredID, int fromSettings, int timeOpen);

		void Send_Telemetry_EVT_MINION_PARTY_STARTED(int totalPartyPoints, string buffSelected, string guestOfHonor, bool isInspiredParty);

		void Send_Telemetry_EVT_PARTY_POINTS_EARNED(int amountOfPartyPoints, string sourceName);

		void Send_Telemetry_EVT_NOTE_SETTING_CHANGE(string settingName, string enabled, string sourceName);

		void Send_Telemetry_EVT_PARTY_SKIPPED();

		void Send_Telemetry_EVT_PINCH_PROMPT(string sourceName, global::Kampai.Game.PendingCurrencyTransaction pct, global::System.Collections.Generic.IList<global::Kampai.Util.QuantityItem> requiredItems, string action);

		void Send_Telemetry_EVT_PINCH_PROMPT(string sourceName, string itemName, int amount, string highLevel, string specific, string type, string action);

		void Send_Telemetry_EVT_DCN(string buttonPressed, string url, string name);

		void Send_Telemetry_EVT_UPSELL(string mtxSellID, global::Kampai.Common.Service.Telemetry.UpsellStatus status);

		void Send_Telemetry_EVT_TSM_TRIGGER_ACTION(global::Kampai.Game.Trigger.TriggerDefinition triggerDefinition, global::Kampai.Game.Trigger.TriggerRewardDefinition reward);

		void Send_Telemetry_EVT_MINION_POPULATION_BENEFIT(string benefit);

		void Send_Telemetry_EVT_MINION_UPGRADE(int newLevel, int tokensUsed, uint tokensBeforeUpgrade);

		void Send_Telemetry_EVT_TSM_TRIGGER_BUY_SELL(global::Kampai.Game.Trigger.TriggerDefinition triggerDefinition, global::Kampai.Game.Trigger.TriggerRewardDefinition reward);

		void Send_Telemetry_EVT_USER_ACQUIRES_BUILDING(string source, int buildingDefId, int sourceDefId);

		void Send_Telemetry_EVT_USER_ACQUIRES_BUILDING(string source, global::Kampai.Game.BuildingDefinition buildingDefinition, int sourceDefId);

		void Send_Telemetry_EVT_MASTER_PLAN_COMPLETE(string masterPlanName, string villainName, int duration);

		void Send_Telemetry_EVT_MASTER_PLAN_COMPONENT_COMPLETE(string masterPlanName, string villainName, string componentName, int orderComplete);

		void Send_Telemetry_EVT_MASTER_PLAN_TASK_COMPLETE(string componentName, string taskType, string requiredItem, int requiredQuantity);

		void Send_Telemetry_EVT_MTX_BOOKEND_EVENT(global::Kampai.Common.MtxBookendTelemetryInfo mtxInfo);

		void Send_Telemetry_CONTACT_US_CLICKED();

		void Send_Telemetry_EVT_AD_INTERACTION(global::Kampai.Game.AdPlacementName placementName, global::Kampai.Game.ItemDefinition reward, int timesRedeemedInCurrentDay);

		void Send_Telemetry_EVT_AD_INTERACTION(global::Kampai.Game.AdPlacementName placementName, global::System.Collections.Generic.IList<global::Kampai.Util.QuantityItem> rewards, int timesRedeemedInCurrentDay);

		void Send_Telemetry_EVT_AD_INTERACTION(string surfaceLocation, string rewardType, int timesRedeemedInCurrentDay);
	}
}
