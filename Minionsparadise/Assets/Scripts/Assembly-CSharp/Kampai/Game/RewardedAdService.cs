namespace Kampai.Game
{
	public class RewardedAdService : global::Kampai.Game.IRewardedAdService
	{
		private sealed class AdPlacementKey : global::System.IEquatable<global::Kampai.Game.RewardedAdService.AdPlacementKey>
		{
			public global::Kampai.Game.AdPlacementName PlacementName;

			public int PlacementInstanceId;

			public override bool Equals(object obj)
			{
				return Equals(obj as global::Kampai.Game.RewardedAdService.AdPlacementKey);
			}

			public bool Equals(global::Kampai.Game.RewardedAdService.AdPlacementKey p)
			{
				if (p == null)
				{
					return false;
				}
				return PlacementName == p.PlacementName && PlacementInstanceId == p.PlacementInstanceId;
			}

			public override int GetHashCode()
			{
				return (int)PlacementName ^ PlacementInstanceId;
			}
		}

		private sealed class PendingAdImpressionData
		{
			public global::Kampai.Game.AdPlacementInstance PlacementInstance { get; private set; }

			public global::Kampai.Game.Transaction.TransactionDefinition Reward { get; private set; }

			public PendingAdImpressionData(global::Kampai.Game.AdPlacementInstance placementInstance, global::Kampai.Game.Transaction.TransactionDefinition reward)
			{
				PlacementInstance = placementInstance;
				Reward = reward;
			}
		}

		private const string TAG = "Ads: ";

		private global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("RewardedAdService") as global::Kampai.Util.IKampaiLogger;

		private int rewardPerDayCountTotal;

		private global::System.Collections.Generic.Dictionary<global::Kampai.Game.RewardedAdService.AdPlacementKey, global::Kampai.Game.AdPlacementInstance> adPlacements = new global::System.Collections.Generic.Dictionary<global::Kampai.Game.RewardedAdService.AdPlacementKey, global::Kampai.Game.AdPlacementInstance>();

		private global::strange.extensions.signal.impl.Signal<int> updatePlacementConditionsByTimerSignal = new global::strange.extensions.signal.impl.Signal<int>();

		private global::Kampai.Game.RewardedAdService.PendingAdImpressionData pendingAdImpressionData;

		[Inject]
		public global::Kampai.Game.AwardLevelSignal awardLevelSignal { get; set; }

		[Inject]
		public global::Kampai.Common.AppResumeCompletedSignal appResumeCompletedSignal { get; set; }

		[Inject]
		public global::Kampai.Game.PlayerSessionCountUpdatedSignal playerSessionCountUpdatedSignal { get; set; }



		[Inject]
		public global::Kampai.Game.ITimeService timeService { get; set; }

		[Inject]
		public global::Kampai.Game.ITimeEventService timeEventService { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject(global::Kampai.Game.GameElement.CONTEXT)]
		public global::strange.extensions.context.api.ICrossContextCapable gameContext { get; set; }

		[Inject]
		public global::Kampai.Game.ResetRewardedAdLimitSignal resetRewardedAdLimitSignal { get; set; }

		[Inject]
		public global::Kampai.Game.AdPlacementActivityStateChangedSignal adPlacementActivityStateChangedSignal { get; set; }



		[Inject]
		public global::Kampai.UI.View.DeclineRewardedAdShowSignal declineRewadedAdShowSignal { get; set; }

		[Inject]
		public global::Kampai.Game.RewardedAdRewardSignal rewardedAdRewardSignal { get; set; }

		[Inject]
		public global::Kampai.Common.ReInitializeGameSignal reInitializeGameSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IConfigurationsService configurationsService { get; set; }

		[Inject]
		public global::Kampai.Main.ILocalizationService localizationService { get; set; }

		private global::Kampai.Game.RewardedAdService.AdPlacementKey GetKey(global::Kampai.Game.AdPlacementName placementName, int instanceId)
		{
			global::Kampai.Game.RewardedAdService.AdPlacementKey adPlacementKey = new global::Kampai.Game.RewardedAdService.AdPlacementKey();
			adPlacementKey.PlacementName = placementName;
			adPlacementKey.PlacementInstanceId = instanceId;
			return adPlacementKey;
		}

		public void Initialize()
		{
			reInitializeGameSignal.AddListener(OnReinitializeGame);
			SubscribeOnTriggerConditionsSignals(true);
			SubscribeOnCooldownConditionSignals(true);
			SubscribeOnRewardedVideoSignals(true);
			LoadPlacementsFromPlayer();
			CleanupInvalidPlacements();
			CheckPlacementConditions();
		}

		public void Destroy()
		{
			reInitializeGameSignal.RemoveListener(OnReinitializeGame);
			SubscribeOnRewardedVideoSignals(false);
			SubscribeForCooldownUpdate(false);
			SubscribeOnCooldownConditionSignals(false);
			SubscribeOnTriggerConditionsSignals(false);
		}

		private void OnReinitializeGame(string notused)
		{
			Destroy();
		}

		private void LoadPlacementsFromPlayer()
		{
			foreach (global::Kampai.Game.AdPlacementInstance item in playerService.GetInstancesByType<global::Kampai.Game.AdPlacementInstance>())
			{
				global::Kampai.Game.RewardedAdService.AdPlacementKey key = GetKey(item.Definition.Name, item.PlacementInstanceId);
				adPlacements.Add(key, item);
			}
			foreach (global::System.Collections.Generic.KeyValuePair<global::Kampai.Game.RewardedAdService.AdPlacementKey, global::Kampai.Game.AdPlacementInstance> adPlacement in adPlacements)
			{
				global::Kampai.Game.AdPlacementInstance value = adPlacement.Value;
				value.ActivityStateOnLastCheck = IsPlacementActive(value);
			}
		}

		public bool IsRewardedVideoAvailable()
		{
			return false;
		}

		public bool IsPlacementAvailable(global::Kampai.Game.AdPlacementName placementName)
		{
			global::Kampai.Game.AdPlacementDefinition placement = GetPlacement(placementName);
			return placement != null;
		}

		public global::Kampai.Game.AdPlacementInstance GetPlacementInstance(global::Kampai.Game.AdPlacementName placementName, int instanceId = 0)
		{
			global::Kampai.Game.AdPlacementDefinition placement = GetPlacement(placementName);
			if (placement == null)
			{
				logger.Debug("{0}IsPlacementActive(): placement is not available", "Ads: ");
				return null;
			}
			return GetOrCreatePlacementInstance(placement, instanceId);
		}

		public bool IsPlacementActive(global::Kampai.Game.AdPlacementName placementName, int instanceId = 0)
		{
			global::Kampai.Game.AdPlacementInstance placementInstance = GetPlacementInstance(placementName, instanceId);
			return IsPlacementActive(placementInstance);
		}

		private bool IsPlacementActive(global::Kampai.Game.AdPlacementInstance instance)
		{
			if (instance == null)
			{
				return false;
			}
			if (!IsRewardedVideoAvailable())
			{
				return false;
			}
			if (IsTotalRewardLimitPerPeriodExceeded())
			{
				return false;
			}
			return instance.IsActive(timeService.CurrentTime());
		}

		private global::Kampai.Game.AdPlacementDefinition GetPlacement(global::Kampai.Game.AdPlacementName placementName)
		{
			global::Kampai.Game.RewardedAdvertisementDefinition rewardedAdvertisementDefinition = definitionService.Get<global::Kampai.Game.RewardedAdvertisementDefinition>();
			if (rewardedAdvertisementDefinition != null)
			{
				global::System.Collections.Generic.List<global::Kampai.Game.AdPlacementDefinition> placementDefinitions = rewardedAdvertisementDefinition.PlacementDefinitions;
				for (int i = 0; i < placementDefinitions.Count; i++)
				{
					global::Kampai.Game.AdPlacementDefinition adPlacementDefinition = placementDefinitions[i];
					if (adPlacementDefinition != null && adPlacementDefinition.Name == placementName && adPlacementDefinition.IsAvailable(gameContext))
					{
						return adPlacementDefinition;
					}
				}
			}
			return null;
		}

		private global::Kampai.Game.AdPlacementInstance GetOrCreatePlacementInstance(global::Kampai.Game.AdPlacementDefinition placementDefinition, int instanceId = 0)
		{
			global::Kampai.Game.RewardedAdService.AdPlacementKey key = GetKey(placementDefinition.Name, instanceId);
			global::Kampai.Game.AdPlacementInstance value;
			if (!adPlacements.TryGetValue(key, out value))
			{
				value = (global::Kampai.Game.AdPlacementInstance)placementDefinition.Build();
				value.PlacementInstanceId = key.PlacementInstanceId;
				playerService.AssignNextInstanceId(value);
				adPlacements.Add(key, value);
				playerService.Add(value);
			}
			return value;
		}

		private void CheckPlacementConditions()
		{
			global::Kampai.Game.RewardedAdvertisementDefinition rewardedAdvertisementDefinition = definitionService.Get<global::Kampai.Game.RewardedAdvertisementDefinition>();
			if (rewardedAdvertisementDefinition == null)
			{
				logger.Error("{0}RewardedAdvertisementDefinition does not exist.", "Ads: ");
				return;
			}
			global::System.Collections.Generic.List<global::Kampai.Game.AdPlacementDefinition> placementDefinitions = rewardedAdvertisementDefinition.PlacementDefinitions;
			for (int i = 0; i < placementDefinitions.Count; i++)
			{
				HandlePlacementDefinitionChange(placementDefinitions[i]);
			}
			bool flag = !IsTotalRewardLimitPerPeriodExceeded(rewardedAdvertisementDefinition);
			for (int j = 0; j < placementDefinitions.Count; j++)
			{
				global::Kampai.Game.AdPlacementDefinition adPlacementDefinition = placementDefinitions[j];
				bool isPlacementActive = flag && adPlacementDefinition.IsAvailable(gameContext) && IsRewardedVideoAvailable();
				NotifyPlacementActivityStateChange(adPlacementDefinition, isPlacementActive);
			}
			RemovePlacements((global::Kampai.Game.AdPlacementInstance adPlacementInstance) => !adPlacementInstance.Definition.IsAvailable(gameContext));
			SubscribeForCooldownUpdate(true);
		}

		private bool IsTotalRewardLimitPerPeriodExceeded(global::Kampai.Game.RewardedAdvertisementDefinition adDefinition = null)
		{
			if (adDefinition == null)
			{
				adDefinition = definitionService.Get<global::Kampai.Game.RewardedAdvertisementDefinition>();
				if (adDefinition == null)
				{
					return true;
				}
			}
			if (adDefinition.MaxRewardsPerDayGlobal < 0)
			{
				return false;
			}
			UpdateTotalRewardPerDayCount();
			return rewardPerDayCountTotal >= adDefinition.MaxRewardsPerDayGlobal;
		}

		private void SubscribeForCooldownUpdate(bool subscribe)
		{
			int num = timeService.CurrentTime();
			global::Kampai.Game.AdPlacementInstance adPlacementInstance = null;
			int num2 = int.MaxValue;
			foreach (global::System.Collections.Generic.KeyValuePair<global::Kampai.Game.RewardedAdService.AdPlacementKey, global::Kampai.Game.AdPlacementInstance> adPlacement in adPlacements)
			{
				global::Kampai.Game.AdPlacementInstance value = adPlacement.Value;
				int num3 = value.LastRewardPeriodStartTimestamp + 86400;
				if (num3 < num2 && num3 > num)
				{
					num2 = num3;
					adPlacementInstance = value;
				}
				if (!value.IsActive(num))
				{
					int iD = value.ID;
					if (timeEventService != null)
					{
						timeEventService.RemoveEvent(iD);
					}
					if (subscribe)
					{
						int cooldownEndTimestamp = value.GetCooldownEndTimestamp(num);
						SchedulePlacementConditionsUpdate(iD, num, cooldownEndTimestamp);
					}
				}
			}
			if (adPlacementInstance != null)
			{
				int cooldownEndTimestamp2 = adPlacementInstance.GetCooldownEndTimestamp(num);
				int num4 = num2;
				if (cooldownEndTimestamp2 < num4 && cooldownEndTimestamp2 > num)
				{
					num4 = cooldownEndTimestamp2;
				}
				int iD2 = adPlacementInstance.ID;
				if (timeEventService != null)
				{
					timeEventService.RemoveEvent(iD2);
				}
				if (subscribe)
				{
					SchedulePlacementConditionsUpdate(iD2, num, num4);
				}
			}
		}

		private void SchedulePlacementConditionsUpdate(int eventID, int currentTimeUTC, int nextUpdateTimestamp)
		{
			int num = nextUpdateTimestamp - currentTimeUTC;
			if (num > 0)
			{
				num++;
				if (timeEventService != null)
				{
					timeEventService.AddEvent(eventID, currentTimeUTC, num, updatePlacementConditionsByTimerSignal);
				}
			}
		}

		private void UpdateTotalRewardPerDayCount()
		{
			rewardPerDayCountTotal = CalcTotalRewardPerDayCount();
		}

		private int CalcTotalRewardPerDayCount()
		{
			int num = 0;
			int currentTimeUTC = timeService.CurrentTime();
			foreach (global::System.Collections.Generic.KeyValuePair<global::Kampai.Game.RewardedAdService.AdPlacementKey, global::Kampai.Game.AdPlacementInstance> adPlacement in adPlacements)
			{
				global::Kampai.Game.AdPlacementInstance value = adPlacement.Value;
				value.UpdateRewardLimitCounter(currentTimeUTC);
				num += value.RewardPerPeriodCount;
			}
			return num;
		}

		private void HandlePlacementDefinitionChange(global::Kampai.Game.AdPlacementDefinition placementDefinition)
		{
			global::Kampai.Game.AdPlacementName name = placementDefinition.Name;
			int currentTimeUTC = timeService.CurrentTime();
			global::System.Collections.Generic.HashSet<global::Kampai.Game.RewardedAdService.AdPlacementKey> hashSet = null;
			bool flag = IsPlacementAvailable(name);
			bool flag2 = true;
			foreach (global::System.Collections.Generic.KeyValuePair<global::Kampai.Game.RewardedAdService.AdPlacementKey, global::Kampai.Game.AdPlacementInstance> adPlacement in adPlacements)
			{
				global::Kampai.Game.AdPlacementInstance value = adPlacement.Value;
				if (adPlacement.Key.PlacementName != name)
				{
					continue;
				}
				flag2 = false;
				bool flag3 = flag && value.IsActive(currentTimeUTC);
				if (value.ActivityStateOnLastCheck && (!flag3 || placementDefinition.ID != value.Definition.ID))
				{
					flag3 = (value.ActivityStateOnLastCheck = false);
					adPlacementActivityStateChangedSignal.Dispatch(value, flag3);
				}
				if (placementDefinition.ID != value.Definition.ID)
				{
					if (hashSet == null)
					{
						hashSet = new global::System.Collections.Generic.HashSet<global::Kampai.Game.RewardedAdService.AdPlacementKey>();
					}
					hashSet.Add(adPlacement.Key);
				}
			}
			if (flag2)
			{
				GetOrCreatePlacementInstance(placementDefinition);
			}
			if (hashSet == null)
			{
				return;
			}
			foreach (global::Kampai.Game.RewardedAdService.AdPlacementKey item in hashSet)
			{
				global::Kampai.Game.AdPlacementInstance i = adPlacements[item];
				adPlacements.Remove(item);
				playerService.Remove(i);
				GetOrCreatePlacementInstance(placementDefinition, item.PlacementInstanceId);
			}
		}

		private void NotifyPlacementActivityStateChange(global::Kampai.Game.AdPlacementDefinition placementDefinition, bool isPlacementActive)
		{
			int currentTimeUTC = timeService.CurrentTime();
			foreach (global::System.Collections.Generic.KeyValuePair<global::Kampai.Game.RewardedAdService.AdPlacementKey, global::Kampai.Game.AdPlacementInstance> adPlacement in adPlacements)
			{
				global::Kampai.Game.AdPlacementInstance value = adPlacement.Value;
				if (adPlacement.Key.PlacementName != placementDefinition.Name)
				{
					continue;
				}
				bool flag = isPlacementActive && value.IsActive(currentTimeUTC);
				if (flag)
				{
					if (!value.ActivityStateOnLastCheck && flag)
					{
						value.ActivityStateOnLastCheck = true;
						adPlacementActivityStateChangedSignal.Dispatch(value, flag);
					}
				}
				else if (value.ActivityStateOnLastCheck && !flag)
				{
					value.ActivityStateOnLastCheck = false;
					adPlacementActivityStateChangedSignal.Dispatch(value, flag);
				}
			}
		}

		private void SubscribeOnTriggerConditionsSignals(bool subscribe)
		{
			if (subscribe)
			{
				awardLevelSignal.AddListener(OnAwardLevel);
				appResumeCompletedSignal.AddListener(CheckPlacementConditions);
				playerSessionCountUpdatedSignal.AddListener(CheckPlacementConditions);
			}
			else
			{
				awardLevelSignal.RemoveListener(OnAwardLevel);
				appResumeCompletedSignal.RemoveListener(CheckPlacementConditions);
				playerSessionCountUpdatedSignal.RemoveListener(CheckPlacementConditions);
			}
		}

		private void SubscribeOnCooldownConditionSignals(bool subscribe)
		{
			if (subscribe)
			{
				resetRewardedAdLimitSignal.AddListener(OnResetRewardedAdLimits);
				updatePlacementConditionsByTimerSignal.AddListener(CheckPlacementConditionsByTimer);
			}
			else
			{
				resetRewardedAdLimitSignal.RemoveListener(OnResetRewardedAdLimits);
				updatePlacementConditionsByTimerSignal.RemoveListener(CheckPlacementConditionsByTimer);
			}
		}

		private void SubscribeOnRewardedVideoSignals(bool subscribe)
		{
			if (subscribe)
			{
/*
				supersonicVideoAdAvailabilityChangedSignal.AddListener(OnVideoAdAvailabilityChanged);
				supersonicVideoAdShowSignal.AddListener(OnVideoAdShow);
				supersonicVideoAdRewardedSignal.AddListener(OnVideoAdRewarded);
*/
				declineRewadedAdShowSignal.AddListener(OnAdWatchDecline);
			}
			else
			{
/*
				supersonicVideoAdAvailabilityChangedSignal.RemoveListener(OnVideoAdAvailabilityChanged);
				supersonicVideoAdShowSignal.RemoveListener(OnVideoAdShow);
				supersonicVideoAdRewardedSignal.RemoveListener(OnVideoAdRewarded);
*/
				declineRewadedAdShowSignal.RemoveListener(OnAdWatchDecline);
			}
		}

		private void CleanupInvalidPlacements()
		{
			RemovePlacements((global::Kampai.Game.AdPlacementInstance i) => i.Definition.Name == global::Kampai.Game.AdPlacementName.INVALID);
		}

		private void RemovePlacements(global::System.Func<global::Kampai.Game.AdPlacementInstance, bool> predicate)
		{
			global::System.Collections.Generic.List<global::Kampai.Game.RewardedAdService.AdPlacementKey> list = new global::System.Collections.Generic.List<global::Kampai.Game.RewardedAdService.AdPlacementKey>();
			foreach (global::System.Collections.Generic.KeyValuePair<global::Kampai.Game.RewardedAdService.AdPlacementKey, global::Kampai.Game.AdPlacementInstance> adPlacement in adPlacements)
			{
				if (predicate(adPlacement.Value))
				{
					list.Add(adPlacement.Key);
				}
			}
			foreach (global::Kampai.Game.RewardedAdService.AdPlacementKey item in list)
			{
				global::Kampai.Game.AdPlacementInstance i = adPlacements[item];
				adPlacements.Remove(item);
				playerService.Remove(i);
			}
		}

		private void OnAwardLevel(global::Kampai.Game.Transaction.TransactionDefinition td)
		{
			CheckPlacementConditions();
		}

		private void CheckPlacementConditionsByTimer(int id)
		{
			CheckPlacementConditions();
		}

		private void OnResetRewardedAdLimits()
		{
			foreach (global::System.Collections.Generic.KeyValuePair<global::Kampai.Game.RewardedAdService.AdPlacementKey, global::Kampai.Game.AdPlacementInstance> adPlacement in adPlacements)
			{
				adPlacement.Value.ResetCooldown();
			}
			UpdateTotalRewardPerDayCount();
			CheckPlacementConditions();
		}

		private void OnVideoAdAvailabilityChanged(bool videoAvailable)
		{
			CheckPlacementConditions();
		}

		public void ShowRewardedVideo(global::Kampai.Game.AdPlacementInstance instance, global::Kampai.Game.Transaction.TransactionDefinition reward)
		{
			if (pendingAdImpressionData != null)
			{
				logger.Error("ShowRewardedVideo(): Unexpected invokation: previous watch-reward exists", "Ads: ");
			}
			pendingAdImpressionData = new global::Kampai.Game.RewardedAdService.PendingAdImpressionData(instance, reward);
			logger.Warning("ShowRewardedVideo(): Supersonic service removed. Cannot show video ad.");
			// Simulate failure or just clear pending data
			pendingAdImpressionData = null;
		}

		private void OnVideoAdShow()
		{
			if (pendingAdImpressionData != null)
			{
				pendingAdImpressionData.PlacementInstance.ActivateWatchAdAcceptCooldown(timeService.CurrentTime());
			}
			CheckPlacementConditions();
		}

		private void OnAdWatchDecline(global::Kampai.Game.AdPlacementInstance instance)
		{
			instance.ActivateWatchAdDeclineCooldown(timeService.CurrentTime());
			CheckPlacementConditions();
		}

		private void OnVideoAdRewarded()
		{
			if (pendingAdImpressionData == null)
			{
				logger.Error("{0}OnVideoAdRewarded(): Unexpected null pending ad impression data", "Ads: ");
				return;
			}
			global::Kampai.Game.AdPlacementInstance placementInstance = pendingAdImpressionData.PlacementInstance;
			global::Kampai.Game.Transaction.TransactionDefinition reward = pendingAdImpressionData.Reward;
			if (reward != null)
			{
				RewardPlayer(reward, placementInstance);
			}
			rewardedAdRewardSignal.Dispatch(placementInstance);
			pendingAdImpressionData = null;
			CheckPlacementConditions();
		}

		public void RewardPlayer(global::Kampai.Game.Transaction.TransactionDefinition reward, global::Kampai.Game.AdPlacementInstance placementInstance)
		{
			if (reward != null)
			{
				global::Kampai.Game.TransactionArg transactionArg = new global::Kampai.Game.TransactionArg();
				transactionArg.Source = "RewardedVideo";
				global::Kampai.Game.TransactionArg arg = transactionArg;
				playerService.RunEntireTransaction(reward, global::Kampai.Game.TransactionTarget.NO_VISUAL, null, arg);
			}
			placementInstance.RegisterReward(timeService.CurrentTime());
			CheckPlacementConditions();
		}

		public string GetPlacementsReport()
		{
			global::System.Text.StringBuilder stringBuilder = new global::System.Text.StringBuilder();
			global::Kampai.Game.RewardedAdvertisementDefinition rewardedAdvertisementDefinition = definitionService.Get<global::Kampai.Game.RewardedAdvertisementDefinition>();
			if (rewardedAdvertisementDefinition == null)
			{
				stringBuilder.AppendFormat("RewardedAdvertisementDefinition is missing in definitions, ad is disabled");
				return stringBuilder.ToString();
			}
			stringBuilder.AppendFormat("Supersonic rewarded video available: {0}\n", IsRewardedVideoAvailable());
			global::Kampai.Game.KillSwitch killSwitch = global::Kampai.Game.KillSwitch.SUPERSONIC;
			stringBuilder.AppendFormat("{0} killswitch status: {1}\n", killSwitch, (!configurationsService.isKillSwitchOn(killSwitch)) ? "off" : "on");
			stringBuilder.AppendFormat("Country: {0}\n", localizationService.GetCountry() ?? "null");
			stringBuilder.AppendFormat("Player level: {0}\n", playerService.GetQuantity(global::Kampai.Game.StaticItem.LEVEL_ID));
			stringBuilder.AppendFormat("Player session count: {0}\n", playerService.GetQuantity(global::Kampai.Game.StaticItem.PLAYER_SESSION_COUNT));
			bool flag = IsTotalRewardLimitPerPeriodExceeded(rewardedAdvertisementDefinition);
			stringBuilder.AppendFormat("Global reward limit per day exceeded: {0}, global reward count: {1}, limit: {2}\n", flag, rewardPerDayCountTotal, rewardedAdvertisementDefinition.MaxRewardsPerDayGlobal);
			stringBuilder.AppendLine("Placements availablitity status:");
			global::System.Collections.Generic.List<global::Kampai.Game.AdPlacementDefinition> placementDefinitions = rewardedAdvertisementDefinition.PlacementDefinitions;
			string empty = string.Empty;
			for (int i = 0; i < placementDefinitions.Count; i++)
			{
				global::Kampai.Game.AdPlacementDefinition adPlacementDefinition = placementDefinitions[i];
				stringBuilder.AppendFormat("{0}placement {1}(ID:{2}) available: {3}\n", empty + "\t", adPlacementDefinition.Name, adPlacementDefinition.ID, adPlacementDefinition.IsAvailable(gameContext));
			}
			stringBuilder.AppendLine("Placement instances activity status:");
			int num = timeService.CurrentTime();
			foreach (global::System.Collections.Generic.KeyValuePair<global::Kampai.Game.RewardedAdService.AdPlacementKey, global::Kampai.Game.AdPlacementInstance> adPlacement in adPlacements)
			{
				global::Kampai.Game.AdPlacementInstance value = adPlacement.Value;
				global::Kampai.Game.AdPlacementDefinition definition = value.Definition;
				bool flag2 = value.IsActive(num);
				stringBuilder.AppendFormat("{0}placement instance {1}(ID:{2}) available: {3}\n", empty + "\t", definition.Name, value.ID, flag2);
				if (!flag2)
				{
					stringBuilder.AppendFormat("{0}reward limit per day exceeded: {1}, reward per period count: {2}, limit: {3}\n", empty + "\t\t", value.IsRewardLimitPerPeriodExceeded(num), value.RewardPerPeriodCount, definition.MaxRewardsPerDay);
					bool flag3 = num - value.LastWatchAdAcceptTimestamp < definition.CooldownSeconds;
					stringBuilder.AppendFormat("{0}cooldown after watch accept: {1}. Cooldown duration sec: {2}\n", empty + "\t\t", flag3, definition.CooldownSeconds);
					bool flag4 = num - value.LastWatchAdDeclineTimestamp < definition.CooldownWatchDeclineSeconds;
					stringBuilder.AppendFormat("{0}cooldown after watch decline: {1}. Cooldown duration sec: {2}\n", empty + "\t\t", flag4, definition.CooldownWatchDeclineSeconds);
					global::System.DateTime dateTime = new global::System.DateTime(1970, 1, 1, 0, 0, 0, 0, global::System.DateTimeKind.Utc).AddSeconds(value.GetCooldownEndTimestamp(num)).ToLocalTime();
					stringBuilder.AppendFormat("{0}cooldown ends at: {1}(local device time)\n", empty + "\t\t", dateTime);
				}
			}
			string text = stringBuilder.ToString();
			logger.Error("{0}GetPlacementsReport(): {1}", "Ads: ", text);
			return text;
		}
	}
}
