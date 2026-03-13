namespace Kampai.Game
{
	public class MIBBuildingObjectMediator : global::strange.extensions.mediation.impl.Mediator
	{
		private global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("MIBBuildingObjectMediator") as global::Kampai.Util.IKampaiLogger;

		private global::Kampai.Game.MIBBuilding building;

		private global::strange.extensions.signal.impl.Signal<int> expiredSignal = new global::strange.extensions.signal.impl.Signal<int>();

		private global::strange.extensions.signal.impl.Signal<int> preloadSignal = new global::strange.extensions.signal.impl.Signal<int>();

		private int timeEventId;

		[Inject]
		public global::Kampai.Game.MIBBuildingObjectView view { get; set; }

		[Inject]
		public global::Kampai.Game.DisplayMIBBuildingSignal displayMIBBuildingSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Main.IHindsightService hindsightService { get; set; }

		[Inject]
		public global::Kampai.Game.ITimeEventService timeEventService { get; set; }

		[Inject]
		public global::Kampai.Game.ITimeService timeService { get; set; }

		[Inject]
		public global::Kampai.Main.HindsightContentPreloadSignal hindsightContentPreloadSignal { get; set; }

		[Inject]
		public global::Kampai.Main.HindsightContentDismissSignal hindsightContentDismissSignal { get; set; }

		[Inject]
		public global::Kampai.Game.GrantMIBRewardsSignal grantMIBRewardsSignal { get; set; }

		[Inject]
		public global::Kampai.Common.ITelemetryService telemetryService { get; set; }

		[Inject]
		public global::Kampai.Common.AppResumeCompletedSignal appResumeCompletedSignal { get; set; }

		[Inject]
		public global::Kampai.Common.AppFocusGainedCompletedSignal appFocusGainedCompletedSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.SpawnDooberSignal spawnDooberSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.CreateResourceIconSignal createResourceIconSignal { get; set; }

		[Inject(global::Kampai.UI.View.UIElement.CAMERA)]
		public global::UnityEngine.Camera uiCamera { get; set; }

		[Inject]
		public global::Kampai.Game.IMIBService mibService { get; set; }

		public override void OnRegister()
		{
			DisplayBuilding(false);
			if (playerService.GetHighestFtueCompleted() >= 999999)
			{
				displayMIBBuildingSignal.AddListener(DisplayBuilding);
				hindsightContentPreloadSignal.AddListener(SetReadyState);
				hindsightContentDismissSignal.AddListener(OnContentDismissed);
				grantMIBRewardsSignal.AddListener(OnGrantRewards);
				expiredSignal.AddListener(OnExpiredSignalFired);
				preloadSignal.AddListener(OnPreloadSignalFired);
				appResumeCompletedSignal.AddListener(OnAppResume);
				appFocusGainedCompletedSignal.AddListener(OnAppResume);
				building = playerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.MIBBuilding>(3129);
				timeEventId = building.ID;
				UpdateState(building.MIBState);
				CheckForRewards();
			}
		}

		public override void OnRemove()
		{
			displayMIBBuildingSignal.RemoveListener(DisplayBuilding);
			hindsightContentPreloadSignal.RemoveListener(SetReadyState);
			hindsightContentDismissSignal.RemoveListener(OnContentDismissed);
			grantMIBRewardsSignal.RemoveListener(OnGrantRewards);
			expiredSignal.RemoveListener(OnExpiredSignalFired);
			preloadSignal.RemoveListener(OnPreloadSignalFired);
			appResumeCompletedSignal.RemoveListener(OnAppResume);
			appFocusGainedCompletedSignal.RemoveListener(OnAppResume);
			if (timeEventId != 0)
			{
				timeEventService.RemoveEvent(timeEventId);
			}
		}

		private void UpdateState(global::Kampai.Game.MIBBuildingState state, bool registerTimeEvents = true)
		{
			switch (state)
			{
			case global::Kampai.Game.MIBBuildingState.READY:
				PreloadThenSetReadyState();
				break;
			case global::Kampai.Game.MIBBuildingState.PRELOADING:
				SetPreloadingState();
				break;
			case global::Kampai.Game.MIBBuildingState.EXPIRED:
				SetExpiredState();
				break;
			}
			if (registerTimeEvents)
			{
				SetupTimeEvents();
			}
		}

		private void SetupTimeEvents()
		{
			timeEventService.AddEvent(timeEventId, timeService.CurrentTime(), building.UTCExpiryTime - timeService.CurrentTime(), expiredSignal);
			timeEventService.AddEvent(timeEventId, timeService.CurrentTime(), building.UTCCooldownTime - timeService.CurrentTime(), preloadSignal);
		}

		private void OnExpiredSignalFired(int id)
		{
			UpdateState(global::Kampai.Game.MIBBuildingState.EXPIRED, false);
		}

		private void OnPreloadSignalFired(int id)
		{
			UpdateState(global::Kampai.Game.MIBBuildingState.PRELOADING);
		}

		private void SetExpiredState()
		{
			building.MIBState = global::Kampai.Game.MIBBuildingState.EXPIRED;
			logger.Debug("MIB: Ad expired (or interacted with), setting building state to {0}", building.MIBState);
			DisplayBuilding(false);
		}

		private void SetPreloadingState()
		{
			building.MIBState = global::Kampai.Game.MIBBuildingState.PRELOADING;
			logger.Debug("MIB: Ad is ready to preload, setting building state to {0}", building.MIBState);
			DisplayBuilding(false);
			building.UTCExpiryTime = timeService.CurrentTime() + building.Definition.ExpiryInSeconds;
			building.UTCCooldownTime = timeService.CurrentTime() + building.Definition.CooldownInSeconds;
			PreloadThenSetReadyState();
		}

		private void PreloadThenSetReadyState()
		{
			global::Kampai.Main.HindsightCampaign cachedContent = hindsightService.GetCachedContent(global::Kampai.Main.HindsightCampaign.Scope.message_in_a_bottle);
			if (cachedContent != null)
			{
				SetReadyState(global::Kampai.Main.HindsightCampaign.Scope.message_in_a_bottle);
			}
		}

		private void SetReadyState(global::Kampai.Main.HindsightCampaign.Scope scope)
		{
			if (scope == global::Kampai.Main.HindsightCampaign.Scope.message_in_a_bottle)
			{
				building.MIBState = global::Kampai.Game.MIBBuildingState.READY;
				logger.Debug("MIB: Ad is ready to show, setting building state to {0}", building.MIBState);
				DisplayBuilding(true);
			}
		}

		private void OnContentDismissed(global::Kampai.Main.HindsightCampaign.Scope scope, global::Kampai.Main.HindsightCampaign.DismissType dismissType)
		{
			if (scope == global::Kampai.Main.HindsightCampaign.Scope.message_in_a_bottle)
			{
				if (dismissType == global::Kampai.Main.HindsightCampaign.DismissType.ACCEPTED)
				{
					mibService.SetReturningKey();
				}
				else
				{
					mibService.ClearReturningKey();
				}
				logger.Debug("MIB: Ad has been dismissed, dismiss type: {0}", dismissType);
				global::Kampai.Game.MIBBuilding firstInstanceByDefinitionId = playerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.MIBBuilding>(3129);
				int num = firstInstanceByDefinitionId.Definition.FirstXTapsWeightedDefinitionId;
				if (firstInstanceByDefinitionId.NumOfRewardsCollectedOnTap > firstInstanceByDefinitionId.Definition.AfterXTapRewards)
				{
					num = firstInstanceByDefinitionId.Definition.SecondXTapsWeightedDefinitionId;
				}
				global::Kampai.Game.Transaction.TransactionDefinition transactionDefinition = mibService.PickWeightedTransaction(num);
				if (transactionDefinition == null)
				{
					logger.Error("MIB: Failed to find weighted definition id: {0}, will not grant user rewards", num);
				}
				else
				{
					OnGrantRewards(global::Kampai.Game.MIBRewardType.ON_TAP, transactionDefinition, new global::UnityEngine.Vector3(firstInstanceByDefinitionId.Location.x, 0f, firstInstanceByDefinitionId.Location.y));
					telemetryService.Send_Telemetry_EVT_DCN(GetButtonPressedString(dismissType), scope.ToString(), scope.ToString());
				}
			}
		}

		private string GetButtonPressedString(global::Kampai.Main.HindsightCampaign.DismissType dismissType)
		{
			if (dismissType == global::Kampai.Main.HindsightCampaign.DismissType.ACCEPTED)
			{
				return "Yes";
			}
			return "No";
		}

		private void OnAppResume()
		{
			logger.Debug("MIB: Ad has resumed from the background, let's check if we should grant the user rewards");
			CheckForRewards();
		}

		private void CheckForRewards()
		{
			if (IsUserReturning())
			{
				DisplayResourceIcon();
			}
		}

		private bool IsUserReturning()
		{
			return building != null && !building.Definition.DisableReturnRewards && mibService.IsUserReturning();
		}

		private void DisplayResourceIcon()
		{
			logger.Debug("MIB: Showing resource icon");
			createResourceIconSignal.Dispatch(new global::Kampai.UI.View.ResourceIconSettings(building.ID, -1, 1));
		}

		private void DisplayBuilding(bool display)
		{
			if (!display && IsUserReturning())
			{
				view.EnableRenderers(true);
			}
			else
			{
				view.EnableRenderers(display);
			}
			view.UpdateColliders(display);
		}

		private void OnGrantRewards(global::Kampai.Game.MIBRewardType rewardType, global::Kampai.Game.Transaction.TransactionDefinition pickedTransactionDef, global::UnityEngine.Vector3 tweenLocation)
		{
			switch (rewardType)
			{
			case global::Kampai.Game.MIBRewardType.ON_TAP:
				UpdateState(global::Kampai.Game.MIBBuildingState.EXPIRED, false);
				break;
			case global::Kampai.Game.MIBRewardType.ON_RETURN:
				mibService.ClearReturningKey();
				UpdateState(building.MIBState, false);
				break;
			}
			GrantRewards(rewardType, pickedTransactionDef, tweenLocation);
		}

		private bool IsRewardTypeDisabled(global::Kampai.Game.MIBRewardType rewardType)
		{
			switch (rewardType)
			{
			case global::Kampai.Game.MIBRewardType.ON_TAP:
				return building.Definition.DisableTapRewards;
			case global::Kampai.Game.MIBRewardType.ON_RETURN:
				return building.Definition.DisableReturnRewards;
			default:
				return false;
			}
		}

		private void IncrementRewardCollected(global::Kampai.Game.MIBRewardType rewardType)
		{
			switch (rewardType)
			{
			case global::Kampai.Game.MIBRewardType.ON_TAP:
				building.NumOfRewardsCollectedOnTap++;
				break;
			case global::Kampai.Game.MIBRewardType.ON_RETURN:
				building.NumOfRewardsCollectedOnReturn++;
				break;
			}
		}

		private void GrantRewards(global::Kampai.Game.MIBRewardType rewardType, global::Kampai.Game.Transaction.TransactionDefinition pickedTransactionDef, global::UnityEngine.Vector3 tweenLocation)
		{
			if (IsRewardTypeDisabled(rewardType))
			{
				return;
			}
			playerService.RunEntireTransaction(pickedTransactionDef, global::Kampai.Game.TransactionTarget.NO_VISUAL, null, new global::Kampai.Game.TransactionArg(rewardType.ToString()));
			IncrementRewardCollected(rewardType);
			if (rewardType == global::Kampai.Game.MIBRewardType.ON_RETURN)
			{
				spawnDooberSignal.Dispatch(uiCamera.WorldToScreenPoint(tweenLocation), DooberUtil.GetDestinationType(pickedTransactionDef.Outputs[0].ID, definitionService), -1, false);
				return;
			}
			foreach (global::Kampai.Util.QuantityItem output in pickedTransactionDef.Outputs)
			{
				spawnDooberSignal.Dispatch(tweenLocation, DooberUtil.GetDestinationType(output.ID, definitionService), -1, true);
			}
		}
	}
}
