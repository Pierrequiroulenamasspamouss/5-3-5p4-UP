namespace Kampai.UI.View
{
	public class MysteryMinionTeaserSelectionMediator : global::Kampai.UI.View.KampaiMediator
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("MysteryMinionTeaserSelectionMediator") as global::Kampai.Util.IKampaiLogger;

		private int rewardIndex;

		private global::Kampai.Game.Transaction.TransactionDefinition trans1;

		private global::Kampai.Game.Transaction.TransactionDefinition trans2;

		private global::Kampai.Game.Trigger.TriggerInstance trigger;

		[Inject]
		public global::Kampai.UI.View.MysteryMinionTeaserSelectionView view { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.UI.View.CloseAllOtherMenuSignal closeSignal { get; set; }

		[Inject]
		public global::Kampai.Main.PlayGlobalSoundFXSignal playSFXSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.HideSkrimSignal hideSkrimSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.IGUIService guiService { get; set; }

		[Inject]
		public global::Kampai.UI.IFancyUIService fancyUIService { get; set; }

		[Inject]
		public global::Kampai.Common.ITelemetryService telemetryService { get; set; }

		[Inject(global::Kampai.Game.GameElement.CONTEXT)]
		public global::strange.extensions.context.api.ICrossContextCapable gameContext { get; set; }

		[Inject(global::Kampai.UI.View.UIElement.CAMERA)]
		public global::UnityEngine.Camera uiCamera { get; set; }

		[Inject]
		public global::Kampai.UI.View.SpawnDooberSignal tweenSignal { get; set; }

		[Inject]
		public global::Kampai.UI.IBuildMenuService buildMenuService { get; set; }

		[Inject]
		public global::Kampai.UI.View.UpdateUIButtonsSignal updateStoreButtonsSignal { get; set; }

		[Inject]
		public global::Kampai.Main.MoveAudioListenerSignal moveAudioListenerSignal { get; set; }

		public override void Initialize(global::Kampai.UI.View.GUIArguments args)
		{
			closeSignal.Dispatch(view.gameObject);
			view.Initialize(fancyUIService, playSFXSignal);
			trigger = args.Get<global::Kampai.Game.Trigger.TriggerInstance>();
			global::Kampai.Game.PendingRewardDefinition pendingRewardDefinition = GetPendingRewardDefinition(trigger);
			if (pendingRewardDefinition == null)
			{
				logger.Error("Failed to get the pending reward definition");
				Close();
				return;
			}
			if (pendingRewardDefinition.transactions.Count < 2)
			{
				logger.Error("Unable to set up Mystery Minion Coming Modal: definition incomplete");
				Close();
				return;
			}
			SetUpFirstTransaction(pendingRewardDefinition);
			SetUpSecondTransaction(pendingRewardDefinition);
			playSFXSignal.Dispatch("Play_menu_popUp_01");
			moveAudioListenerSignal.Dispatch(false, view.MinionSlot.transform);
		}

		private global::Kampai.Game.PendingRewardDefinition GetPendingRewardDefinition(global::Kampai.Game.Trigger.TriggerInstance triggerInstance)
		{
			if (triggerInstance == null)
			{
				return null;
			}
			global::System.Collections.Generic.IList<global::Kampai.Game.Trigger.TriggerRewardDefinition> rewards = triggerInstance.Definition.rewards;
			if (rewards == null || rewards.Count == 0)
			{
				return null;
			}
			global::Kampai.Game.Trigger.TriggerRewardDefinition triggerRewardDefinition = rewards[0];
			global::Kampai.Game.Trigger.CaptainTeaseTriggerRewardDefinition captainTeaseTriggerRewardDefinition = triggerRewardDefinition as global::Kampai.Game.Trigger.CaptainTeaseTriggerRewardDefinition;
			if (captainTeaseTriggerRewardDefinition == null)
			{
				return null;
			}
			return definitionService.Get<global::Kampai.Game.PendingRewardDefinition>(captainTeaseTriggerRewardDefinition.PendingRewardDefinitionID);
		}

		private void SetUpFirstTransaction(global::Kampai.Game.PendingRewardDefinition prDef)
		{
			trans1 = definitionService.Get<global::Kampai.Game.Transaction.TransactionDefinition>(prDef.transactions[0]);
			view.SetUpRewardIconDisplayable(view.choice1_icon1, definitionService.Get<global::Kampai.Game.DisplayableDefinition>(trans1.Outputs[0].ID), view.choice1_icon1_amt, trans1.Outputs[0].Quantity);
			view.SetUpRewardIconDisplayable(view.choice1_icon2, definitionService.Get<global::Kampai.Game.DisplayableDefinition>(trans1.Outputs[1].ID), view.choice1_icon2_amt, trans1.Outputs[1].Quantity);
		}

		private void SetUpSecondTransaction(global::Kampai.Game.PendingRewardDefinition prDef)
		{
			trans2 = definitionService.Get<global::Kampai.Game.Transaction.TransactionDefinition>(prDef.transactions[1]);
			view.SetUpRewardIconDisplayable(view.choice2_icon1, definitionService.Get<global::Kampai.Game.DisplayableDefinition>(trans2.Outputs[0].ID), view.choice2_icon1_amt, trans2.Outputs[0].Quantity);
			view.SetUpRewardIconDisplayable(view.choice2_icon2, definitionService.Get<global::Kampai.Game.DisplayableDefinition>(trans2.Outputs[1].ID), view.choice2_icon2_amt, trans2.Outputs[1].Quantity);
		}

		private void Clicked_1()
		{
			view.PlayerSelectedFirstReward(true);
			rewardIndex = 0;
		}

		private void Clicked_2()
		{
			view.PlayerSelectedFirstReward(false);
			rewardIndex = 1;
		}

		private void Clicked_Confirm()
		{
			global::Kampai.Game.Transaction.TransactionDefinition transactionDefinition = ((rewardIndex != 0) ? trans2 : trans1);
			playerService.RunEntireTransaction(transactionDefinition, global::Kampai.Game.TransactionTarget.NO_VISUAL, null);
			updateStoreButtonsSignal.Dispatch(false);
			global::System.Collections.Generic.List<global::Kampai.UI.View.KampaiImage> list = new global::System.Collections.Generic.List<global::Kampai.UI.View.KampaiImage>();
			list.Add((rewardIndex != 0) ? view.choice2_icon1 : view.choice1_icon1);
			list.Add((rewardIndex != 0) ? view.choice2_icon2 : view.choice1_icon2);
			DooberUtil.CheckForTween(transactionDefinition, list, true, uiCamera, tweenSignal, definitionService);
			SendBuildingTelemetry(transactionDefinition);
			logger.Debug(string.Format("Player selected reward choice {0}", rewardIndex + 1));
			Close();
		}

		private void SendBuildingTelemetry(global::Kampai.Game.Transaction.TransactionDefinition transactionDefinition)
		{
			for (int i = 0; i < transactionDefinition.Outputs.Count; i++)
			{
				global::Kampai.Util.QuantityItem quantityItem = transactionDefinition.Outputs[i];
				global::Kampai.Game.BuildingDefinition definition;
				definitionService.TryGet<global::Kampai.Game.BuildingDefinition>(quantityItem.ID, out definition);
				if (definition != null)
				{
					for (int j = 0; j < quantityItem.Quantity; j++)
					{
						buildMenuService.CompleteBuildMenuUpdate(definition.Type, definition.ID);
						telemetryService.Send_Telemetry_EVT_USER_ACQUIRES_BUILDING(definition.TaxonomyType, quantityItem.ID, 0);
					}
				}
			}
		}

		protected void Close()
		{
			moveAudioListenerSignal.Dispatch(true, null);
			playSFXSignal.Dispatch("Play_menu_disappear_01");
			view.Close();
		}

		public override void OnRegister()
		{
			base.OnRegister();
			view.OnMenuClose.AddListener(OnMenuClose);
			view.choice1Button.ClickedSignal.AddListener(Clicked_1);
			view.choice2Button.ClickedSignal.AddListener(Clicked_2);
			view.confirmButton.ClickedSignal.AddListener(Clicked_Confirm);
			view.PulseSelectButtons();
		}

		public override void OnRemove()
		{
			base.OnRemove();
			CleanupListeners();
		}

		private void CleanupListeners()
		{
			view.OnMenuClose.RemoveListener(OnMenuClose);
			view.choice1Button.ClickedSignal.RemoveListener(Clicked_1);
			view.choice2Button.ClickedSignal.RemoveListener(Clicked_2);
			view.confirmButton.ClickedSignal.RemoveListener(Clicked_Confirm);
		}

		private void OnMenuClose()
		{
			if (trigger != null && trigger.Definition.rewards.Count > 0)
			{
				gameContext.injectionBinder.GetInstance<global::Kampai.Game.RewardTriggerSignal>().Dispatch(trigger, trigger.Definition.rewards[0]);
			}
			view.Release();
			hideSkrimSignal.Dispatch("TSMTeaseSkrim");
			guiService.Execute(global::Kampai.UI.View.GUIOperation.Unload, "screen_MysteryMinionTeaserSelectionModal");
		}
	}
}
