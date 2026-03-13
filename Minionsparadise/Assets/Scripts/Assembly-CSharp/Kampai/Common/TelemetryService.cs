namespace Kampai.Common
{
	public class TelemetryService : global::Kampai.Common.IIapTelemetryService, global::Kampai.Common.ITelemetryService
	{
		private const string EVT_KEYTYPE_ENUMERATION = "EVT_KEYTYPE_ENUMERATION";

		private const string EVT_KEYTYPE_SCORE = "EVT_KEYTYPE_SCORE";

		private const string EVT_KEYTYPE_DURATION = "EVT_KEYTYPE_DURATION";

		private const string EVT_KEYTYPE_NONE = "EVT_KEYTYPE_NONE";

		private global::Kampai.Game.IPlayerService playerService;

		private global::Kampai.Game.IPlayerDurationService playerDurationService;

		private global::Kampai.Game.IDefinitionService definitionService;

		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("TelemetryService") as global::Kampai.Util.IKampaiLogger;

		private global::System.Collections.Generic.List<global::Kampai.Common.ITelemetrySender> telemetrySenders = new global::System.Collections.Generic.List<global::Kampai.Common.ITelemetrySender>();

		private global::System.Collections.Generic.List<global::Kampai.Common.IIapTelemetryService> iapTelemetryServices = new global::System.Collections.Generic.List<global::Kampai.Common.IIapTelemetryService>();

		private int gameStartTime;

		private float lastFunnelEalTime;

		private float lastGameLoadFunnelTime;

		[Inject]
		public ILocalPersistanceService localPersistService { get; set; }

		[Inject]
		public global::Kampai.Game.ITimeService timeService { get; set; }

		public void AddTelemetrySender(global::Kampai.Common.ITelemetrySender sender)
		{
			foreach (global::Kampai.Common.ITelemetrySender telemetrySender in telemetrySenders)
			{
				if (object.ReferenceEquals(telemetrySender, sender))
				{
					return;
				}
			}
			telemetrySenders.Add(sender);
		}

		public void AddIapTelemetryService(global::Kampai.Common.IIapTelemetryService service)
		{
			foreach (global::Kampai.Common.IIapTelemetryService iapTelemetryService in iapTelemetryServices)
			{
				if (object.ReferenceEquals(iapTelemetryService, service))
				{
					return;
				}
			}
			iapTelemetryServices.Add(service);
		}

		public void SharingUsage(global::Kampai.Common.ITelemetrySender sender, bool enabled)
		{
			foreach (global::Kampai.Common.ITelemetrySender telemetrySender in telemetrySenders)
			{
				if (object.ReferenceEquals(telemetrySender.GetType(), sender.GetType()))
				{
					telemetrySender.SharingUsage(SharingUsageEnabled() && enabled);
					break;
				}
			}
		}

		public void GameStarted()
		{
			gameStartTime = timeService.Uptime();
		}

		public int SecondsSinceGameStart()
		{
			return timeService.Uptime() - gameStartTime;
		}

		public virtual void LogGameEvent(global::Kampai.Common.TelemetryEvent gameEvent)
		{
			foreach (global::Kampai.Common.ITelemetrySender telemetrySender in telemetrySenders)
			{
				telemetrySender.SendEvent(gameEvent);
			}
		}

		public virtual void COPPACompliance()
		{
			foreach (global::Kampai.Common.ITelemetrySender telemetrySender in telemetrySenders)
			{
				telemetrySender.COPPACompliance();
			}
		}

		public void SharingUsageCompliance()
		{
			bool enabled = SharingUsageEnabled();
			foreach (global::Kampai.Common.ITelemetrySender telemetrySender in telemetrySenders)
			{
				telemetrySender.SharingUsage(enabled);
			}
		}

		public void SharingUsage(bool enabled)
		{
			localPersistService.PutDataIntPlayer("SharingUsage", enabled ? 1 : 0);
			foreach (global::Kampai.Common.ITelemetrySender telemetrySender in telemetrySenders)
			{
				telemetrySender.SharingUsage(enabled);
			}
		}

		public bool SharingUsageEnabled()
		{
			if (localPersistService.HasKeyPlayer("SharingUsage"))
			{
				return localPersistService.GetDataIntPlayer("SharingUsage") != 0;
			}
			return true;
		}

		public global::System.Collections.Generic.IList<global::Kampai.Common.TelemetryParameter> GetLevelGrindPremium()
		{
			global::System.Collections.Generic.IList<global::Kampai.Common.TelemetryParameter> list = new global::System.Collections.Generic.List<global::Kampai.Common.TelemetryParameter>();
			list.Add(getPlayerItem(global::Kampai.Game.StaticItem.LEVEL_ID, global::Kampai.Common.ParameterName.LEVEL));
			list.Add(getPlayerItem(global::Kampai.Game.StaticItem.GRIND_CURRENCY_ID, global::Kampai.Common.ParameterName.GRIND_CURRENCY_BALANCE));
			list.Add(getPlayerItem(global::Kampai.Game.StaticItem.PREMIUM_CURRENCY_ID, global::Kampai.Common.ParameterName.PREMIUM_CURRENCY_BALANCE));
			return list;
		}

		public global::Kampai.Common.TelemetryParameter getPlayerItem(global::Kampai.Game.StaticItem item, global::Kampai.Common.ParameterName name)
		{
			object value = getPlayerItemValue(item);
			return new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", value, name);
		}

		private uint getPlayerItemValue(global::Kampai.Game.StaticItem item)
		{
			return (playerService != null && playerService.IsPlayerInitialized()) ? playerService.GetQuantity(item) : 0u;
		}

		public global::System.Collections.Generic.IList<global::Kampai.Common.TelemetryParameter> GetTaxonomyParameters(global::Kampai.Game.TaxonomyDefinition taxonomyDefinition)
		{
			global::System.Collections.Generic.IList<global::Kampai.Common.TelemetryParameter> list = new global::System.Collections.Generic.List<global::Kampai.Common.TelemetryParameter>();
			string value = string.Empty;
			string value2 = string.Empty;
			string value3 = string.Empty;
			if (taxonomyDefinition != null)
			{
				value = taxonomyDefinition.TaxonomyHighLevel;
				value2 = taxonomyDefinition.TaxonomySpecific;
				value3 = taxonomyDefinition.TaxonomyType;
			}
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", value, global::Kampai.Common.ParameterName.TAXONOMY_HIGH_LEVEL));
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", value2, global::Kampai.Common.ParameterName.TAXONOMY_SPECIFIC));
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", value3, global::Kampai.Common.ParameterName.TAXONOMY_TYPE));
			return list;
		}

		public void GetTaxonomyParameters(global::Kampai.Game.TaxonomyDefinition taxonomyDefinition, out string highLevel, out string specific, out string type)
		{
			highLevel = string.Empty;
			specific = string.Empty;
			type = string.Empty;
			if (taxonomyDefinition != null)
			{
				highLevel = taxonomyDefinition.TaxonomyHighLevel;
				specific = taxonomyDefinition.TaxonomySpecific;
				type = taxonomyDefinition.TaxonomyType;
			}
		}

		private string GetDefinitionLocalizedKey(global::Kampai.Game.Definition definition)
		{
			return (!string.IsNullOrEmpty(definition.LocalizedKey)) ? definition.LocalizedKey : definition.ID.ToString();
		}

		public void Send_Telemetry_EVT_GAME_ERROR_GAMEPLAY(string nameOfError, string errorDetails, bool userFacing)
		{
			global::Kampai.Common.TelemetryEvent telemetryEvent = new global::Kampai.Common.TelemetryEvent(SynergyTrackingEventType.EVT_GAME_ERROR_GAMEPLAY);
			global::System.Collections.Generic.List<global::Kampai.Common.TelemetryParameter> list = new global::System.Collections.Generic.List<global::Kampai.Common.TelemetryParameter>();
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", nameOfError, global::Kampai.Common.ParameterName.ERROR_NAME));
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", errorDetails, global::Kampai.Common.ParameterName.ERROR_DETAILS));
			list.AddRange(GetLevelGrindPremium());
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", getUserFacingString(userFacing), global::Kampai.Common.ParameterName.USER_FACING));
			telemetryEvent.Parameters = list;
			LogGameEvent(telemetryEvent);
		}

		public string getUserFacingString(bool userFacing)
		{
			if (userFacing)
			{
				return "USER_FACING";
			}
			return "NOT_USER_FACING";
		}

		public void Send_Telemetry_EVT_GAME_ERROR_CRASH(string nameOfError, string crashReason, string crashTime, string errorDetails)
		{
			global::Kampai.Common.TelemetryEvent telemetryEvent = new global::Kampai.Common.TelemetryEvent(SynergyTrackingEventType.EVT_GAME_ERROR_CRASH);
			global::System.Collections.Generic.List<global::Kampai.Common.TelemetryParameter> list = new global::System.Collections.Generic.List<global::Kampai.Common.TelemetryParameter>();
			long num = 0L;
			if (playerService != null)
			{
				num = playerService.ID;
			}
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", nameOfError, global::Kampai.Common.ParameterName.ERROR_NAME));
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", crashReason, global::Kampai.Common.ParameterName.ERROR_REASON));
			list.AddRange(GetLevelGrindPremium());
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", errorDetails, global::Kampai.Common.ParameterName.ERROR_DETAILS));
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", crashTime, global::Kampai.Common.ParameterName.EVENT_TIMESTAMP));
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", num, global::Kampai.Common.ParameterName.PLAYER_ID));
			telemetryEvent.Parameters = list;
			LogGameEvent(telemetryEvent);
		}

		public void Send_Telemetry_EVT_GAME_ERROR_CONNECTIVITY(string nameOfError, string errorDetails, bool userFacing)
		{
			global::Kampai.Common.TelemetryEvent telemetryEvent = new global::Kampai.Common.TelemetryEvent(SynergyTrackingEventType.EVT_GAME_ERROR_CONNECTIVITY);
			global::System.Collections.Generic.List<global::Kampai.Common.TelemetryParameter> list = new global::System.Collections.Generic.List<global::Kampai.Common.TelemetryParameter>();
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", nameOfError, global::Kampai.Common.ParameterName.ERROR_NAME));
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", errorDetails, global::Kampai.Common.ParameterName.ERROR_DETAILS));
			list.AddRange(GetLevelGrindPremium());
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", getUserFacingString(userFacing), global::Kampai.Common.ParameterName.USER_FACING));
			telemetryEvent.Parameters = list;
			LogGameEvent(telemetryEvent);
		}

		public void Send_Telemetry_EVT_IGE_FREE_CREDITS_EARNED(int grindEarned, string eventName, bool purchasedCurrencySpent)
		{
			if (grindEarned != 0)
			{
				global::Kampai.Common.TelemetryEvent telemetryEvent = new global::Kampai.Common.TelemetryEvent(SynergyTrackingEventType.EVT_IGE_FREE_CREDITS_EARNED);
				global::System.Collections.Generic.List<global::Kampai.Common.TelemetryParameter> list = new global::System.Collections.Generic.List<global::Kampai.Common.TelemetryParameter>();
				list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_SCORE", grindEarned, global::Kampai.Common.ParameterName.GRIND_CURRENCY_EARNED));
				list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", eventName, global::Kampai.Common.ParameterName.EVENT_NAME));
				list.AddRange(GetLevelGrindPremium());
				list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", PremiumPurchaseArgument(purchasedCurrencySpent), global::Kampai.Common.ParameterName.CURRENCY_EARN_SPEND_TYPE));
				list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_NONE", string.Empty, global::Kampai.Common.ParameterName.NONE));
				list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_NONE", string.Empty, global::Kampai.Common.ParameterName.NONE));
				list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_NONE", string.Empty, global::Kampai.Common.ParameterName.NONE));
				list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", playerService.ID, global::Kampai.Common.ParameterName.PLAYER_ID));
				telemetryEvent.Parameters = list;
				LogGameEvent(telemetryEvent);
			}
		}

		public void Send_Telemetry_EVT_IGE_PAID_CREDITS_EARNED(int premiumEarned, string eventName, bool purchasedCurrencySpent)
		{
			if (premiumEarned != 0)
			{
				global::Kampai.Common.TelemetryEvent telemetryEvent = new global::Kampai.Common.TelemetryEvent(SynergyTrackingEventType.EVT_IGE_PAID_CREDITS_EARNED);
				global::System.Collections.Generic.List<global::Kampai.Common.TelemetryParameter> list = new global::System.Collections.Generic.List<global::Kampai.Common.TelemetryParameter>();
				list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_SCORE", premiumEarned, global::Kampai.Common.ParameterName.PREMIUM_CURRENCY_EARNED));
				list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", eventName, global::Kampai.Common.ParameterName.EVENT_NAME));
				list.AddRange(GetLevelGrindPremium());
				list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", PremiumPurchaseArgument(purchasedCurrencySpent), global::Kampai.Common.ParameterName.CURRENCY_EARN_SPEND_TYPE));
				list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_NONE", string.Empty, global::Kampai.Common.ParameterName.NONE));
				list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_NONE", string.Empty, global::Kampai.Common.ParameterName.NONE));
				list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_NONE", string.Empty, global::Kampai.Common.ParameterName.NONE));
				list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", playerService.ID, global::Kampai.Common.ParameterName.PLAYER_ID));
				telemetryEvent.Parameters = list;
				LogGameEvent(telemetryEvent);
			}
		}

		public void Send_Telemetry_EVT_IGE_FREE_CREDITS_PURCHASE_REVENUE(int grindSpent, string itemPurchased, bool purchasedCurrencySpent, string highLevel, string specific, string type)
		{
			if (grindSpent != 0)
			{
				global::Kampai.Common.TelemetryEvent telemetryEvent = new global::Kampai.Common.TelemetryEvent(SynergyTrackingEventType.EVT_IGE_FREE_CREDITS_PURCHASE_REVENUE);
				global::System.Collections.Generic.List<global::Kampai.Common.TelemetryParameter> list = new global::System.Collections.Generic.List<global::Kampai.Common.TelemetryParameter>();
				list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_SCORE", grindSpent, global::Kampai.Common.ParameterName.GRIND_CURRENCY_SPENT));
				list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", itemPurchased, global::Kampai.Common.ParameterName.ITEM_PURCHASED));
				list.AddRange(GetLevelGrindPremium());
				list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", PremiumPurchaseArgument(purchasedCurrencySpent), global::Kampai.Common.ParameterName.CURRENCY_EARN_SPEND_TYPE));
				list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", highLevel, global::Kampai.Common.ParameterName.TAXONOMY_HIGH_LEVEL));
				list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", specific, global::Kampai.Common.ParameterName.TAXONOMY_SPECIFIC));
				list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", type, global::Kampai.Common.ParameterName.TAXONOMY_TYPE));
				list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", playerService.ID, global::Kampai.Common.ParameterName.PLAYER_ID));
				telemetryEvent.Parameters = list;
				LogGameEvent(telemetryEvent);
			}
		}

		public void Send_Telemetry_EVT_IGE_PAID_CREDITS_PURCHASE_REVENUE(int premiumSpent, string itemPurchased, bool purchasedCurrencySpent, string highLevel, string specific, string type)
		{
			if (premiumSpent != 0)
			{
				global::Kampai.Common.TelemetryEvent telemetryEvent = new global::Kampai.Common.TelemetryEvent(SynergyTrackingEventType.EVT_IGE_PAID_CREDITS_PURCHASE_REVENUE);
				global::System.Collections.Generic.List<global::Kampai.Common.TelemetryParameter> list = new global::System.Collections.Generic.List<global::Kampai.Common.TelemetryParameter>();
				list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_SCORE", premiumSpent, global::Kampai.Common.ParameterName.PREMIUM_CURRENCY_SPENT));
				list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", itemPurchased, global::Kampai.Common.ParameterName.ITEM_PURCHASED));
				list.AddRange(GetLevelGrindPremium());
				list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", PremiumPurchaseArgument(purchasedCurrencySpent), global::Kampai.Common.ParameterName.CURRENCY_EARN_SPEND_TYPE));
				list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", highLevel, global::Kampai.Common.ParameterName.TAXONOMY_HIGH_LEVEL));
				list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", specific, global::Kampai.Common.ParameterName.TAXONOMY_SPECIFIC));
				list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", type, global::Kampai.Common.ParameterName.TAXONOMY_TYPE));
				list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", playerService.ID, global::Kampai.Common.ParameterName.PLAYER_ID));
				telemetryEvent.Parameters = list;
				LogGameEvent(telemetryEvent);
			}
		}

		public void Send_Telemetry_EVT_IGE_RESOURCE_CRAFTABLE_EARNED(int amount, string itemName, string itemType, string highLevel, string specific, string type)
		{
			global::Kampai.Common.TelemetryEvent telemetryEvent = new global::Kampai.Common.TelemetryEvent(SynergyTrackingEventType.EVT_IGE_RESOURCE_CRAFTABLE_EARNED);
			global::System.Collections.Generic.List<global::Kampai.Common.TelemetryParameter> list = new global::System.Collections.Generic.List<global::Kampai.Common.TelemetryParameter>();
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_SCORE", amount, global::Kampai.Common.ParameterName.AMOUNT));
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", itemName, global::Kampai.Common.ParameterName.ITEM_NAME));
			list.AddRange(GetLevelGrindPremium());
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", itemType, global::Kampai.Common.ParameterName.ITEM_TYPE));
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", highLevel, global::Kampai.Common.ParameterName.TAXONOMY_HIGH_LEVEL));
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", specific, global::Kampai.Common.ParameterName.TAXONOMY_SPECIFIC));
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", type, global::Kampai.Common.ParameterName.TAXONOMY_TYPE));
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", playerService.ID, global::Kampai.Common.ParameterName.PLAYER_ID));
			telemetryEvent.Parameters = list;
			LogGameEvent(telemetryEvent);
		}

		public void Send_Telemetry_EVT_IGE_RESOURCE_CRAFTABLE_SPENT(int amount, string sourceName, string itemName, string highLevel, string specific, string type)
		{
			global::Kampai.Common.TelemetryEvent telemetryEvent = new global::Kampai.Common.TelemetryEvent(SynergyTrackingEventType.EVT_IGE_RESOURCE_CRAFTABLE_SPENT);
			global::System.Collections.Generic.List<global::Kampai.Common.TelemetryParameter> list = new global::System.Collections.Generic.List<global::Kampai.Common.TelemetryParameter>();
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_SCORE", amount, global::Kampai.Common.ParameterName.AMOUNT));
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", itemName, global::Kampai.Common.ParameterName.ITEM_NAME));
			list.AddRange(GetLevelGrindPremium());
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", sourceName, global::Kampai.Common.ParameterName.SOURCE_NAME));
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", highLevel, global::Kampai.Common.ParameterName.TAXONOMY_HIGH_LEVEL));
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", specific, global::Kampai.Common.ParameterName.TAXONOMY_SPECIFIC));
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", type, global::Kampai.Common.ParameterName.TAXONOMY_TYPE));
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", playerService.ID, global::Kampai.Common.ParameterName.PLAYER_ID));
			telemetryEvent.Parameters = list;
			LogGameEvent(telemetryEvent);
		}

		public void Send_Telemetry_EVT_IGE_STORE_VISIT(string trafficSource, string storeVisited)
		{
			global::Kampai.Common.TelemetryEvent telemetryEvent = new global::Kampai.Common.TelemetryEvent(SynergyTrackingEventType.EVT_IGE_STORE_VISIT);
			global::System.Collections.Generic.List<global::Kampai.Common.TelemetryParameter> list = new global::System.Collections.Generic.List<global::Kampai.Common.TelemetryParameter>();
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", trafficSource, global::Kampai.Common.ParameterName.TRAFFIC_SOURCE));
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", storeVisited, global::Kampai.Common.ParameterName.STORE_VISITED));
			list.AddRange(GetLevelGrindPremium());
			telemetryEvent.Parameters = list;
			LogGameEvent(telemetryEvent);
		}

		public void Send_Telemetry_EVT_USER_TUTORIAL_FUNNEL_EAL(string tutorialName, string step)
		{
			int num = global::UnityEngine.Mathf.RoundToInt(global::UnityEngine.Time.realtimeSinceStartup - lastFunnelEalTime);
			lastFunnelEalTime = global::UnityEngine.Time.realtimeSinceStartup;
			global::Kampai.Common.TelemetryEvent telemetryEvent = new global::Kampai.Common.TelemetryEvent(SynergyTrackingEventType.EVT_USER_TUTORIAL_FUNNEL_EAL);
			global::System.Collections.Generic.List<global::Kampai.Common.TelemetryParameter> list = new global::System.Collections.Generic.List<global::Kampai.Common.TelemetryParameter>();
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", tutorialName, global::Kampai.Common.ParameterName.TUTORIAL_NAME));
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", step, global::Kampai.Common.ParameterName.STEP));
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_DURATION", num, global::Kampai.Common.ParameterName.DURATION));
			list.AddRange(GetLevelGrindPremium());
			telemetryEvent.Parameters = list;
			LogGameEvent(telemetryEvent);
		}

		public void Send_Telemetry_EVT_USER_GAME_LOAD_FUNNEL(string step, string swrveGroup, string performance)
		{
			float num = timeService.RealtimeSinceStartup();
			int num2 = global::UnityEngine.Mathf.RoundToInt(num - lastGameLoadFunnelTime);
			lastGameLoadFunnelTime = num;
			global::Kampai.Common.TelemetryEvent telemetryEvent = new global::Kampai.Common.TelemetryEvent(SynergyTrackingEventType.EVT_USER_GAME_LOAD_FUNNEL);
			global::System.Collections.Generic.List<global::Kampai.Common.TelemetryParameter> list = new global::System.Collections.Generic.List<global::Kampai.Common.TelemetryParameter>();
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_DURATION", num2, global::Kampai.Common.ParameterName.DURATION));
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", step, global::Kampai.Common.ParameterName.STEP));
			list.AddRange(GetLevelGrindPremium());
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", swrveGroup, global::Kampai.Common.ParameterName.SWRVE_GROUP));
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", performance, global::Kampai.Common.ParameterName.PERFORMANCE));
			telemetryEvent.Parameters = list;
			logger.Debug(string.Format("Send_Telemetry_EVT_USER_GAME_LOAD_FUNNEL: {0} - {1}", step, swrveGroup));
			LogGameEvent(telemetryEvent);
		}

		public void Send_Telemetry_EVT_MINION_POPULATION_BENEFIT(string benefit)
		{
			global::Kampai.Common.TelemetryEvent telemetryEvent = new global::Kampai.Common.TelemetryEvent(SynergyTrackingEventType.EVT_POPULATION_GOAL);
			global::System.Collections.Generic.List<global::Kampai.Common.TelemetryParameter> list = new global::System.Collections.Generic.List<global::Kampai.Common.TelemetryParameter>();
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", benefit, global::Kampai.Common.ParameterName.POPULATION_GOAL_UNLOCKED));
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_NONE", string.Empty, global::Kampai.Common.ParameterName.NONE));
			list.AddRange(GetLevelGrindPremium());
			telemetryEvent.Parameters = list;
			LogGameEvent(telemetryEvent);
		}

		public void Send_Telemetry_EVT_MINION_UPGRADE(int newLevel, int tokensUsed, uint tokensBeforeUpgrade)
		{
			global::Kampai.Common.TelemetryEvent telemetryEvent = new global::Kampai.Common.TelemetryEvent(SynergyTrackingEventType.EVT_MINION_LEVEL);
			global::System.Collections.Generic.List<global::Kampai.Common.TelemetryParameter> list = new global::System.Collections.Generic.List<global::Kampai.Common.TelemetryParameter>();
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", newLevel, global::Kampai.Common.ParameterName.NEW_MINION_LEVEL));
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", tokensBeforeUpgrade, global::Kampai.Common.ParameterName.MINION_UPGRADE_TOKENS));
			list.AddRange(GetLevelGrindPremium());
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", tokensUsed, global::Kampai.Common.ParameterName.UPGRADE_TOKENS_CONSUMED));
			telemetryEvent.Parameters = list;
			LogGameEvent(telemetryEvent);
		}

		public void Send_Telemetry_EVT_USER_GAME_DOWNLOAD_FUNNEL(string bundleName, int duration, long size, bool isNetworkWifi)
		{
			global::Kampai.Common.TelemetryEvent telemetryEvent = new global::Kampai.Common.TelemetryEvent(SynergyTrackingEventType.EVT_USER_GAME_DOWNLOAD_FUNNEL);
			global::System.Collections.Generic.List<global::Kampai.Common.TelemetryParameter> list = new global::System.Collections.Generic.List<global::Kampai.Common.TelemetryParameter>();
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", bundleName, global::Kampai.Common.ParameterName.DLC_NAME));
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_DURATION", duration, global::Kampai.Common.ParameterName.DURATION));
			list.AddRange(GetLevelGrindPremium());
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_SCORE", size, global::Kampai.Common.ParameterName.DLC_SIZE));
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", isNetworkWifi.ToString(), global::Kampai.Common.ParameterName.DLC_IS_WIFI));
			telemetryEvent.Parameters = list;
			logger.Debug(string.Format("Send_Telemetry_EVT_USER_GAME_DOWNLOAD_FUNNEL: {0} in {1}", bundleName, duration));
			LogGameEvent(telemetryEvent);
		}

		public void Send_Telemetry_EVT_GP_LEVEL_PROMOTION()
		{
			int totalSecondsSinceLevelUp = playerDurationService.TotalSecondsSinceLevelUp;
			int gameplaySecondsSinceLevelUp = playerDurationService.GameplaySecondsSinceLevelUp;
			logger.Info(string.Format("LEVELUP TELEMETRY: TOTAL SECONDS: {0}", totalSecondsSinceLevelUp));
			logger.Info(string.Format("LEVELUP TELEMETRY: GAMEPLAY SECONDS: {0}", gameplaySecondsSinceLevelUp));
			global::Kampai.Common.TelemetryEvent telemetryEvent = new global::Kampai.Common.TelemetryEvent(SynergyTrackingEventType.EVT_GP_LEVEL_PROMOTION);
			global::System.Collections.Generic.List<global::Kampai.Common.TelemetryParameter> list = new global::System.Collections.Generic.List<global::Kampai.Common.TelemetryParameter>();
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_DURATION", totalSecondsSinceLevelUp, global::Kampai.Common.ParameterName.DURATION_TOTAL));
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_DURATION", gameplaySecondsSinceLevelUp, global::Kampai.Common.ParameterName.DURATION_GAMEPLAY));
			list.AddRange(GetLevelGrindPremium());
			telemetryEvent.Parameters = list;
			LogGameEvent(telemetryEvent);
		}

		private global::System.Collections.Generic.IEnumerable<global::Kampai.Common.TelemetryParameter> GetFtueParameters()
		{
			int ftueLevel = playerService.GetHighestFtueCompleted();
			if (ftueLevel < 999999)
			{
				yield return new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", "FTUE", global::Kampai.Common.ParameterName.FTUE_LEVEL);
				yield return new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", ftueLevel.ToString(), global::Kampai.Common.ParameterName.FTUE_LEVEL);
			}
		}

		public void Send_Telemetry_EVT_GP_ACHIEVEMENTS_CHECKPOINTS_EAL(string achievementName, global::Kampai.Common.Service.Telemetry.TelemetryAchievementType type, int PartyPointsEarned, string questGiver = "")
		{
			global::Kampai.Common.TelemetryEvent telemetryEvent = new global::Kampai.Common.TelemetryEvent(SynergyTrackingEventType.EVT_GP_ACHIEVEMENTS_CHECKPOINTS_EAL);
			global::System.Collections.Generic.List<global::Kampai.Common.TelemetryParameter> list = new global::System.Collections.Generic.List<global::Kampai.Common.TelemetryParameter>();
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", achievementName, global::Kampai.Common.ParameterName.ACHIEVEMENT_NAME));
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", type.ToString(), global::Kampai.Common.ParameterName.ARCHIVEMENT_TYPE));
			list.AddRange(GetLevelGrindPremium());
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", PartyPointsEarned, global::Kampai.Common.ParameterName.AMOUNT_PARTY_POINTS_EARNED));
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", questGiver, global::Kampai.Common.ParameterName.QUEST_GIVER));
			list.AddRange(GetFtueParameters());
			telemetryEvent.Parameters = list;
			LogGameEvent(telemetryEvent);
		}

		public void Send_Telemetry_EVT_GP_ACHIEVEMENTS_CHECKPOINTS_EAL_ProceduralQuest(string achievementName, global::Kampai.Common.Service.Telemetry.ProceduralQuestEndState endState, int PartyPointsEarned)
		{
			global::Kampai.Common.Service.Telemetry.TelemetryAchievementType telemetryAchievementType = global::Kampai.Common.Service.Telemetry.TelemetryAchievementType.ProceduralQuest;
			global::Kampai.Common.TelemetryEvent telemetryEvent = new global::Kampai.Common.TelemetryEvent(SynergyTrackingEventType.EVT_GP_ACHIEVEMENTS_CHECKPOINTS_EAL);
			global::System.Collections.Generic.List<global::Kampai.Common.TelemetryParameter> list = new global::System.Collections.Generic.List<global::Kampai.Common.TelemetryParameter>();
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", achievementName, global::Kampai.Common.ParameterName.ACHIEVEMENT_NAME));
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", telemetryAchievementType.ToString(), global::Kampai.Common.ParameterName.ARCHIVEMENT_TYPE));
			list.AddRange(GetLevelGrindPremium());
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", PartyPointsEarned, global::Kampai.Common.ParameterName.AMOUNT_PARTY_POINTS_EARNED));
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", string.Empty, global::Kampai.Common.ParameterName.QUEST_GIVER));
			list.AddRange(GetFtueParameters());
			telemetryEvent.Parameters = list;
			LogGameEvent(telemetryEvent);
		}

		public void Send_Telemetry_EVT_GP_ACHIEVEMENTS_STARTED_EAL(string achievementName, global::Kampai.Common.Service.Telemetry.TelemetryAchievementType type, string questGiver = "")
		{
			global::Kampai.Common.TelemetryEvent telemetryEvent = new global::Kampai.Common.TelemetryEvent(SynergyTrackingEventType.ACHIEVEMENT_OR_QUEST_STARTED);
			global::System.Collections.Generic.List<global::Kampai.Common.TelemetryParameter> list = new global::System.Collections.Generic.List<global::Kampai.Common.TelemetryParameter>();
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", achievementName, global::Kampai.Common.ParameterName.ACHIEVEMENT_NAME));
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", type.ToString(), global::Kampai.Common.ParameterName.ARCHIVEMENT_TYPE));
			list.AddRange(GetLevelGrindPremium());
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", string.Empty, global::Kampai.Common.ParameterName.PROCEDURAL_QUEST_END_STATE));
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", questGiver, global::Kampai.Common.ParameterName.QUEST_GIVER));
			telemetryEvent.Parameters = list;
			LogGameEvent(telemetryEvent);
		}

		public void Send_Telemetry_EVT_EBISU_LOGIN_GAMECENTER(string loginLocation)
		{
			global::Kampai.Common.TelemetryEvent telemetryEvent = new global::Kampai.Common.TelemetryEvent(SynergyTrackingEventType.EVT_GC_SIGNON);
			global::System.Collections.Generic.List<global::Kampai.Common.TelemetryParameter> list = new global::System.Collections.Generic.List<global::Kampai.Common.TelemetryParameter>();
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", loginLocation, global::Kampai.Common.ParameterName.LOGIN_LOCATION));
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_NONE", string.Empty, global::Kampai.Common.ParameterName.NONE));
			list.AddRange(GetLevelGrindPremium());
			telemetryEvent.Parameters = list;
			LogGameEvent(telemetryEvent);
		}

		public void Send_Telemetry_EVT_EBISU_LOGIN_GOOGLEPLAY(string loginLocation)
		{
			global::Kampai.Common.TelemetryEvent telemetryEvent = new global::Kampai.Common.TelemetryEvent(SynergyTrackingEventType.EVT_GP_SIGNON);
			global::System.Collections.Generic.List<global::Kampai.Common.TelemetryParameter> list = new global::System.Collections.Generic.List<global::Kampai.Common.TelemetryParameter>();
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", loginLocation, global::Kampai.Common.ParameterName.LOGIN_LOCATION));
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_NONE", string.Empty, global::Kampai.Common.ParameterName.NONE));
			list.AddRange(GetLevelGrindPremium());
			telemetryEvent.Parameters = list;
			LogGameEvent(telemetryEvent);
		}

		public void Send_Telemetry_EVT_EBISU_LOGIN_FACEBOOK(string loginLocation, string loginSource)
		{
			global::Kampai.Common.TelemetryEvent telemetryEvent = new global::Kampai.Common.TelemetryEvent(SynergyTrackingEventType.EVT_FB_SIGNON);
			global::System.Collections.Generic.List<global::Kampai.Common.TelemetryParameter> list = new global::System.Collections.Generic.List<global::Kampai.Common.TelemetryParameter>();
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", loginLocation, global::Kampai.Common.ParameterName.LOGIN_LOCATION));
			if (string.IsNullOrEmpty(loginSource))
			{
				list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_NONE", string.Empty, global::Kampai.Common.ParameterName.NONE));
			}
			else
			{
				list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", loginSource, global::Kampai.Common.ParameterName.LOGIN_SOURCE));
			}
			list.AddRange(GetLevelGrindPremium());
			telemetryEvent.Parameters = list;
			LogGameEvent(telemetryEvent);
		}

		public void Send_Telemetry_EVT_AGE_GATE_SET(int year, int month)
		{
			global::Kampai.Common.TelemetryEvent telemetryEvent = new global::Kampai.Common.TelemetryEvent(SynergyTrackingEventType.EVT_COPPA_AGE_GATE);
			global::System.Collections.Generic.List<global::Kampai.Common.TelemetryParameter> list = new global::System.Collections.Generic.List<global::Kampai.Common.TelemetryParameter>();
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", year, global::Kampai.Common.ParameterName.YEAR));
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", month, global::Kampai.Common.ParameterName.MONTH));
			telemetryEvent.Parameters = list;
			LogGameEvent(telemetryEvent);
		}

		public void Send_TelemetryCharacterPrestiged(global::Kampai.Game.Prestige prestige)
		{
			global::Kampai.Common.TelemetryEvent telemetryEvent = new global::Kampai.Common.TelemetryEvent(SynergyTrackingEventType.EVT_CHARACTER_PRESTIGED);
			global::System.Collections.Generic.List<global::Kampai.Common.TelemetryParameter> list = new global::System.Collections.Generic.List<global::Kampai.Common.TelemetryParameter>();
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", prestige.Definition.LocalizedKey, global::Kampai.Common.ParameterName.CHARACTER_NAME));
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", prestige.CurrentOrdersCompleted, global::Kampai.Common.ParameterName.ORDERS_COMPLETED));
			list.AddRange(GetLevelGrindPremium());
			telemetryEvent.Parameters = list;
			LogGameEvent(telemetryEvent);
		}

		public void Send_TelemetryOrderBoard(bool isFillingOrder, global::Kampai.Game.Transaction.TransactionDefinition transactionDef, int characterDefinitionId)
		{
			SynergyTrackingEventType type = ((!isFillingOrder) ? SynergyTrackingEventType.EVT_ORDER_CANCEL_SUM : SynergyTrackingEventType.EVT_ORDER_COMPLETED_SUM);
			global::Kampai.Common.TelemetryEvent telemetryEvent = new global::Kampai.Common.TelemetryEvent(type);
			global::System.Collections.Generic.List<global::Kampai.Common.TelemetryParameter> list = new global::System.Collections.Generic.List<global::Kampai.Common.TelemetryParameter>();
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", string.Empty, global::Kampai.Common.ParameterName.NONE));
			string value = "none";
			if (characterDefinitionId != 0 && definitionService != null)
			{
				global::Kampai.Game.PrestigeDefinition definition = null;
				if (definitionService.TryGet<global::Kampai.Game.PrestigeDefinition>(characterDefinitionId, out definition))
				{
					value = string.Format("Prestiged with {0}", (definition == null) ? "uknown" : definition.LocalizedKey);
				}
			}
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", value, global::Kampai.Common.ParameterName.PRESTIGED_WITH));
			list.AddRange(GetLevelGrindPremium());
			uint num = 0u;
			uint num2 = 0u;
			foreach (global::Kampai.Util.QuantityItem output in transactionDef.Outputs)
			{
				if (output.ID == 0)
				{
					num = output.Quantity;
				}
				else if (output.ID == 2)
				{
					num2 = output.Quantity;
				}
			}
			int count = transactionDef.Inputs.Count;
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", num, global::Kampai.Common.ParameterName.GRIND_CURRENCY_EARNED));
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", num2, global::Kampai.Common.ParameterName.XP_EARNED));
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", count, global::Kampai.Common.ParameterName.INPUTS_COUNT));
			telemetryEvent.Parameters = list;
			LogGameEvent(telemetryEvent);
			type = ((!isFillingOrder) ? SynergyTrackingEventType.EVT_ORDER_CANCEL_DET : SynergyTrackingEventType.EVT_ORDER_COMPLETED_DET);
			telemetryEvent = new global::Kampai.Common.TelemetryEvent(type);
			list = new global::System.Collections.Generic.List<global::Kampai.Common.TelemetryParameter>();
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", string.Empty, global::Kampai.Common.ParameterName.NONE));
			if (count > 0)
			{
				AddIngredient(transactionDef.Inputs[0], list);
			}
			else
			{
				list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", "none:0", global::Kampai.Common.ParameterName.INGREDIENT));
			}
			list.AddRange(GetLevelGrindPremium());
			for (int i = 1; i < count; i++)
			{
				AddIngredient(transactionDef.Inputs[i], list);
			}
			telemetryEvent.Parameters = list;
			LogGameEvent(telemetryEvent);
		}

		private void AddIngredient(global::Kampai.Util.QuantityItem item, global::System.Collections.Generic.List<global::Kampai.Common.TelemetryParameter> parameters)
		{
			global::Kampai.Game.ItemDefinition definition;
			if (definitionService != null && definitionService.TryGet<global::Kampai.Game.ItemDefinition>(item.ID, out definition))
			{
				string value = string.Format("{0}:{1}", (definition != null) ? definition.LocalizedKey : "unknown", item.Quantity);
				parameters.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", value, global::Kampai.Common.ParameterName.INGREDIENT));
			}
		}

		public void Send_Telemetry_EVT_IN_APP_MESSAGE_DISPLAYED(string inAppMessageName, global::Kampai.Main.HindsightCampaign.DismissType choice)
		{
			global::Kampai.Common.TelemetryEvent telemetryEvent = new global::Kampai.Common.TelemetryEvent(SynergyTrackingEventType.EVT_IPAD_UPSELL_MESSAGE_DISPLAYED);
			global::System.Collections.Generic.List<global::Kampai.Common.TelemetryParameter> list = new global::System.Collections.Generic.List<global::Kampai.Common.TelemetryParameter>();
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", inAppMessageName, global::Kampai.Common.ParameterName.IN_APP_MESSAGE_NAME));
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", choice, global::Kampai.Common.ParameterName.USER_CHOICE));
			list.AddRange(GetLevelGrindPremium());
			telemetryEvent.Parameters = list;
			LogGameEvent(telemetryEvent);
		}

		public void Send_Telemetry_EVT_USER_TRACKING_OPTOUT()
		{
			global::Kampai.Common.TelemetryEvent telemetryEvent = new global::Kampai.Common.TelemetryEvent(SynergyTrackingEventType.EVT_USER_TRACKING_OPTOUT);
			global::System.Collections.Generic.List<global::Kampai.Common.TelemetryParameter> list = new global::System.Collections.Generic.List<global::Kampai.Common.TelemetryParameter>();
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_NONE", string.Empty, global::Kampai.Common.ParameterName.NONE));
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_NONE", string.Empty, global::Kampai.Common.ParameterName.NONE));
			list.AddRange(GetLevelGrindPremium());
			telemetryEvent.Parameters = list;
			LogGameEvent(telemetryEvent);
		}

		public void Send_Telemetry_EVT_MINI_GAME_PLAYED(string mignetteName, int score, float timePlayed, int xpReward)
		{
			global::Kampai.Common.TelemetryEvent telemetryEvent = new global::Kampai.Common.TelemetryEvent(SynergyTrackingEventType.EVT_MINI_GAME_PLAYED);
			global::System.Collections.Generic.List<global::Kampai.Common.TelemetryParameter> list = new global::System.Collections.Generic.List<global::Kampai.Common.TelemetryParameter>();
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", mignetteName, global::Kampai.Common.ParameterName.MIGNETTE_NAME));
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", score, global::Kampai.Common.ParameterName.SCORE));
			list.AddRange(GetLevelGrindPremium());
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", timePlayed, global::Kampai.Common.ParameterName.TIME_PLAYED));
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", xpReward, global::Kampai.Common.ParameterName.AMOUNT_PARTY_POINTS_EARNED));
			telemetryEvent.Parameters = list;
			LogGameEvent(telemetryEvent);
		}

		private global::Kampai.Common.TelemetryParameter IsSpender()
		{
			string dataPlayer = localPersistService.GetDataPlayer("IsSpender");
			if (dataPlayer.Equals("true"))
			{
				return new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", "spender", global::Kampai.Common.ParameterName.SPENDER);
			}
			return new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", "non-spender", global::Kampai.Common.ParameterName.SPENDER);
		}

		public void Send_Telemetry_EVT_USER_DATA_AT_APP_START(int seconds, int tokenCount, int minions, string swrveGroup, string expansions)
		{
			logger.Debug(string.Format("Send_Telemetry_EVT_USER_DATA_AT_APP_START: {0} {1} {2} {3} {4}", seconds, tokenCount, minions, swrveGroup, expansions));
			global::Kampai.Common.TelemetryEvent telemetryEvent = new global::Kampai.Common.TelemetryEvent(SynergyTrackingEventType.EVT_USER_DATA_AT_APP_START);
			global::System.Collections.Generic.List<global::Kampai.Common.TelemetryParameter> list = new global::System.Collections.Generic.List<global::Kampai.Common.TelemetryParameter>();
			long iD = playerService.ID;
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", seconds, global::Kampai.Common.ParameterName.TIME_SINCE_LAST_PLAYED));
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", tokenCount, global::Kampai.Common.ParameterName.MINION_UPGRADE_TOKENS));
			list.AddRange(GetLevelGrindPremium());
			list.Add(IsSpender());
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", minions, global::Kampai.Common.ParameterName.NUM_MINIONS));
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", swrveGroup, global::Kampai.Common.ParameterName.SWRVE_GROUP));
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", expansions, global::Kampai.Common.ParameterName.LAND_EXPANSIONS));
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", iD, global::Kampai.Common.ParameterName.PLAYER_ID));
			telemetryEvent.Parameters = list;
			LogGameEvent(telemetryEvent);
		}

		public void Send_Telemetry_EVT_USER_DATA_AT_APP_CLOSE()
		{
			global::Kampai.Common.TelemetryEvent telemetryEvent = new global::Kampai.Common.TelemetryEvent(SynergyTrackingEventType.EVT_USER_DATA_AT_APP_CLOSE);
			global::System.Collections.Generic.List<global::Kampai.Common.TelemetryParameter> list = new global::System.Collections.Generic.List<global::Kampai.Common.TelemetryParameter>();
			long iD = playerService.ID;
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_NONE", string.Empty, global::Kampai.Common.ParameterName.NONE));
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_NONE", string.Empty, global::Kampai.Common.ParameterName.NONE));
			list.AddRange(GetLevelGrindPremium());
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_NONE", string.Empty, global::Kampai.Common.ParameterName.NONE));
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_NONE", string.Empty, global::Kampai.Common.ParameterName.NONE));
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_NONE", string.Empty, global::Kampai.Common.ParameterName.NONE));
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_NONE", string.Empty, global::Kampai.Common.ParameterName.NONE));
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", iD, global::Kampai.Common.ParameterName.PLAYER_ID));
			telemetryEvent.Parameters = list;
			LogGameEvent(telemetryEvent);
		}

		public void Send_Telemetry_EVT_STORAGE_LIMIT_HIT(int storageLimit)
		{
			global::Kampai.Common.TelemetryEvent telemetryEvent = new global::Kampai.Common.TelemetryEvent(SynergyTrackingEventType.EVT_STORAGE_LIMIT_HIT);
			global::System.Collections.Generic.List<global::Kampai.Common.TelemetryParameter> list = new global::System.Collections.Generic.List<global::Kampai.Common.TelemetryParameter>();
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", storageLimit, global::Kampai.Common.ParameterName.STORAGE_LIMIT));
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_NONE", string.Empty, global::Kampai.Common.ParameterName.NONE));
			list.AddRange(GetLevelGrindPremium());
			telemetryEvent.Parameters = list;
			LogGameEvent(telemetryEvent);
		}

		public void Send_Telemetry_EVT_SOCIAL_EVENT_COMPLETION(int teamSize)
		{
			global::Kampai.Common.TelemetryEvent telemetryEvent = new global::Kampai.Common.TelemetryEvent(SynergyTrackingEventType.EVT_SOCIAL_EVENT_COMPLETION);
			global::System.Collections.Generic.List<global::Kampai.Common.TelemetryParameter> list = new global::System.Collections.Generic.List<global::Kampai.Common.TelemetryParameter>();
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_NONE", string.Empty, global::Kampai.Common.ParameterName.NONE));
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_NONE", string.Empty, global::Kampai.Common.ParameterName.NONE));
			list.AddRange(GetLevelGrindPremium());
			if (teamSize == 1)
			{
				list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", "solo", global::Kampai.Common.ParameterName.SOLO_TEAM));
			}
			else
			{
				list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", "team", global::Kampai.Common.ParameterName.SOLO_TEAM));
			}
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", teamSize, global::Kampai.Common.ParameterName.TEAM_SIZE));
			telemetryEvent.Parameters = list;
			LogGameEvent(telemetryEvent);
		}

		public void Send_Telemetry_EVT_SOCIAL_EVENT_CONTRIBUTION(string item, int quantity, int teamSize, int xpReward)
		{
			global::Kampai.Common.TelemetryEvent telemetryEvent = new global::Kampai.Common.TelemetryEvent(SynergyTrackingEventType.EVT_SOCIAL_EVENT_CONTRIBUTION);
			global::System.Collections.Generic.List<global::Kampai.Common.TelemetryParameter> list = new global::System.Collections.Generic.List<global::Kampai.Common.TelemetryParameter>();
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", item, global::Kampai.Common.ParameterName.ITEM_NAME));
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", quantity, global::Kampai.Common.ParameterName.AMOUNT));
			list.AddRange(GetLevelGrindPremium());
			if (teamSize == 1)
			{
				list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", "solo", global::Kampai.Common.ParameterName.SOLO_TEAM));
			}
			else
			{
				list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", "team", global::Kampai.Common.ParameterName.SOLO_TEAM));
			}
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", teamSize, global::Kampai.Common.ParameterName.TEAM_SIZE));
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", xpReward, global::Kampai.Common.ParameterName.AMOUNT_PARTY_POINTS_EARNED));
			telemetryEvent.Parameters = list;
			LogGameEvent(telemetryEvent);
		}

		public void Send_Telemtry_EVT_MINI_TIER_REACHED(string mignetteName, int tier, int plays)
		{
			global::Kampai.Common.TelemetryEvent telemetryEvent = new global::Kampai.Common.TelemetryEvent(SynergyTrackingEventType.EVT_MINI_TIER_REACHED);
			global::System.Collections.Generic.List<global::Kampai.Common.TelemetryParameter> list = new global::System.Collections.Generic.List<global::Kampai.Common.TelemetryParameter>();
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", mignetteName, global::Kampai.Common.ParameterName.MIGNETTE_NAME));
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", tier, global::Kampai.Common.ParameterName.REWARD_TIER));
			list.AddRange(GetLevelGrindPremium());
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", plays, global::Kampai.Common.ParameterName.TIMES_PLAYED));
			telemetryEvent.Parameters = list;
			LogGameEvent(telemetryEvent);
		}

		public void Send_Telemtry_EVT_MARKETPLACE_ITEM_LISTED(string itemName, int quantity, int price, string highLevel, string specific, string type, string other)
		{
			global::Kampai.Common.TelemetryEvent telemetryEvent = new global::Kampai.Common.TelemetryEvent(SynergyTrackingEventType.EVT_MARKETPLACE_ITEM_LISTED);
			global::System.Collections.Generic.List<global::Kampai.Common.TelemetryParameter> list = new global::System.Collections.Generic.List<global::Kampai.Common.TelemetryParameter>();
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", itemName, global::Kampai.Common.ParameterName.ITEM_NAME));
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", quantity, global::Kampai.Common.ParameterName.AMOUNT));
			list.AddRange(GetLevelGrindPremium());
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", price, global::Kampai.Common.ParameterName.PRICE_LISTED_AT));
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", highLevel, global::Kampai.Common.ParameterName.TAXONOMY_HIGH_LEVEL));
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", specific, global::Kampai.Common.ParameterName.TAXONOMY_SPECIFIC));
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", type, global::Kampai.Common.ParameterName.TAXONOMY_TYPE));
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", other, global::Kampai.Common.ParameterName.TAXONOMY_OTHER));
			telemetryEvent.Parameters = list;
			LogGameEvent(telemetryEvent);
		}

		public void Send_Telemtry_EVT_MARKETPLACE_VIEWED(string viewType)
		{
			global::Kampai.Common.TelemetryEvent telemetryEvent = new global::Kampai.Common.TelemetryEvent(SynergyTrackingEventType.EVT_MARKETPLACE_VIEWED);
			global::System.Collections.Generic.List<global::Kampai.Common.TelemetryParameter> list = new global::System.Collections.Generic.List<global::Kampai.Common.TelemetryParameter>();
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", viewType, global::Kampai.Common.ParameterName.VIEW_TYPE));
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_NONE", string.Empty, global::Kampai.Common.ParameterName.NONE));
			list.AddRange(GetLevelGrindPremium());
			telemetryEvent.Parameters = list;
			LogGameEvent(telemetryEvent);
		}

		public void Send_Telemetry_EVT_MINION_PARTY_STARTED(int totalPartyPoints, string buffSelected, string guestOfHonor, bool isInspiredParty)
		{
			global::Kampai.Common.TelemetryEvent telemetryEvent = new global::Kampai.Common.TelemetryEvent(SynergyTrackingEventType.EVT_MINION_PARTY_STARTED);
			global::System.Collections.Generic.List<global::Kampai.Common.TelemetryParameter> list = new global::System.Collections.Generic.List<global::Kampai.Common.TelemetryParameter>();
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", totalPartyPoints, global::Kampai.Common.ParameterName.TOTAL_PARTY_POINTS_EARNED));
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", buffSelected, global::Kampai.Common.ParameterName.PARTY_BUFF_TYPE));
			list.AddRange(GetLevelGrindPremium());
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", guestOfHonor, global::Kampai.Common.ParameterName.PARTY_GUEST_OF_HONOR));
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", isInspiredParty ? 1 : 0, global::Kampai.Common.ParameterName.PARTY_IS_INSPIRED));
			telemetryEvent.Parameters = list;
			LogGameEvent(telemetryEvent);
		}

		public void Send_Telemetry_EVT_PARTY_POINTS_EARNED(int amountOfPartyPoints, string sourceName)
		{
			global::Kampai.Common.TelemetryEvent telemetryEvent = new global::Kampai.Common.TelemetryEvent(SynergyTrackingEventType.EVT_PARTY_POINTS_EARNED);
			global::System.Collections.Generic.List<global::Kampai.Common.TelemetryParameter> list = new global::System.Collections.Generic.List<global::Kampai.Common.TelemetryParameter>();
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", amountOfPartyPoints, global::Kampai.Common.ParameterName.AMOUNT_PARTY_POINTS_EARNED));
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", sourceName, global::Kampai.Common.ParameterName.PARTY_POINTS_EARNED_FROM));
			list.AddRange(GetLevelGrindPremium());
			telemetryEvent.Parameters = list;
			LogGameEvent(telemetryEvent);
		}

		public void Send_Telemetry_EVT_NOTE_SETTING_CHANGE(string settingName, string enabled, string sourceName)
		{
			global::Kampai.Common.TelemetryEvent telemetryEvent = new global::Kampai.Common.TelemetryEvent(SynergyTrackingEventType.EVT_NOTE_SETTING_CHANGE);
			global::System.Collections.Generic.List<global::Kampai.Common.TelemetryParameter> list = new global::System.Collections.Generic.List<global::Kampai.Common.TelemetryParameter>();
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", settingName, global::Kampai.Common.ParameterName.NOTIFICATION_CHANGED_NAME));
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", enabled, global::Kampai.Common.ParameterName.NOTIFICATION_ENABLED));
			list.AddRange(GetLevelGrindPremium());
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", sourceName, global::Kampai.Common.ParameterName.NOTIFICATION_CHANGED_FROM));
			telemetryEvent.Parameters = list;
			LogGameEvent(telemetryEvent);
		}

		public void Send_Telemetry_EVT_PLAYER_TRAINING(int triggeredID, int fromSettings, int timeOpen)
		{
			global::Kampai.Common.TelemetryEvent telemetryEvent = new global::Kampai.Common.TelemetryEvent(SynergyTrackingEventType.EVT_PLAYER_TRAINING);
			global::System.Collections.Generic.List<global::Kampai.Common.TelemetryParameter> list = new global::System.Collections.Generic.List<global::Kampai.Common.TelemetryParameter>();
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", triggeredID, global::Kampai.Common.ParameterName.TRIGGERED_DEF_ID));
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", fromSettings, global::Kampai.Common.ParameterName.SOURCE_OF_TRAINING));
			list.AddRange(GetLevelGrindPremium());
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", timeOpen, global::Kampai.Common.ParameterName.TIME_SPENT_ON_TRAINING));
			telemetryEvent.Parameters = list;
			LogGameEvent(telemetryEvent);
		}

		public void SetPlayerServiceReference(global::Kampai.Game.IPlayerService playerService)
		{
			this.playerService = playerService;
		}

		public void SetPlayerDurationServiceReference(global::Kampai.Game.IPlayerDurationService playerDurationService)
		{
			this.playerDurationService = playerDurationService;
		}

		public void SetDefinitionServiceReference(global::Kampai.Game.IDefinitionService definitionService)
		{
			this.definitionService = definitionService;
		}

		public void Send_Telemetry_EVT_RATE_MY_APP(string promptType, bool? userAccepted)
		{
			global::Kampai.Common.TelemetryEvent telemetryEvent = new global::Kampai.Common.TelemetryEvent(SynergyTrackingEventType.EVT_RATE_MY_APP);
			global::System.Collections.Generic.List<global::Kampai.Common.TelemetryParameter> list = new global::System.Collections.Generic.List<global::Kampai.Common.TelemetryParameter>();
			string value = "Cancel";
			if (userAccepted.HasValue)
			{
				value = ((!userAccepted.Value) ? "No" : "Yes");
			}
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", promptType, global::Kampai.Common.ParameterName.PROMPT_TYPE));
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", value, global::Kampai.Common.ParameterName.USER_CHOICE));
			list.AddRange(GetLevelGrindPremium());
			telemetryEvent.Parameters = list;
			LogGameEvent(telemetryEvent);
		}

		public void SendInAppPurchaseEventOnPurchaseComplete(global::Kampai.Common.IapTelemetryEvent iapTelemetryEvent)
		{
			foreach (global::Kampai.Common.IIapTelemetryService iapTelemetryService in iapTelemetryServices)
			{
				iapTelemetryService.SendInAppPurchaseEventOnPurchaseComplete(iapTelemetryEvent);
			}
		}

		public void SendInAppPurchaseEventOnProductDelivery(string sku, global::Kampai.Game.Transaction.TransactionDefinition reward)
		{
			foreach (global::Kampai.Common.IIapTelemetryService iapTelemetryService in iapTelemetryServices)
			{
				iapTelemetryService.SendInAppPurchaseEventOnProductDelivery(sku, reward);
			}
		}

		private string PremiumPurchaseArgument(bool isPremium)
		{
			return (!isPremium) ? "FREE" : "TRUE";
		}

		public void Send_Telemetry_EVT_PARTY_SKIPPED()
		{
			global::Kampai.Common.TelemetryEvent telemetryEvent = new global::Kampai.Common.TelemetryEvent(SynergyTrackingEventType.EVT_PARTY_SKIPPED);
			global::System.Collections.Generic.List<global::Kampai.Common.TelemetryParameter> list = new global::System.Collections.Generic.List<global::Kampai.Common.TelemetryParameter>();
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_NONE", string.Empty, global::Kampai.Common.ParameterName.NONE));
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_NONE", string.Empty, global::Kampai.Common.ParameterName.NONE));
			list.AddRange(GetLevelGrindPremium());
			telemetryEvent.Parameters = list;
			LogGameEvent(telemetryEvent);
		}

		public void Send_Telemetry_EVT_PINCH_PROMPT(string sourceName, global::Kampai.Game.PendingCurrencyTransaction pct, global::System.Collections.Generic.IList<global::Kampai.Util.QuantityItem> requiredItems, string action)
		{
			if (pct == null || requiredItems == null || requiredItems.Count == 0)
			{
				return;
			}
			global::Kampai.Game.TransactionArg transactionArg = pct.GetTransactionArg();
			if (transactionArg != null && transactionArg.InstanceId != 0)
			{
				global::Kampai.Game.Instance byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.Instance>(transactionArg.InstanceId);
				if (byInstanceId != null)
				{
					sourceName = byInstanceId.Definition.LocalizedKey;
				}
			}
			global::System.Collections.Generic.IList<global::Kampai.Util.QuantityItem> outputs = pct.GetPendingTransaction().Outputs;
			global::System.Collections.Generic.IList<global::Kampai.Util.QuantityItem> inputs = pct.GetPendingTransaction().Inputs;
			global::Kampai.Game.TaxonomyDefinition definition = null;
			if (outputs != null && outputs.Count > 0)
			{
				definitionService.TryGet<global::Kampai.Game.TaxonomyDefinition>(outputs[0].ID, out definition);
			}
			foreach (global::Kampai.Util.QuantityItem requiredItem in requiredItems)
			{
				global::Kampai.Game.ItemDefinition itemDefinition = definitionService.Get<global::Kampai.Game.ItemDefinition>(requiredItem.ID);
				int quantity = (int)requiredItem.Quantity;
				foreach (global::Kampai.Util.QuantityItem item in inputs)
				{
					if (item.ID == requiredItem.ID)
					{
						quantity = (int)item.Quantity;
						break;
					}
				}
				Send_Telemetry_EVT_PINCH_PROMPT(sourceName, itemDefinition.LocalizedKey, quantity, (definition == null) ? string.Empty : definition.TaxonomyHighLevel, (definition == null) ? string.Empty : definition.TaxonomySpecific, (definition == null) ? string.Empty : definition.TaxonomyType, action);
			}
		}

		public void Send_Telemetry_EVT_PINCH_PROMPT(string sourceName, string itemName, int amount, string highLevel, string specific, string type, string action)
		{
			global::Kampai.Common.TelemetryEvent telemetryEvent = new global::Kampai.Common.TelemetryEvent(SynergyTrackingEventType.EVT_PINCH_PROMPT);
			global::System.Collections.Generic.List<global::Kampai.Common.TelemetryParameter> list = new global::System.Collections.Generic.List<global::Kampai.Common.TelemetryParameter>();
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_SCORE", amount, global::Kampai.Common.ParameterName.AMOUNT));
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", itemName, global::Kampai.Common.ParameterName.ITEM_NAME));
			list.AddRange(GetLevelGrindPremium());
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", sourceName, global::Kampai.Common.ParameterName.SOURCE_NAME));
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", highLevel, global::Kampai.Common.ParameterName.TAXONOMY_HIGH_LEVEL));
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", specific, global::Kampai.Common.ParameterName.TAXONOMY_SPECIFIC));
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", type, global::Kampai.Common.ParameterName.TAXONOMY_TYPE));
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", action, global::Kampai.Common.ParameterName.PINCH_PROMPT_ACTION));
			telemetryEvent.Parameters = list;
			LogGameEvent(telemetryEvent);
		}

		public void Send_Telemetry_EVT_DCN(string buttonPressed, string url, string name)
		{
			global::Kampai.Common.TelemetryEvent telemetryEvent = new global::Kampai.Common.TelemetryEvent(SynergyTrackingEventType.EVT_DCN);
			global::System.Collections.Generic.List<global::Kampai.Common.TelemetryParameter> list = new global::System.Collections.Generic.List<global::Kampai.Common.TelemetryParameter>();
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", buttonPressed, global::Kampai.Common.ParameterName.DCN_BUTTON));
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", url, global::Kampai.Common.ParameterName.DCN_URL));
			list.AddRange(GetLevelGrindPremium());
			list.Add(IsSpender());
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", name, global::Kampai.Common.ParameterName.DCN_NAME));
			telemetryEvent.Parameters = list;
			LogGameEvent(telemetryEvent);
		}

		public void Send_Telemetry_EVT_UPSELL(string mtxSellID, global::Kampai.Common.Service.Telemetry.UpsellStatus status)
		{
			global::Kampai.Common.TelemetryEvent telemetryEvent = new global::Kampai.Common.TelemetryEvent(SynergyTrackingEventType.EVT_UPSELL_NOTIFICATION);
			global::System.Collections.Generic.List<global::Kampai.Common.TelemetryParameter> list = new global::System.Collections.Generic.List<global::Kampai.Common.TelemetryParameter>();
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", mtxSellID, global::Kampai.Common.ParameterName.MTX_SELL_ID));
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", status.ToString(), global::Kampai.Common.ParameterName.UPSELL_STATUS));
			list.AddRange(GetLevelGrindPremium());
			telemetryEvent.Parameters = list;
			LogGameEvent(telemetryEvent);
		}

		public void Send_Telemetry_EVT_MASTER_PLAN_COMPLETE(string masterPlanName, string villainName, int duration)
		{
			global::Kampai.Common.TelemetryEvent telemetryEvent = new global::Kampai.Common.TelemetryEvent(SynergyTrackingEventType.EVT_MASTER_PLAN_COMPLETE);
			global::System.Collections.Generic.List<global::Kampai.Common.TelemetryParameter> list = new global::System.Collections.Generic.List<global::Kampai.Common.TelemetryParameter>();
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", masterPlanName, global::Kampai.Common.ParameterName.MASTER_PLAN_NAME));
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_DURATION", duration, global::Kampai.Common.ParameterName.MASTER_PLAN_DURATION));
			list.AddRange(GetLevelGrindPremium());
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", villainName, global::Kampai.Common.ParameterName.MASTER_PLAN_VILLAIN_NAME));
			telemetryEvent.Parameters = list;
			LogGameEvent(telemetryEvent);
		}

		public void Send_Telemetry_EVT_MASTER_PLAN_COMPONENT_COMPLETE(string masterPlanName, string villainName, string componentName, int orderComplete)
		{
			global::Kampai.Common.TelemetryEvent telemetryEvent = new global::Kampai.Common.TelemetryEvent(SynergyTrackingEventType.EVT_MASTER_PLAN_COMPONENT_COMPLETE);
			global::System.Collections.Generic.List<global::Kampai.Common.TelemetryParameter> list = new global::System.Collections.Generic.List<global::Kampai.Common.TelemetryParameter>();
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", componentName, global::Kampai.Common.ParameterName.MASTER_PLAN_COMPONENT_NAME));
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", orderComplete, global::Kampai.Common.ParameterName.MASTER_PLAN_COMPONENT_ORDER));
			list.AddRange(GetLevelGrindPremium());
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", masterPlanName, global::Kampai.Common.ParameterName.MASTER_PLAN_NAME));
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", villainName, global::Kampai.Common.ParameterName.MASTER_PLAN_VILLAIN_NAME));
			telemetryEvent.Parameters = list;
			LogGameEvent(telemetryEvent);
		}

		public void Send_Telemetry_EVT_MASTER_PLAN_TASK_COMPLETE(string componentName, string taskType, string requiredItem, int requiredQuantity)
		{
			global::Kampai.Common.TelemetryEvent telemetryEvent = new global::Kampai.Common.TelemetryEvent(SynergyTrackingEventType.EVT_MASTER_PLAN_TASK_COMPLETE);
			global::System.Collections.Generic.List<global::Kampai.Common.TelemetryParameter> list = new global::System.Collections.Generic.List<global::Kampai.Common.TelemetryParameter>();
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", taskType, global::Kampai.Common.ParameterName.MASTER_PLAN_TASK_TYPE));
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", componentName, global::Kampai.Common.ParameterName.MASTER_PLAN_COMPONENT_NAME));
			list.AddRange(GetLevelGrindPremium());
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", requiredItem, global::Kampai.Common.ParameterName.MASTER_PLAN_TASK_REQUIRED_ITEM));
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", requiredQuantity, global::Kampai.Common.ParameterName.MASTER_PLAN_TASK_REQUIRED_QUANTITY));
			telemetryEvent.Parameters = list;
			LogGameEvent(telemetryEvent);
		}

		public void Send_Telemetry_CONTACT_US_CLICKED()
		{
			long iD = playerService.ID;
			global::Kampai.Common.TelemetryEvent telemetryEvent = new global::Kampai.Common.TelemetryEvent(SynergyTrackingEventType.EVT_CONTACT_US_CLICKED);
			global::System.Collections.Generic.List<global::Kampai.Common.TelemetryParameter> list = new global::System.Collections.Generic.List<global::Kampai.Common.TelemetryParameter>();
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", iD, global::Kampai.Common.ParameterName.PLAYER_ID));
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_NONE", string.Empty, global::Kampai.Common.ParameterName.NONE));
			list.AddRange(GetLevelGrindPremium());
			telemetryEvent.Parameters = list;
			LogGameEvent(telemetryEvent);
		}

		public void Send_Telemetry_EVT_USER_ACQUIRES_BUILDING(string source, int buildingDefId, int sourceDefId)
		{
			global::Kampai.Game.BuildingDefinition definition;
			if (definitionService.TryGet<global::Kampai.Game.BuildingDefinition>(buildingDefId, out definition))
			{
				Send_Telemetry_EVT_USER_ACQUIRES_BUILDING(source, definition, sourceDefId);
			}
		}

		public void Send_Telemetry_EVT_USER_ACQUIRES_BUILDING(string source, global::Kampai.Game.BuildingDefinition buildingDefinition, int sourceDefId)
		{
			global::Kampai.Common.TelemetryEvent telemetryEvent = new global::Kampai.Common.TelemetryEvent(SynergyTrackingEventType.EVT_USER_ACQUIRES_BUILDING);
			global::System.Collections.Generic.List<global::Kampai.Common.TelemetryParameter> list = new global::System.Collections.Generic.List<global::Kampai.Common.TelemetryParameter>();
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", GetDefinitionLocalizedKey(buildingDefinition), global::Kampai.Common.ParameterName.BUILDING));
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", source, global::Kampai.Common.ParameterName.SOURCE_NAME));
			global::System.Collections.Generic.List<global::Kampai.Common.TelemetryParameter> list2 = list;
			list2.AddRange(GetLevelGrindPremium());
			list2.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", (sourceDefId != 0) ? sourceDefId.ToString() : "NULL", global::Kampai.Common.ParameterName.SOURCE_DEF_ID));
			list2.AddRange(GetTaxonomyParameters(buildingDefinition));
			telemetryEvent.Parameters = list2;
			LogGameEvent(telemetryEvent);
		}

		public void Send_Telemetry_EVT_TSM_TRIGGER_ACTION(global::Kampai.Game.Trigger.TriggerDefinition triggerDefinition, global::Kampai.Game.Trigger.TriggerRewardDefinition reward)
		{
			if (reward == null)
			{
				return;
			}
			global::Kampai.Game.Trigger.UpsellTriggerRewardDefinition upsellTriggerRewardDefinition = reward as global::Kampai.Game.Trigger.UpsellTriggerRewardDefinition;
			global::Kampai.Common.TelemetryEvent telemetryEvent = new global::Kampai.Common.TelemetryEvent(SynergyTrackingEventType.EVT_TSM_TRIGGER_ACTION);
			string value = string.Empty;
			string value2 = TelemetryTriggerRewardCost(reward);
			if (!string.IsNullOrEmpty(value2))
			{
				value = "Transaction";
			}
			else if (upsellTriggerRewardDefinition != null)
			{
				value2 = string.Format("Upsell: {0}", upsellTriggerRewardDefinition.upsellId);
				value = "Upsell";
			}
			else if (reward.IsFree)
			{
				global::System.Text.StringBuilder stringBuilder = new global::System.Text.StringBuilder();
				for (int i = 0; i < global::Kampai.Game.Transaction.TransactionDataExtension.GetOutputCount(reward.transaction); i++)
				{
					global::Kampai.Util.QuantityItem outputItem = global::Kampai.Game.Transaction.TransactionDataExtension.GetOutputItem(reward.transaction, i);
					global::Kampai.Game.BuildingDefinition definition;
					if (definitionService.TryGet<global::Kampai.Game.BuildingDefinition>(outputItem.ID, out definition))
					{
						Send_Telemetry_EVT_USER_ACQUIRES_BUILDING(definition.TaxonomyType, definition, reward.ID);
					}
					if (i == 0)
					{
						stringBuilder.Append(outputItem.ToString(definitionService));
					}
					else
					{
						stringBuilder.AppendFormat(", {0}", outputItem.ToString(definitionService));
					}
				}
				value2 = stringBuilder.ToString();
				value = "Award";
			}
			global::System.Collections.Generic.List<global::Kampai.Common.TelemetryParameter> list = new global::System.Collections.Generic.List<global::Kampai.Common.TelemetryParameter>();
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", value2, global::Kampai.Common.ParameterName.TSM_COST_AWARD_AMOUNT));
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", value, global::Kampai.Common.ParameterName.TSM_TRIGGER_ACTION));
			global::System.Collections.Generic.List<global::Kampai.Common.TelemetryParameter> list2 = list;
			list2.AddRange(GetLevelGrindPremium());
			list2.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", (upsellTriggerRewardDefinition == null) ? string.Empty : upsellTriggerRewardDefinition.upsellId.ToString(), global::Kampai.Common.ParameterName.OFFER_ID));
			list2.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", triggerDefinition.ID, global::Kampai.Common.ParameterName.TSM_TRIGGER_TYPE));
			telemetryEvent.Parameters = list2;
			LogGameEvent(telemetryEvent);
			Send_Telemetry_EVT_TSM_TRIGGER_BUY_SELL(triggerDefinition, reward);
		}

		public void Send_Telemetry_EVT_TSM_TRIGGER_BUY_SELL(global::Kampai.Game.Trigger.TriggerDefinition triggerDefinition, global::Kampai.Game.Trigger.TriggerRewardDefinition reward)
		{
			if (reward != null && !reward.IsFree)
			{
				global::Kampai.Common.TelemetryEvent telemetryEvent = new global::Kampai.Common.TelemetryEvent(SynergyTrackingEventType.EVT_TSM_TRIGGER_BUY_SELL);
				global::System.Collections.Generic.List<global::Kampai.Common.TelemetryParameter> list = new global::System.Collections.Generic.List<global::Kampai.Common.TelemetryParameter>();
				list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", TelemetryTriggerRewardCost(reward), global::Kampai.Common.ParameterName.COST));
				list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", triggerDefinition.ID, global::Kampai.Common.ParameterName.TRIGGER_DEF_ID));
				global::System.Collections.Generic.List<global::Kampai.Common.TelemetryParameter> list2 = list;
				list2.AddRange(GetLevelGrindPremium());
				for (int i = 0; i < 5; i++)
				{
					global::Kampai.Util.QuantityItem outputItem = global::Kampai.Game.Transaction.TransactionDataExtension.GetOutputItem(reward.transaction, i);
					string value = ((outputItem != null) ? outputItem.ToString(definitionService) : string.Empty);
					list2.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", value, global::Kampai.Common.ParameterName.ITEM_QUANTITY));
				}
				telemetryEvent.Parameters = list2;
				LogGameEvent(telemetryEvent);
			}
		}

		private string TelemetryTriggerRewardCost(global::Kampai.Game.Trigger.TriggerRewardDefinition reward)
		{
			if (!reward.HasInputs)
			{
				if (!reward.IsCash)
				{
					return string.Empty;
				}
				global::Kampai.Game.PremiumCurrencyItemDefinition premiumCurrencyItemDefinition = definitionService.Get<global::Kampai.Game.PremiumCurrencyItemDefinition>(reward.SKUId);
				return string.Format("MTX: {0}", premiumCurrencyItemDefinition.SKU);
			}
			global::Kampai.Util.QuantityItem inputItem = global::Kampai.Game.Transaction.TransactionDataExtension.GetInputItem(reward.transaction, 0);
			return inputItem.ToString(definitionService);
		}

		public void Send_Telemetry_EVT_AD_INTERACTION(global::Kampai.Game.AdPlacementName placementName, global::System.Collections.Generic.IList<global::Kampai.Util.QuantityItem> rewards, int timesRedeemedInCurrentDay)
		{
			global::Kampai.Game.ItemDefinition itemDefinition;
			int rewardAmount;
			if (global::Kampai.Game.RewardedAdUtil.GetFirstItemDefintion(rewards, out itemDefinition, out rewardAmount, definitionService))
			{
				string surfaceLocation = global::Kampai.Common.RewardedAdTelemetryUtil.GetSurfaceLocation(placementName);
				string rewardType = global::Kampai.Common.RewardedAdTelemetryUtil.GetRewardType(placementName, itemDefinition);
				Send_Telemetry_EVT_AD_INTERACTION(surfaceLocation, rewardType, timesRedeemedInCurrentDay);
			}
		}

		public void Send_Telemetry_EVT_AD_INTERACTION(global::Kampai.Game.AdPlacementName placementName, global::Kampai.Game.ItemDefinition reward, int timesRedeemedInCurrentDay)
		{
			string surfaceLocation = global::Kampai.Common.RewardedAdTelemetryUtil.GetSurfaceLocation(placementName);
			string rewardType = global::Kampai.Common.RewardedAdTelemetryUtil.GetRewardType(placementName, reward);
			Send_Telemetry_EVT_AD_INTERACTION(surfaceLocation, rewardType, timesRedeemedInCurrentDay);
		}

		public void Send_Telemetry_EVT_AD_INTERACTION(string surfaceLocation, string rewardType, int timesRedeemedInCurrentDay)
		{
			global::Kampai.Common.TelemetryEvent telemetryEvent = new global::Kampai.Common.TelemetryEvent(SynergyTrackingEventType.EVT_AD_INTERACTION);
			global::System.Collections.Generic.List<global::Kampai.Common.TelemetryParameter> list = new global::System.Collections.Generic.List<global::Kampai.Common.TelemetryParameter>();
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", surfaceLocation, global::Kampai.Common.ParameterName.AD_SURFACE_LOCATION));
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", rewardType, global::Kampai.Common.ParameterName.AD_REWARD_TYPE));
			global::System.Collections.Generic.List<global::Kampai.Common.TelemetryParameter> list2 = list;
			list2.AddRange(GetLevelGrindPremium());
			list2.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", timesRedeemedInCurrentDay.ToString(), global::Kampai.Common.ParameterName.AD_TIMES_REDEEMED_IN_CURRENT_DAY));
			list2.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", string.Empty, global::Kampai.Common.ParameterName.TAXONOMY_HIGH_LEVEL));
			list2.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", string.Empty, global::Kampai.Common.ParameterName.TAXONOMY_SPECIFIC));
			list2.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", string.Empty, global::Kampai.Common.ParameterName.TAXONOMY_TYPE));
			list2.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", playerService.ID, global::Kampai.Common.ParameterName.PLAYER_ID));
			telemetryEvent.Parameters = list2;
			LogGameEvent(telemetryEvent);
		}

		public void Send_Telemetry_EVT_GAME_BUTTON_PRESSED_GENERIC(global::Kampai.Util.GameConstants.TrackedGameButton buttonName, string optionalParam2 = "", global::Kampai.Common.ParameterName param2Name = global::Kampai.Common.ParameterName.NONE)
		{
			global::Kampai.Common.TelemetryEvent telemetryEvent = new global::Kampai.Common.TelemetryEvent(SynergyTrackingEventType.EVT_GAME_BUTTON_PRESSED);
			global::System.Collections.Generic.List<global::Kampai.Common.TelemetryParameter> list = new global::System.Collections.Generic.List<global::Kampai.Common.TelemetryParameter>();
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", buttonName.ToString(), global::Kampai.Common.ParameterName.BUTTON_NAME));
			global::System.Collections.Generic.List<global::Kampai.Common.TelemetryParameter> list2 = list;
			list2.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", optionalParam2, param2Name));
			list2.AddRange(GetLevelGrindPremium());
			for (int i = 0; i < 4; i++)
			{
				list2.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", string.Empty, global::Kampai.Common.ParameterName.NONE));
			}
			list2.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", playerService.ID, global::Kampai.Common.ParameterName.PLAYER_ID));
			telemetryEvent.Parameters = list2;
			LogGameEvent(telemetryEvent);
		}

		public void Send_Telemetry_EVT_GAME_XPROMO_BUTTON_PRESSED(global::Kampai.Util.GameConstants.TrackedGameButton buttonName, bool petsInstalled)
		{
			Send_Telemetry_EVT_GAME_BUTTON_PRESSED_GENERIC(buttonName, (!petsInstalled) ? "Pets Not Installed" : "Pets Installed", global::Kampai.Common.ParameterName.XPROMO_PETS_INSTALLED);
		}

		public void Send_Telemetry_EVT_MTX_BOOKEND_EVENT(global::Kampai.Common.MtxBookendTelemetryInfo mtxInfo)
		{
			global::Kampai.Common.TelemetryEvent telemetryEvent = new global::Kampai.Common.TelemetryEvent(SynergyTrackingEventType.EVT_MTX_BOOKEND_INFO);
			global::System.Collections.Generic.List<global::Kampai.Common.TelemetryParameter> list = new global::System.Collections.Generic.List<global::Kampai.Common.TelemetryParameter>();
			list.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", mtxInfo.productId, global::Kampai.Common.ParameterName.MTX_SELL_ID));
			global::System.Collections.Generic.List<global::Kampai.Common.TelemetryParameter> list2 = list;
			if (mtxInfo.purchaseStage != global::Kampai.Common.MTXPurchaseStage.Complete_Fail)
			{
				list2.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", mtxInfo.purchaseStage.ToString(), global::Kampai.Common.ParameterName.STAGE_OF_PURCHASE));
			}
			else
			{
				list2.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", mtxInfo.purchaseStage.ToString() + " " + mtxInfo.failedReason, global::Kampai.Common.ParameterName.STAGE_OF_PURCHASE));
			}
			list2.AddRange(GetLevelGrindPremium());
			if (mtxInfo.purchaseStage == global::Kampai.Common.MTXPurchaseStage.Initiate)
			{
				list2.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", string.Empty, global::Kampai.Common.ParameterName.PURCHASE_COMPLETION_TIME));
			}
			else
			{
				list2.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", mtxInfo.timeToComplete.ToString(), global::Kampai.Common.ParameterName.PURCHASE_COMPLETION_TIME));
			}
			for (int i = 0; i < 3; i++)
			{
				list2.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", string.Empty, global::Kampai.Common.ParameterName.NONE));
			}
			list2.Add(new global::Kampai.Common.TelemetryParameter("EVT_KEYTYPE_ENUMERATION", playerService.ID, global::Kampai.Common.ParameterName.PLAYER_ID));
			telemetryEvent.Parameters = list2;
			LogGameEvent(telemetryEvent);
		}
	}
}
