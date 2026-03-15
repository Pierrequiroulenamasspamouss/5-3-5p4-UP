namespace Kampai.UI.View
{
	public class TSMGiftModalMediator : global::Kampai.UI.View.UIStackMediator<global::Kampai.UI.View.TSMGiftModalView>, global::Kampai.Game.IDefinitionsHotSwapHandler
	{
		private global::Kampai.Game.Trigger.TriggerInstance instance;

		private global::Kampai.Game.RewardTriggerSignal rewardTriggerSignal;

		private global::Kampai.UI.View.CloseTSMModalSignal closeModalSignal;

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.UI.IFancyUIService fancyUIService { get; set; }

		[Inject]
		public global::Kampai.Main.PlayGlobalSoundFXSignal playSFXSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.HideSkrimSignal hideSkrimSignal { get; set; }

		[Inject]
		public global::Kampai.Main.MoveAudioListenerSignal moveAudioListenerSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.IGUIService guiService { get; set; }

		[Inject(global::Kampai.Game.GameElement.CONTEXT)]
		public global::strange.extensions.context.api.ICrossContextCapable gameContext { get; set; }

		public override void Initialize(global::Kampai.UI.View.GUIArguments args)
		{
			base.Initialize(args);
			instance = args.Get<global::Kampai.Game.Trigger.TriggerInstance>();
			base.view.InitializeView(instance, fancyUIService, guiService, moveAudioListenerSignal);
			playSFXSignal.Dispatch("Play_menu_popUp_01");
		}

		public override void OnRegister()
		{
			base.OnRegister();
			base.view.OnRewardCollected.AddListener(OnCollectReward);
			base.view.OnMenuClose.AddListener(OnMenuClose);
			rewardTriggerSignal = gameContext.injectionBinder.GetInstance<global::Kampai.Game.RewardTriggerSignal>();
			closeModalSignal = gameContext.injectionBinder.GetInstance<global::Kampai.UI.View.CloseTSMModalSignal>();
			closeModalSignal.AddListener(OnMenuClose);
		}

		public override void OnRemove()
		{
			base.OnRemove();
			base.view.OnRewardCollected.RemoveListener(OnCollectReward);
			base.view.OnMenuClose.RemoveListener(OnMenuClose);
			if (closeModalSignal != null)
			{
				closeModalSignal.RemoveListener(OnMenuClose);
			}
		}

		protected override void Close()
		{
			moveAudioListenerSignal.Dispatch(true, null);
			playSFXSignal.Dispatch("Play_menu_disappear_01");
			base.view.Release();
			base.view.Close();
		}

		private void OnCollectReward(global::Kampai.Game.Trigger.TriggerRewardDefinition triggerReward)
		{
			if (rewardTriggerSignal != null && triggerReward != null)
			{
				rewardTriggerSignal.Dispatch(instance, triggerReward);
			}
		}

		private void OnMenuClose()
		{
			hideSkrimSignal.Dispatch("ProceduralTaskSkrim");
			guiService.Execute(global::Kampai.UI.View.GUIOperation.Unload, "popup_TSM_Gift_Upsell");
		}

		public void OnDefinitionsHotSwap(global::Kampai.Game.IDefinitionService definitionService)
		{
			Close();
			global::Kampai.UI.View.IGUICommand iGUICommand = guiService.BuildCommand(global::Kampai.UI.View.GUIOperation.Queue, "popup_TSM_Gift_Upsell");
			iGUICommand.skrimScreen = "ProceduralTaskSkrim";
			iGUICommand.darkSkrim = true;
			global::System.Collections.Generic.IList<global::Kampai.Game.Trigger.TriggerDefinition> triggerDefinitions = definitionService.GetTriggerDefinitions();
			global::Kampai.Game.Trigger.TriggerDefinition definition = null;
			foreach (global::Kampai.Game.Trigger.TriggerDefinition item in triggerDefinitions)
			{
				if (item.ID == instance.ID)
				{
					definition = item;
					break;
				}
			}
			instance.OnDefinitionHotSwap(definition);
			iGUICommand.Args.Add(typeof(global::Kampai.Game.Trigger.TriggerInstance), instance);
			guiService.Execute(iGUICommand);
		}
	}
}
