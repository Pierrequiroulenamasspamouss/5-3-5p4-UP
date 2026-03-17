namespace Kampai.UI.View
{
	public class MignetteScoreSummaryMediator : global::Kampai.UI.View.UIStackMediator<global::Kampai.UI.View.MignetteScoreSummaryView>
	{
		private const float initialDooberDelayTime = 1.2f;

		private const float dooberFlyTime = 1f;

		private const float dooberFlyScale = 1.75f;

		private const float waitBetweenDoobers = 0.1f;

		private const int maxDoobersToSend = 5;

		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("MignetteScoreSummaryMediator") as global::Kampai.Util.IKampaiLogger;

		private int mignetteBuildingID;

		private global::Kampai.Game.MignetteBuilding mignetteBuilding;

		private bool showMignetteScoreIncrease;

		private int xpReward;

		private bool hasUnlockedCompositeBuildingReward;

		private global::Kampai.Game.DisplayableDefinition rewardReadyItemDef;

		private global::Kampai.Game.Transaction.TransactionDefinition rewardReadyTransactionDef;

		private bool isClosed;

		private global::Kampai.Game.RewardCollection collection;

		[Inject]
		public global::Kampai.Main.ILocalizationService localizationService { get; set; }

		[Inject]
		public global::Kampai.Game.Mignette.MignetteGameModel mignetteGameModel { get; set; }

		[Inject]
		public global::Kampai.UI.View.IGUIService guiService { get; set; }

		[Inject]
		public global::Kampai.Game.StopMignetteSignal stopMignettSignal { get; set; }

		[Inject]
		public global::Kampai.Game.MignetteCollectionService collectionService { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.UI.View.SetHUDButtonsVisibleSignal setHUDButtonsVisibleSignal { get; set; }

		[Inject]
		public global::Kampai.Game.CreditCollectionRewardSignal creditCollectionRewardSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.SpawnDooberSignal spawnDooberSignal { get; set; }

		[Inject]
		public global::Kampai.Common.ITelemetryService telemetryService { get; set; }

		[Inject(global::Kampai.UI.View.UIElement.CAMERA)]
		public global::UnityEngine.Camera uiCamera { get; set; }

		[Inject]
		public global::Kampai.Main.PlayGlobalMusicSignal musicSignal { get; set; }

		[Inject]
		public global::Kampai.Main.PlayGlobalSoundFXSignal globalAudioSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.HideSkrimSignal hideSkrim { get; set; }

		[Inject]
		public global::Kampai.UI.View.EnableSkrimButtonSignal enableSkrimSignal { get; set; }

		[Inject]
		public global::Kampai.Game.CameraAutoMoveToMignetteSignal moveToMignetteSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.ShowAllWayFindersSignal showAllWayFindersSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.HideAllWayFindersSignal hideAllWayFindersSignal { get; set; }

		[Inject]
		public global::Kampai.Game.ReconcileSalesSignal reconcileSalesSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.RefreshMTXStoreSignal refreshMTXStoreSignal { get; set; }

		[Inject]
		public global::Kampai.Game.MignetteCallMinionsModel model { get; set; }

		public override void Initialize(global::Kampai.UI.View.GUIArguments args)
		{
			hideAllWayFindersSignal.Dispatch();
			showMignetteScoreIncrease = args.Get<bool>();
			mignetteBuildingID = args.Get<int>();
			mignetteBuilding = playerService.GetByInstanceId<global::Kampai.Game.MignetteBuilding>(mignetteBuildingID);
			bool isMignetteUnlocked = true;
			if (mignetteBuilding == null)
			{
				mignetteBuilding = args.Get<global::Kampai.Game.MignetteBuilding>();
				isMignetteUnlocked = false;
			}
			collection = collectionService.GetActiveCollectionForMignette(mignetteBuilding, false);
			collection.NumTimesPlayed++;
			for (int i = 0; i < collection.Definition.Rewards.Count; i++)
			{
				CreateRewardIndicator(collection, collection.Definition.Rewards[i]);
			}
			int score = (showMignetteScoreIncrease ? mignetteGameModel.CurrentGameScore : 0);
			if (showMignetteScoreIncrease && rewardReadyTransactionDef != null)
			{
				collectionService.pendingRewardTransaction = collectionService.CreditRewardForActiveMignette();
				refreshMTXStoreSignal.Dispatch();
			}
			xpReward = global::UnityEngine.Mathf.CeilToInt(mignetteBuilding.Definition.XPRewardFactor);
			base.view.Init(localizationService, score, collection.CollectionScoreProgress, collection.CollectionScorePreReset, collection.GetMaxScore(), xpReward, mignetteBuilding.Definition, showMignetteScoreIncrease, isMignetteUnlocked, hasUnlockedCompositeBuildingReward);
			base.view.ConfirmButton.ClickedSignal.AddListener(OnConfirmButtonClicked);
			base.view.CollectCurrencySignal.AddListener(OnCollectCurrency);
			if (showMignetteScoreIncrease)
			{
				HandleScoreIncrease();
			}
		}

		private void HandleScoreIncrease()
		{
			global::Kampai.UI.View.IGUICommand command = guiService.BuildCommand(global::Kampai.UI.View.GUIOperation.Load, "MignetteHUD");
			global::UnityEngine.GameObject mignetteHUD = guiService.Execute(command);
			StartCoroutine(ensureHUDAvailable(mignetteHUD));
		}

		private global::System.Collections.IEnumerator ensureHUDAvailable(global::UnityEngine.GameObject mignetteHUD)
		{
			yield return new global::UnityEngine.WaitForEndOfFrame();
			global::Kampai.UI.View.MignetteHUDMediator hudMediator = mignetteHUD.GetComponent<global::Kampai.UI.View.MignetteHUDMediator>();
			if (hudMediator != null)
			{
				hudMediator.StartScorePresentationSequence();
			}
			StartCoroutine(spawnDoobersThenCall(base.view.ProgressBarCurrencyIcon.rectTransform, onHUDPresentationFinished));
		}

		private global::System.Collections.IEnumerator spawnDoobersThenCall(global::UnityEngine.RectTransform flyToTarget, global::System.Action completeCallback)
		{
			yield return new global::UnityEngine.WaitForSeconds(1.2f);
			int numDoobers = global::UnityEngine.Mathf.Min(mignetteGameModel.CurrentGameScore, 5);
			globalAudioSignal.Dispatch("Play_Mign_smallScoreEvent_01");
			for (int i = 0; i < numDoobers; i++)
			{
				spawnMignetteCurrencyDoober(flyToTarget);
				globalAudioSignal.Dispatch("Play_mignette_scoreFlyDown_01");
				yield return new global::UnityEngine.WaitForSeconds(0.1f);
			}
			float firstArrivalTimeFromNow = 1f - 0.1f * (float)numDoobers;
			yield return new global::UnityEngine.WaitForSeconds(firstArrivalTimeFromNow);
			completeCallback();
		}

		private void spawnMignetteCurrencyDoober(global::UnityEngine.RectTransform flyToTarget)
		{
			global::UnityEngine.RectTransform currencyImageClone = global::UnityEngine.Object.Instantiate(base.view.GroupScoreCurrencyIcon.rectTransform);
			currencyImageClone.SetParent(base.view.GroupScoreCurrencyIcon.rectTransform, false);
			currencyImageClone.anchoredPosition = global::UnityEngine.Vector2.zero;
			currencyImageClone.transform.position = new global::UnityEngine.Vector3(currencyImageClone.transform.position.x, currencyImageClone.transform.position.y, -10f);
			currencyImageClone.localScale = global::UnityEngine.Vector3.one;
			currencyImageClone.SetParent(flyToTarget, true);
			Go.to(currencyImageClone.transform, 1f, new GoTweenConfig().setEaseType(GoEaseType.CubicInOut).localPosition(global::UnityEngine.Vector3.zero).onComplete(delegate(AbstractGoTween thisTween)
			{
				thisTween.destroy();
				global::UnityEngine.Object.Destroy(currencyImageClone.gameObject);
			}));
			Go.to(currencyImageClone.transform, 0.5f, new GoTweenConfig().setEaseType(GoEaseType.Linear).scale(1.75f));
			Go.to(currencyImageClone.transform, 0.5f, new GoTweenConfig().setDelay(0.5f).setEaseType(GoEaseType.Linear).scale(1f));
		}

		public override void OnRemove()
		{
			base.OnRemove();
			base.view.ConfirmButton.ClickedSignal.RemoveListener(OnConfirmButtonClicked);
			base.view.CollectCurrencySignal.RemoveListener(OnCollectCurrency);
			setHUDButtonsVisibleSignal.Dispatch(true);
			showAllWayFindersSignal.Dispatch();
		}

		private void CreateRewardIndicator(global::Kampai.Game.RewardCollection collection, global::Kampai.Game.CollectionReward reward)
		{
			bool flag = collection.IsRewardReadyForCollection(reward);
			global::Kampai.Game.Transaction.TransactionDefinition transactionDefinition = definitionService.Get<global::Kampai.Game.Transaction.TransactionDefinition>(reward.TransactionID);
			global::Kampai.Game.DisplayableDefinition displayableDefinition = definitionService.Get(transactionDefinition.Outputs[0].ID) as global::Kampai.Game.DisplayableDefinition;
			if (flag && rewardReadyItemDef == null)
			{
				rewardReadyItemDef = displayableDefinition;
				rewardReadyTransactionDef = transactionDefinition;
				int tier = collection.Definition.Rewards.IndexOf(reward) + 1;
				telemetryService.Send_Telemtry_EVT_MINI_TIER_REACHED(mignetteBuilding.Definition.LocalizedKey, tier, collection.NumTimesPlayed);
				if (displayableDefinition is global::Kampai.Game.CompositeBuildingPieceDefinition)
				{
					hasUnlockedCompositeBuildingReward = true;
				}
			}
			base.view.CreateRewardIndicator(reward.RequiredPoints, transactionDefinition.Outputs[0].Quantity, collection.GetMaxScore(), displayableDefinition.Image, displayableDefinition.Mask, flag);
		}

		private void onHUDPresentationFinished()
		{
			base.view.RefreshProgressBar(OnCompleteRefreshProgressBar);
		}

		private void OnCompleteRefreshProgressBar()
		{
			StartCoroutine(AwardXPAndShowButton());
		}

		private global::System.Collections.IEnumerator AwardXPAndShowButton()
		{
			yield return new global::UnityEngine.WaitForSeconds(2f);
			playerService.AddXP(xpReward);
			logger.Log(global::Kampai.Util.KampaiLogLevel.Info, "Awarded " + xpReward + " Fun Points For completing the mignette");
			global::UnityEngine.Vector3 buildingPosition = new global::UnityEngine.Vector3(mignetteBuilding.Location.x, 0f, mignetteBuilding.Location.y);
			spawnDooberSignal.Dispatch(buildingPosition, global::Kampai.UI.View.DestinationType.XP, 2, true);
			globalAudioSignal.Dispatch("Play_drop_harvest_01");
			yield return new global::UnityEngine.WaitForSeconds(2.5f);
			enableSkrimSignal.Dispatch(true);
			base.view.ConfirmButton.gameObject.SetActive(true);
		}

		private void OnCollectCurrency(global::UnityEngine.GameObject spawnDooberTransform)
		{
			StartCoroutine(SpawnDoobersThenCreditReward(spawnDooberTransform));
		}

		private global::System.Collections.IEnumerator SpawnDoobersThenCreditReward(global::UnityEngine.GameObject spawnDooberTransform)
		{
			for (int i = 0; i < 7; i++)
			{
				globalAudioSignal.Dispatch("Play_mignette_rewardFlyUp_01");
				DooberUtil.CheckForTween(rewardReadyTransactionDef, new global::System.Collections.Generic.List<global::UnityEngine.GameObject> { spawnDooberTransform }, true, uiCamera, spawnDooberSignal, definitionService);
				yield return new global::UnityEngine.WaitForSeconds(0.2f);
			}
			yield return new global::UnityEngine.WaitForSeconds(0.8f);
		}

		private void CloseMignetteScoreView()
		{
			if (showMignetteScoreIncrease && !isClosed)
			{
				isClosed = true;
				if (hasUnlockedCompositeBuildingReward)
				{
					creditCollectionRewardSignal.Dispatch();
				}
				hideSkrim.Dispatch("MignetteSkrim");
				guiService.Execute(global::Kampai.UI.View.GUIOperation.Unload, "MignetteScoreSummary");
				stopMignettSignal.Dispatch(false);
				global::System.Collections.Generic.Dictionary<string, float> type = new global::System.Collections.Generic.Dictionary<string, float>();
				musicSignal.Dispatch("Play_backGroundMusic_01", type);
				if (mignetteBuilding.Definition.ID == 3502)
				{
					reconcileSalesSignal.Dispatch(0);
				}
			}
		}

		private void CloseMiniGamesView()
		{
			if (!showMignetteScoreIncrease)
			{
				model.NumberOfMinionsToCall = mignetteBuilding.MinionSlotsOwned;
				model.Building = mignetteBuilding;
				model.SignalSender = base.view.gameObject;
				bool type = ((mignetteBuilding.State != global::Kampai.Game.BuildingState.Cooldown) ? true : false);
				moveToMignetteSignal.Dispatch(mignetteBuilding.Definition.ID, type, new global::Kampai.Game.PanInstructions(null));
			}
		}

		protected override void Close()
		{
			CloseMignetteScoreView();
		}

		private void OnConfirmButtonClicked()
		{
			CloseMignetteScoreView();
			CloseMiniGamesView();
		}
	}
}
