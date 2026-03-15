namespace Kampai.UI.View
{
	public class QuestRewardMediator : global::Kampai.UI.View.UIStackMediator<global::Kampai.UI.View.QuestRewardView>
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("QuestRewardMediator") as global::Kampai.Util.IKampaiLogger;

		private global::Kampai.Game.Quest q;

		private bool collected;

		private global::Kampai.UI.View.ModalSettings modalSettings = new global::Kampai.UI.View.ModalSettings();

		private readonly global::Kampai.Game.AdPlacementName adPlacementName = global::Kampai.Game.AdPlacementName.QUEST;

		private global::Kampai.Game.AdPlacementInstance adPlacementInstance;

		[Inject]
		public global::Kampai.Common.ITelemetryService telemetryService { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.IQuestService questService { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.UI.IBuildMenuService buildMenuService { get; set; }

		[Inject]
		public global::Kampai.UI.View.CloseAllOtherMenuSignal closeSignal { get; set; }

		[Inject]
		public global::Kampai.Main.ILocalizationService localService { get; set; }

		[Inject(global::Kampai.UI.View.UIElement.CAMERA)]
		public global::UnityEngine.Camera uiCamera { get; set; }

		[Inject]
		public global::Kampai.Main.PlayGlobalSoundFXSignal soundFXSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.SpawnDooberSignal tweenSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.IGUIService guiService { get; set; }

		[Inject]
		public global::Kampai.UI.View.FTUERewardClosed ftueRewardClosed { get; set; }

		[Inject]
		public global::Kampai.UI.View.HideSkrimSignal hideSkrim { get; set; }

		[Inject]
		public global::Kampai.UI.View.HideDelayHUDSignal hideDelayHUDSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.HideDelayStoreSignal hideDelayStoreSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IZoomCameraModel zoomCameraModel { get; set; }

		[Inject]
		public global::Kampai.UI.IFancyUIService fancyUIService { get; set; }

		[Inject]
		public global::Kampai.Main.MoveAudioListenerSignal toggleCharacterAudioSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.ShowAllWayFindersSignal showAllWayFindersSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.HideAllWayFindersSignal hideAllWayFindersSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.IncreaseInventoryCountForBuildMenuSignal increaseInventoryCountSignal { get; set; }

		[Inject(global::Kampai.Game.GameElement.CONTEXT)]
		public global::strange.extensions.context.api.ICrossContextCapable gameContext { get; set; }

		[Inject]
		public global::Kampai.Game.IMasterPlanQuestService masterPlanQuestService { get; set; }

		[Inject]
		public global::Kampai.Game.IMasterPlanService masterPlanService { get; set; }

		[Inject]
		public global::Kampai.UI.View.ResetLairWayfinderIconSignal resetIconSignal { get; set; }

		[Inject]
		public global::Kampai.Game.DisplayPlayerTrainingSignal playerTrainingSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IRewardedAdService rewardedAdService { get; set; }

		[Inject]
		public global::Kampai.Game.RewardedAdRewardSignal rewardedAdRewardSignal { get; set; }

		[Inject]
		public global::Kampai.Game.AdPlacementActivityStateChangedSignal adPlacementActivityStateChangedSignal { get; set; }

		public override void OnRegister()
		{
			base.OnRegister();
			soundFXSignal.Dispatch("Play_completeQuest_01");
			toggleCharacterAudioSignal.Dispatch(false, base.view.MinionSlot.transform);
			collected = false;
			base.view.collectButton.ClickedSignal.AddListener(Collect);
			base.view.adVideoButton.ClickedSignal.AddListener(AdVideo);
			base.view.OnMenuClose.AddListener(OnMenuClose);
			rewardedAdRewardSignal.AddListener(OnRewardedAdReward);
			adPlacementActivityStateChangedSignal.AddListener(OnAdPlacementActivityStateChanged);
			hideAllWayFindersSignal.Dispatch();
		}

		public override void OnRemove()
		{
			base.OnRemove();
			base.view.collectButton.ClickedSignal.RemoveListener(Collect);
			base.view.adVideoButton.ClickedSignal.RemoveListener(AdVideo);
			base.view.OnMenuClose.RemoveListener(OnMenuClose);
			rewardedAdRewardSignal.RemoveListener(OnRewardedAdReward);
			adPlacementActivityStateChangedSignal.RemoveListener(OnAdPlacementActivityStateChanged);
			showAllWayFindersSignal.Dispatch();
		}

		public override void Initialize(global::Kampai.UI.View.GUIArguments args)
		{
			int id = args.Get<int>();
			modalSettings.enableCollectThrob = args.Contains<global::Kampai.UI.ThrobCollectButton>();
			RegisterRewards(id);
		}

		private void RegisterRewards(int id)
		{
			q = masterPlanQuestService.GetQuestByInstanceId(id);
			if (q == null)
			{
				toggleCharacterAudioSignal.Dispatch(true, null);
				logger.Error("You are trying to show a quest reward with an empty quest: {0}. Something is wrong.", id);
				OnMenuClose();
				return;
			}
			base.view.Init(q, localService, definitionService, playerService, modalSettings, fancyUIService, questService);
			if (q.GetActiveDefinition().type == global::Kampai.Game.QuestType.MasterPlan)
			{
				base.view.SetupVillainAtlas();
			}
			UpdateAdButton();
		}

		protected override void Close()
		{
			toggleCharacterAudioSignal.Dispatch(true, null);
			soundFXSignal.Dispatch("Play_menu_disappear_01");
			if (q != null)
			{
				Collect();
			}
		}

		private void Collect()
		{
			ExecuteRewardTransaction(1u);
		}

		private void ExecuteRewardTransaction(uint multiplier = 1)
		{
			if (q == null || collected)
			{
				return;
			}
			collected = true;
			global::Kampai.Game.QuestDefinition activeDefinition = q.GetActiveDefinition();
			global::Kampai.Game.Transaction.TransactionDefinition transactionDefinition = GetQuestReward();
			if (transactionDefinition != null)
			{
				if (multiplier > 1)
				{
					transactionDefinition = GetMultiReward(transactionDefinition, multiplier);
				}
				global::Kampai.Game.TransactionArg arg = new global::Kampai.Game.TransactionArg((activeDefinition.type != global::Kampai.Game.QuestType.MasterPlan) ? "Quest" : "MasterPlan");
				playerService.RunEntireTransaction(transactionDefinition, global::Kampai.Game.TransactionTarget.NO_VISUAL, CollectTransactionCallback, arg);
			}
		}

		private global::Kampai.Game.Transaction.TransactionDefinition GetQuestReward()
		{
			if (q == null)
			{
				return null;
			}
			global::Kampai.Game.QuestDefinition activeDefinition = q.GetActiveDefinition();
			return activeDefinition.GetReward(definitionService);
		}

		private global::Kampai.Game.Transaction.TransactionDefinition GetMultiReward(global::Kampai.Game.Transaction.TransactionDefinition transaction, uint multipier)
		{
			global::Kampai.Game.Transaction.TransactionDefinition transactionDefinition = transaction.CopyTransaction();
			global::System.Collections.Generic.IList<global::Kampai.Util.QuantityItem> outputs = transactionDefinition.Outputs;
			if (outputs != null)
			{
				for (int i = 0; i < outputs.Count; i++)
				{
					outputs[i].Quantity *= multipier;
				}
			}
			return transactionDefinition;
		}

		private void AdVideo()
		{
			if (adPlacementInstance != null)
			{
				rewardedAdService.ShowRewardedVideo(adPlacementInstance);
			}
		}

		private void UpdateAdButton()
		{
			bool flag = rewardedAdService.IsPlacementActive(adPlacementName);
			global::Kampai.Game.AdPlacementInstance placementInstance = rewardedAdService.GetPlacementInstance(adPlacementName);
			bool flag2 = IsQuestRewardTypeAllowed(placementInstance);
			bool adEnabled = flag && flag2 && placementInstance != null;
			base.view.EnableAdButton(adEnabled);
			adPlacementInstance = placementInstance;
		}

		private bool IsQuestRewardTypeAllowed(global::Kampai.Game.AdPlacementInstance placement)
		{
			if (placement == null)
			{
				return false;
			}
			if (q == null)
			{
				return false;
			}
			global::Kampai.Game.QuestDefinition activeDefinition = q.GetActiveDefinition();
			if (activeDefinition != null)
			{
				if (activeDefinition.ForceDisableRewardedAd2xReward)
				{
					return false;
				}
				if (activeDefinition.ForceEnableRewardedAd2xReward)
				{
					return true;
				}
			}
			global::Kampai.Game.Quest2xRewardDefinition quest2xRewardDefinition = placement.Definition as global::Kampai.Game.Quest2xRewardDefinition;
			if (quest2xRewardDefinition != null)
			{
				global::System.Collections.Generic.List<int> allowedRewardItemTypes = quest2xRewardDefinition.AllowedRewardItemTypes;
				if (allowedRewardItemTypes != null)
				{
					global::Kampai.Game.Transaction.TransactionDefinition questReward = GetQuestReward();
					if (questReward != null)
					{
						global::System.Collections.Generic.IList<global::Kampai.Util.QuantityItem> outputs = questReward.Outputs;
						if (outputs != null && outputs.Count > 0)
						{
							foreach (global::Kampai.Util.QuantityItem item in outputs)
							{
								if (!allowedRewardItemTypes.Contains(item.ID))
								{
									return false;
								}
							}
							return true;
						}
					}
				}
			}
			return false;
		}

		private void OnRewardedAdReward(global::Kampai.Game.AdPlacementInstance placement)
		{
			if (placement.Equals(adPlacementInstance))
			{
				ExecuteRewardTransaction(2u);
				rewardedAdService.RewardPlayer(null, placement);
				global::Kampai.Game.Transaction.TransactionDefinition questReward = GetQuestReward();
				if (questReward != null)
				{
					telemetryService.Send_Telemetry_EVT_AD_INTERACTION(placement.Definition.Name, questReward.Outputs, placement.RewardPerPeriodCount);
				}
				adPlacementInstance = null;
			}
		}

		private void OnAdPlacementActivityStateChanged(global::Kampai.Game.AdPlacementInstance placement, bool enabled)
		{
			UpdateAdButton();
		}

		private void CollectTransactionCallback(global::Kampai.Game.PendingCurrencyTransaction pct)
		{
			if (pct.Success)
			{
				if (q.GetActiveDefinition().type == global::Kampai.Game.QuestType.MasterPlan)
				{
					global::Kampai.Game.MasterPlanQuestType.Component component = q as global::Kampai.Game.MasterPlanQuestType.Component;
					if (component != null)
					{
						if (component.component != null)
						{
							component.component.State = ((!component.isBuildQuest) ? global::Kampai.Game.MasterPlanComponentState.TasksCollected : global::Kampai.Game.MasterPlanComponentState.Complete);
							if (component.component.State == global::Kampai.Game.MasterPlanComponentState.Complete)
							{
								global::Kampai.Game.MasterPlanDefinition definition = masterPlanService.CurrentMasterPlan.Definition;
								global::Kampai.Game.VillainDefinition villainDefinition = definitionService.Get<global::Kampai.Game.VillainDefinition>(definition.VillainCharacterDefID);
								int componentCompleteCount = masterPlanService.GetComponentCompleteCount(definition);
								global::Kampai.Game.MasterPlanComponentBuildingDefinition masterPlanComponentBuildingDefinition = definitionService.Get<global::Kampai.Game.MasterPlanComponentBuildingDefinition>(definition.BuildingDefID);
								global::Kampai.Game.MasterPlanComponentBuildingDefinition masterPlanComponentBuildingDefinition2 = definitionService.Get<global::Kampai.Game.MasterPlanComponentBuildingDefinition>(component.component.buildingDefID);
								telemetryService.Send_Telemetry_EVT_MASTER_PLAN_COMPONENT_COMPLETE(masterPlanComponentBuildingDefinition.TaxonomySpecific, villainDefinition.LocalizedKey, masterPlanComponentBuildingDefinition2.TaxonomySpecific, componentCompleteCount);
							}
						}
						else
						{
							gameContext.injectionBinder.GetInstance<global::Kampai.Game.DisplayMasterPlanRewardDialogSignal>().Dispatch(component.masterPlan);
							playerTrainingSignal.Dispatch(19000030, false, new global::strange.extensions.signal.impl.Signal<bool>());
						}
						resetIconSignal.Dispatch();
					}
				}
				CheckForBuildingAward();
				soundFXSignal.Dispatch("Play_button_click_01");
			}
			DooberUtil.CheckForTween(base.view.transactionDefinition, base.view.viewList, true, uiCamera, tweenSignal, definitionService);
			base.view.CloseView();
		}

		private void CheckForBuildingAward()
		{
			foreach (global::Kampai.Util.QuantityItem output in base.view.transactionDefinition.Outputs)
			{
				int iD = output.ID;
				global::Kampai.Game.BuildingDefinition definition;
				if (definitionService.TryGet<global::Kampai.Game.BuildingDefinition>(iD, out definition))
				{
					switch (definition.Type)
					{
					case BuildingType.BuildingTypeIdentifier.CRAFTING:
					case BuildingType.BuildingTypeIdentifier.DECORATION:
					case BuildingType.BuildingTypeIdentifier.LEISURE:
					case BuildingType.BuildingTypeIdentifier.RESOURCE:
					{
						int storeItemDefinitionIDFromBuildingID = buildMenuService.GetStoreItemDefinitionIDFromBuildingID(iD);
						global::Kampai.Game.StoreItemDefinition storeItemDefinition = definitionService.Get<global::Kampai.Game.StoreItemDefinition>(storeItemDefinitionIDFromBuildingID);
						buildMenuService.AddUncheckedInventoryItem(storeItemDefinition.Type, iD);
						increaseInventoryCountSignal.Dispatch();
						break;
					}
					}
				}
			}
		}

		private void OnMenuClose()
		{
			if (base.view.rewardDisplay != null)
			{
				StopCoroutine(base.view.rewardDisplay);
			}
			ftueRewardClosed.Dispatch();
			if (q != null)
			{
				gameContext.injectionBinder.GetInstance<global::Kampai.Game.GoToNextQuestStateSignal>().Dispatch(q.GetActiveDefinition().ID);
			}
			if (zoomCameraModel.ZoomedIn && zoomCameraModel.LastZoomBuildingType == global::Kampai.Game.BuildingZoomType.TIKIBAR)
			{
				hideDelayHUDSignal.Dispatch(3f);
				hideDelayStoreSignal.Dispatch(3f);
			}
			hideSkrim.Dispatch("QuestRewardSkrim");
			guiService.Execute(global::Kampai.UI.View.GUIOperation.Unload, "popup_QuestReward");
			toggleCharacterAudioSignal.Dispatch(true, null);
			closeSignal.Dispatch(base.view.gameObject);
		}
	}
}
