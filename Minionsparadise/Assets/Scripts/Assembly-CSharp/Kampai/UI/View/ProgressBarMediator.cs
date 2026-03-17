namespace Kampai.UI.View
{
	public class ProgressBarMediator : global::strange.extensions.mediation.impl.Mediator
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("ProgressBarMediator") as global::Kampai.Util.IKampaiLogger;

		private int rushCost;

		private bool started;

		[Inject]
		public global::Kampai.UI.View.ProgressBarView view { get; set; }

		[Inject]
		public global::Kampai.Game.ITimeEventService timeEventService { get; set; }

		[Inject]
		public global::Kampai.Main.PlayGlobalSoundFXSignal playSFXSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.UI.IPositionService positionService { get; set; }

		[Inject]
		public global::Kampai.Main.ILocalizationService localizationService { get; set; }

		[Inject(global::Kampai.Game.GameElement.CONTEXT)]
		public global::strange.extensions.context.api.ICrossContextCapable gameContext { get; set; }

		[Inject]
		public global::Kampai.UI.View.RushRevealBuildingSignal rushRevealBuildingSignal { get; set; }

		[Inject(global::Kampai.UI.View.UIElement.CONTEXT)]
		public global::strange.extensions.context.api.ICrossContextCapable uiContext { get; set; }

		[Inject]
		public global::Kampai.UI.View.RemoveWorldProgressSignal removeWorldProgressSignal { get; set; }

		public override void OnRegister()
		{
			view.Init(positionService, gameContext, logger, playerService, localizationService);
			view.OnTimerCompleteSignal.AddListener(OnComplete);
			view.rushButton.ClickedSignal.AddListener(Rush);
			view.OnShowSignal.AddListener(OnShow);
			view.OnRemoveSignal.AddListener(OnRemoveProgressBar);
		}

		public override void OnRemove()
		{
			view.OnTimerCompleteSignal.RemoveListener(OnComplete);
			view.rushButton.ClickedSignal.RemoveListener(Rush);
			view.OnShowSignal.RemoveListener(OnShow);
			view.OnRemoveSignal.RemoveListener(OnRemoveProgressBar);
		}

		private void OnShow()
		{
			started = true;
			view.StartTime(view.startTime, view.endTime);
			InvokeRepeating("UpdateTime", 0.001f, 1f);
		}

		private void OnRemoveProgressBar()
		{
			removeWorldProgressSignal.Dispatch(view.TrackedId);
		}

		public void StopTime()
		{
			if (started)
			{
				view.OnTimerCompleteSignal.RemoveListener(OnComplete);
				view.StopTime();
				CancelInvoke("UpdateTime");
			}
		}

		public void UpdateTime()
		{
			int timeRemaining = timeEventService.GetTimeRemaining(view.TrackedId);
			view.UpdateTime(timeRemaining);
			rushCost = timeEventService.CalculateRushCostForTimer(timeRemaining, global::Kampai.Game.RushActionType.CONSTRUCTION);
			view.SetRushCost(rushCost);
		}

		private void OnComplete(int instanceId)
		{
			if (view.TrackedId == instanceId)
			{
				removeWorldProgressSignal.Dispatch(instanceId);
				StopTime();
			}
		}

		private void Rush()
		{
			if (view.rushButton.isDoubleConfirmed())
			{
				playerService.ProcessRush(rushCost, true, RushTransactionCallback, view.TrackedId);
			}
		}

		private void RushTransactionCallback(global::Kampai.Game.PendingCurrencyTransaction pct)
		{
			if (pct.Success)
			{
				int trackedId = view.TrackedId;
				uiContext.injectionBinder.GetInstance<global::Kampai.UI.View.SetBuildingRushedSignal>().Dispatch(trackedId);
				timeEventService.RushEvent(trackedId);
				rushRevealBuildingSignal.Dispatch(trackedId);
				playSFXSignal.Dispatch("Play_button_premium_01");
			}
		}
	}
}
