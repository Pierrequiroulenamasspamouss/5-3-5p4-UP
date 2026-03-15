namespace Kampai.UI.View
{
	public class ProceduralQuestMediator : global::Kampai.UI.View.UIStackMediator<global::Kampai.UI.View.ProceduralQuestView>
	{
		private global::Kampai.Game.Quest quest;

		private int itemCountOnPreviousUpdate = -1;

		[Inject]
		public global::Kampai.Main.ILocalizationService localService { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.Game.DeliverTaskItemSignal deliverTaskItemSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.UpdateProceduralQuestPanelSignal updateSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.CloseAllOtherMenuSignal closeSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.HideSkrimSignal hideSkrimSignal { get; set; }

		[Inject]
		public global::Kampai.Game.CancelTSMQuestTaskSignal cancelQuestTaskSignal { get; set; }

		[Inject]
		public global::Kampai.Game.StartQuestTaskSignal startQuestTaskSignal { get; set; }

		[Inject]
		public global::Kampai.Game.CollectTSMQuestTaskRewardSignal collectRewardSignal { get; set; }

		[Inject]
		public global::Kampai.Main.PlayGlobalSoundFXSignal playSFXSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.IGUIService guiService { get; set; }

		[Inject(global::Kampai.Game.GameElement.CONTEXT)]
		public global::strange.extensions.context.api.ICrossContextCapable gameContext { get; set; }

		[Inject]
		public global::Kampai.UI.IFancyUIService fancyUIService { get; set; }

		[Inject]
		public global::Kampai.UI.View.SetStorageCapacitySignal setStorageCapacitySignal { get; set; }

		[Inject]
		public global::Kampai.Main.MoveAudioListenerSignal moveAudioListenerSignal { get; set; }

		public override void OnRegister()
		{
			closeSignal.Dispatch(null);
			base.OnRegister();
			base.view.Init(localService, fancyUIService, moveAudioListenerSignal);
			updateSignal.AddListener(UpdateView);
			base.view.OnMenuClose.AddListener(OnMenuClose);
			base.view.YesSellButton.onClick.AddListener(YesSell);
			base.view.NoSellButton.onClick.AddListener(DeclineQuest);
			gameContext.injectionBinder.GetInstance<global::Kampai.Game.DisplayMinionSelectedIconSignal>().Dispatch(301, true);
			gameContext.injectionBinder.GetInstance<global::Kampai.Game.ToggleVignetteSignal>().Dispatch(true, 0f);
		}

		public override void OnRemove()
		{
			base.OnRemove();
			updateSignal.RemoveListener(UpdateView);
			base.view.OnMenuClose.RemoveListener(OnMenuClose);
			CleanupListeners();
			gameContext.injectionBinder.GetInstance<global::Kampai.Game.DisplayMinionSelectedIconSignal>().Dispatch(301, false);
			gameContext.injectionBinder.GetInstance<global::Kampai.Game.ToggleVignetteSignal>().Dispatch(false, null);
		}

		private void CleanupListeners()
		{
			base.view.YesSellButton.onClick.RemoveListener(YesSell);
			base.view.NoSellButton.onClick.RemoveListener(DeclineQuest);
		}

		public override void Initialize(global::Kampai.UI.View.GUIArguments args)
		{
			int questInstanceId = args.Get<int>();
			UpdateView(questInstanceId);
		}

		private void UpdateView(int questInstanceId)
		{
			quest = playerService.GetByInstanceId<global::Kampai.Game.Quest>(questInstanceId);
			if (quest == null || quest.GetActiveDefinition() == null || quest.GetActiveDefinition().SurfaceType != global::Kampai.Game.QuestSurfaceType.ProcedurallyGenerated)
			{
				return;
			}
			global::System.Collections.Generic.IList<global::Kampai.Game.QuestStep> steps = quest.Steps;
			if (steps == null || steps.Count == 0 || steps[0] == null || quest.GetActiveDefinition().QuestSteps[0] == null)
			{
				return;
			}
			int rewardCount = 0;
			global::Kampai.Game.ItemDefinition inputItem = definitionService.Get<global::Kampai.Game.ItemDefinition>(quest.GetActiveDefinition().QuestSteps[0].ItemDefinitionID);
			global::Kampai.Game.ItemDefinition outputItem = null;
			global::Kampai.Game.Transaction.TransactionDefinition reward = quest.GetActiveDefinition().GetReward(definitionService);
			if (reward != null)
			{
				global::System.Collections.Generic.IList<global::Kampai.Util.QuantityItem> outputs = reward.Outputs;
				if (outputs.Count > 0)
				{
					rewardCount = (int)outputs[0].Quantity;
					outputItem = definitionService.Get<global::Kampai.Game.ItemDefinition>(outputs[0].ID);
				}
			}
			if (quest.state == global::Kampai.Game.QuestState.Harvestable)
			{
				Collect();
			}
			else
			{
				HandleNonHarvestable(rewardCount, inputItem, outputItem);
			}
		}

		private void HandleNonHarvestable(int rewardCount, global::Kampai.Game.ItemDefinition inputItem, global::Kampai.Game.ItemDefinition outputItem)
		{
			global::Kampai.Game.QuestStepDefinition questStepDefinition = quest.GetActiveDefinition().QuestSteps[0];
			int itemAmount = questStepDefinition.ItemAmount;
			int quantityByDefinitionId = (int)playerService.GetQuantityByDefinitionId(questStepDefinition.ItemDefinitionID);
			base.view.InitSellView(itemAmount, quantityByDefinitionId, rewardCount, inputItem, outputItem);
			if (itemCountOnPreviousUpdate < 0)
			{
				itemCountOnPreviousUpdate = quantityByDefinitionId;
			}
			else if (itemCountOnPreviousUpdate != quantityByDefinitionId)
			{
				itemCountOnPreviousUpdate = quantityByDefinitionId;
				setStorageCapacitySignal.Dispatch();
			}
		}

		protected override void Close()
		{
			moveAudioListenerSignal.Dispatch(true, null);
			playSFXSignal.Dispatch("Play_menu_disappear_01");
			base.view.Close();
		}

		private void OnMenuClose()
		{
			hideSkrimSignal.Dispatch("ProceduralTaskSkrim");
			guiService.Execute(global::Kampai.UI.View.GUIOperation.Unload, "popup_TSM_SellItems");
		}

		private void Collect()
		{
			CleanupListeners();
			collectRewardSignal.Dispatch(quest);
			closeSignal.Dispatch(null);
		}

		private void DeclineQuest()
		{
			playSFXSignal.Dispatch("Play_button_click_01");
			cancelQuestTaskSignal.Dispatch(quest);
			closeSignal.Dispatch(null);
		}

		private void YesSell()
		{
			if (!base.view.IsAnimationPlaying("Close"))
			{
				playSFXSignal.Dispatch("Play_button_click_01");
				startQuestTaskSignal.Dispatch(quest);
				deliverTaskItemSignal.Dispatch(new global::Kampai.Util.Tuple<int, int>(quest.ID, 0));
			}
		}
	}
}
