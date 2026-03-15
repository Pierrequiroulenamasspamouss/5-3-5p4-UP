namespace Kampai.UI.View
{
	public class QuestPanelMediator : global::Kampai.UI.View.UIStackMediator<global::Kampai.UI.View.QuestPanelView>
	{
		private global::Kampai.UI.View.ModalSettings modalSettings = new global::Kampai.UI.View.ModalSettings();

		private int showQuestRewardID;

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.UI.View.QuestDetailIDSignal idSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.CloseQuestBookSignal closeQuestSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.IGUIService guiService { get; set; }

		[Inject]
		public ILocalPersistanceService localPersist { get; set; }

		[Inject]
		public global::Kampai.Main.ILocalizationService localService { get; set; }

		[Inject]
		public global::Kampai.UI.View.UpdateQuestPanelWithNewQuestSignal updateQuestPanelSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.UpdateQuestLineProgressSignal updateQuestLineProgressSignal { get; set; }

		[Inject]
		public global::Kampai.Main.PlayGlobalSoundFXSignal playSoundFXSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.HideSkrimSignal hideSkrimSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.ShowQuestRewardSignal questRewardSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.FTUEProgressSignal FTUEsignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.FTUEQuestPanelCloseSignal FTUECloseMenuSignal { get; set; }

		[Inject]
		public global::Kampai.Game.ITimeService timeService { get; set; }

		[Inject]
		public global::Kampai.UI.View.QuestUIModel questUIModel { get; set; }

		[Inject]
		public global::Kampai.UI.View.ShowHUDSignal showHUDSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IZoomCameraModel zoomCameraModel { get; set; }

		[Inject]
		public global::Kampai.Game.IPrestigeService prestigeService { get; set; }

		[Inject]
		public global::Kampai.UI.IFancyUIService fancyUIService { get; set; }

		[Inject]
		public global::Kampai.Main.MoveAudioListenerSignal toggleCharacterAudioSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.ShowSettingsButtonSignal showSettingsButtonSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.ShowPetsButtonSignal showPetsButtonSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.SetBuildMenuEnabledSignal setBuildMenuEnabledSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.ShowAllWayFindersSignal showAllWayFindersSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.HideAllWayFindersSignal hideAllWayFindersSignal { get; set; }

		[Inject]
		public global::Kampai.Game.DisplayPlayerTrainingSignal displayPlayerTrainingSignal { get; set; }

		[Inject]
		public global::Kampai.Main.FadeBackgroundAudioSignal fadeBackgroundAudioSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.QuestRewardPopupContentsSignal popupContentsSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.HideRewardDisplaySignal hideRewardDisplaySignal { get; set; }

		[Inject]
		public global::Kampai.Game.IMasterPlanQuestService masterPlanQuestService { get; set; }

		[Inject]
		public global::Kampai.UI.View.UIModel uiModel { get; set; }

		[Inject]
		public global::Kampai.Game.IQuestService questService { get; set; }

		[Inject]
		public global::Kampai.Game.IGoToService goToService { get; set; }

		public override void OnRegister()
		{
			base.OnRegister();
			FTUEsignal.Dispatch();
			base.view.OnMenuClose.AddListener(OnMenuClose);
			base.view.RewardItemButton.ClickedSignal.AddListener(RewardItemsClicked);
			idSignal.AddListener(RegisterID);
			updateQuestPanelSignal.AddListener(UpdateQuestSteps);
			closeQuestSignal.AddListener(Close);
			showSettingsButtonSignal.Dispatch(false);
			setBuildMenuEnabledSignal.Dispatch(false);
			showPetsButtonSignal.Dispatch(false);
			hideAllWayFindersSignal.Dispatch();
			FadeBackgroundAudio(true);
			questService.UpdateMasterPlanQuestLine();
		}

		public override void OnRemove()
		{
			showSettingsButtonSignal.Dispatch(true);
			setBuildMenuEnabledSignal.Dispatch(true);
			showPetsButtonSignal.Dispatch(true);
			base.OnRemove();
			base.view.OnMenuClose.RemoveListener(OnMenuClose);
			base.view.RewardItemButton.ClickedSignal.RemoveListener(RewardItemsClicked);
			idSignal.RemoveListener(RegisterID);
			updateQuestPanelSignal.RemoveListener(UpdateQuestSteps);
			closeQuestSignal.RemoveListener(Close);
			showAllWayFindersSignal.Dispatch();
			FadeBackgroundAudio(false);
		}

		public override void Initialize(global::Kampai.UI.View.GUIArguments args)
		{
			int id = args.Get<int>();
			modalSettings.enableGotoThrob = args.Contains<global::Kampai.UI.ThrobGotoButton>();
			modalSettings.enableDeliverThrob = args.Contains<global::Kampai.UI.ThrobDeliverButton>();
			modalSettings.enablePurchaseButtons = !args.Contains<global::Kampai.UI.DisablePurchaseButton>();
			base.view.Init();
			base.view.questService = questService;
			base.view.localizationService = localService;
			base.view.timeService = timeService;
			base.view.modalSettings = modalSettings;
			toggleCharacterAudioSignal.Dispatch(false, base.view.currentQuestView.MinionSlot.transform);
			RegisterID(id);
			base.closeAllOtherMenuSignal.Dispatch(base.view.gameObject);
		}

		private void RegisterID(int id)
		{
			playSoundFXSignal.Dispatch("Play_menu_popUp_01");
			global::Kampai.Game.Quest questByInstanceId = masterPlanQuestService.GetQuestByInstanceId(id);
			global::System.Collections.Generic.List<global::Kampai.Game.Quest> quests = masterPlanQuestService.GetQuests();
			InitQuestTabs(quests, questByInstanceId, questByInstanceId.GetActiveDefinition().SurfaceID, false);
			if (questByInstanceId.state == global::Kampai.Game.QuestState.Harvestable)
			{
				showQuestRewardID = questByInstanceId.ID;
			}
			global::Kampai.Game.Transaction.TransactionDefinition reward = questByInstanceId.GetActiveDefinition().GetReward(definitionService);
			if (reward != null)
			{
				base.view.CreateQuestSteps(questByInstanceId, reward, definitionService);
				base.view.Open();
			}
			updateQuestLineProgressSignal.Dispatch(id);
			global::Kampai.Game.QuestDefinition definition = questByInstanceId.Definition;
			if (definition.ShowRewardsPopupByDefault && !localPersist.HasKey(definition.ID.ToString()))
			{
				popupContentsSignal.Dispatch(base.view.RewardItemButton.questRewards);
				localPersist.PutDataInt(definition.ID.ToString(), 0);
			}
		}

		private void UpdateQuestSteps(int questId)
		{
			hideRewardDisplaySignal.Dispatch();
			global::Kampai.Game.Quest questByInstanceId = masterPlanQuestService.GetQuestByInstanceId(questId);
			global::System.Collections.Generic.List<global::Kampai.Game.Quest> quests = masterPlanQuestService.GetQuests();
			InitQuestTabs(quests, questByInstanceId, questByInstanceId.GetActiveDefinition().SurfaceID);
			global::Kampai.Game.Transaction.TransactionDefinition reward = questByInstanceId.GetActiveDefinition().GetReward(definitionService);
			if (reward != null)
			{
				base.view.CreateQuestSteps(questByInstanceId, reward, definitionService);
			}
		}

		private void InitQuestTabs(global::System.Collections.Generic.List<global::Kampai.Game.Quest> quests, global::Kampai.Game.Quest selectedQuest, int surfaceId, bool isUpdate = true)
		{
			int lastSelectedQuestID = questUIModel.lastSelectedQuestID;
			questUIModel.lastSelectedQuestID = selectedQuest.ID;
			global::Kampai.Game.Prestige prestige = prestigeService.GetPrestige(surfaceId);
			global::Kampai.UI.DummyCharacterType characterType = global::Kampai.UI.DummyCharacterType.Minion;
			if (prestige.Definition.Type == global::Kampai.Game.PrestigeType.Minion)
			{
				global::Kampai.Game.Definition definition = definitionService.Get<global::Kampai.Game.Definition>(prestige.Definition.TrackedDefinitionID);
				if (definition is global::Kampai.Game.NamedCharacterDefinition)
				{
					characterType = global::Kampai.UI.DummyCharacterType.NamedCharacter;
				}
			}
			else
			{
				characterType = global::Kampai.UI.DummyCharacterType.NamedCharacter;
			}
			if (isUpdate)
			{
				base.view.SetCurrentQuestImage(surfaceId, characterType);
				global::Kampai.Game.Quest questByInstanceId = masterPlanQuestService.GetQuestByInstanceId(lastSelectedQuestID);
				base.view.SwapQuest(questByInstanceId, selectedQuest.ID);
			}
			else
			{
				base.view.InitCurrentQuestImage(surfaceId, fancyUIService, characterType);
				base.view.InitQuestTabs(quests, selectedQuest.ID);
			}
		}

		private void RewardItemsClicked(global::System.Collections.Generic.List<global::Kampai.Game.DisplayableDefinition> itemDefs)
		{
			popupContentsSignal.Dispatch(itemDefs);
		}

		private void OnMenuClose()
		{
			if (!uiModel.GoToClicked && questService.ShouldPulseMoveButtonAccept())
			{
				goToService.OpenStoreFromAnywhere(3123);
			}
			uiModel.GoToClicked = false;
			base.view.RemoveCharacters();
			FTUECloseMenuSignal.Dispatch();
			hideSkrimSignal.Dispatch("QuestPanelSkrim");
			guiService.Execute(global::Kampai.UI.View.GUIOperation.Unload, "screen_QuestPanel");
			if (showQuestRewardID != 0)
			{
				questRewardSignal.Dispatch(showQuestRewardID);
				return;
			}
			global::Kampai.Game.Quest byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.Quest>(questUIModel.lastSelectedQuestID);
			if (byInstanceId != null)
			{
				displayPlayerTrainingSignal.Dispatch(byInstanceId.GetActiveDefinition().QuestModalClosePlayerTrainingCategoryItemId, false, new global::strange.extensions.signal.impl.Signal<bool>());
			}
		}

		protected override void Close()
		{
			toggleCharacterAudioSignal.Dispatch(true, null);
			playSoundFXSignal.Dispatch("Play_menu_disappear_01");
			if (zoomCameraModel.ZoomedIn && zoomCameraModel.LastZoomBuildingType == global::Kampai.Game.BuildingZoomType.TIKIBAR)
			{
				showHUDSignal.Dispatch(false);
			}
			base.view.CloseView();
		}

		private void FadeBackgroundAudio(bool fade)
		{
			if (zoomCameraModel.ZoomedIn && !zoomCameraModel.ZoomInProgress)
			{
				fadeBackgroundAudioSignal.Dispatch(fade, "Play_tikiBar_snapshotDuck_01");
			}
		}
	}
}
