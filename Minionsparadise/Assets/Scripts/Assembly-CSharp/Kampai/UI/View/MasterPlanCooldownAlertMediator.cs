namespace Kampai.UI.View
{
	public class MasterPlanCooldownAlertMediator : global::Kampai.UI.View.UIStackMediator<global::Kampai.UI.View.MasterPlanCooldownAlertView>
	{
		private global::Kampai.Game.MasterPlan plan;

		[Inject]
		public global::Kampai.Game.ITimeEventService timeEventService { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.IMasterPlanService masterPlanService { get; set; }

		[Inject]
		public global::Kampai.Main.ILocalizationService localService { get; set; }

		[Inject]
		public global::Kampai.UI.View.IGUIService guiService { get; set; }

		[Inject]
		public global::Kampai.UI.IFancyUIService fancyUIService { get; set; }

		[Inject]
		public global::Kampai.UI.View.HideSkrimSignal hideSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.SetPremiumCurrencySignal setPremiumCurrencySignal { get; set; }

		[Inject]
		public global::Kampai.Main.PlayGlobalSoundFXSignal playGlobalSFX { get; set; }

		[Inject(global::Kampai.Game.GameElement.CONTEXT)]
		public global::strange.extensions.context.api.ICrossContextCapable gameContext { get; set; }

		[Inject]
		public global::Kampai.Game.VillainLairModel lairModel { get; set; }

		public override void OnRegister()
		{
			base.OnRegister();
			gameContext.injectionBinder.GetInstance<global::Kampai.Game.MasterPlanCooldownCompleteSignal>().AddListener(CoolDownComplete);
			base.view.OnMenuClose.AddListener(CloseAnimationComplete);
			base.view.waitButton.ClickedSignal.AddListener(CloseButton);
			base.view.rushButton.ClickedSignal.AddListener(RushCooldown);
		}

		public override void OnRemove()
		{
			base.OnRemove();
			gameContext.injectionBinder.GetInstance<global::Kampai.Game.MasterPlanCooldownCompleteSignal>().RemoveListener(CoolDownComplete);
			base.view.OnMenuClose.RemoveListener(CloseAnimationComplete);
			base.view.waitButton.ClickedSignal.RemoveListener(CloseButton);
			base.view.rushButton.ClickedSignal.RemoveListener(RushCooldown);
		}

		public override void Initialize(global::Kampai.UI.View.GUIArguments args)
		{
			plan = args.Get<global::Kampai.Game.MasterPlan>();
			bool hasReceivedFirstReward = masterPlanService.HasReceivedInitialRewardFromPlanDefinition(plan.Definition);
			base.view.Init(plan, hasReceivedFirstReward, timeEventService, definitionService, localService, fancyUIService, guiService);
			playGlobalSFX.Dispatch("Play_menu_popUp_01");
			lairModel.seenCooldownAlert = true;
		}

		private void CloseButton()
		{
			Close();
		}

		protected override void Close()
		{
			base.view.Close();
			playGlobalSFX.Dispatch("Play_menu_disappear_01");
		}

		private void CoolDownComplete(int masterplanID)
		{
			Close();
		}

		private void RushCooldown()
		{
			if (base.view.rushButton.isDoubleConfirmed())
			{
				playerService.ProcessRush(base.view.rushCost, true, "MasterPlanRush", RushTransactionCallback);
			}
		}

		private void RushTransactionCallback(global::Kampai.Game.PendingCurrencyTransaction pct)
		{
			if (pct.Success)
			{
				playGlobalSFX.Dispatch("Play_button_premium_01");
				setPremiumCurrencySignal.Dispatch();
				timeEventService.RushEvent(plan.ID);
				Close();
			}
		}

		private void CloseAnimationComplete()
		{
			if (base.view.timerRoutine != null)
			{
				StopCoroutine(base.view.timerRoutine);
			}
			base.view.Cleanup();
			plan.displayCooldownAlert = false;
			if (plan.cooldownUTCStartTime != 0)
			{
				gameContext.injectionBinder.GetInstance<global::Kampai.Game.CleanupMasterPlanSignal>().Dispatch(plan);
			}
			hideSignal.Dispatch("MasterPlanCooldownAlert");
			guiService.Execute(global::Kampai.UI.View.GUIOperation.Unload, "screen_MasterPlanCooldownAlert");
		}
	}
}
