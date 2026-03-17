namespace Kampai.Main.View
{
	public class AppTrackerMediator : global::strange.extensions.mediation.impl.Mediator
	{
		private global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("AppTrackerMediator") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public AppTrackerView view { get; set; }

		[Inject]
		public global::Kampai.Common.AppPauseSignal pauseSignal { get; set; }

		[Inject]
		public global::Kampai.Common.AppResumeSignal resumeSignal { get; set; }

		[Inject]
		public global::Kampai.Common.AppQuitSignal quitSignal { get; set; }

		[Inject]
		public global::Kampai.Common.AppFocusGainedSignal focusGainedSignal { get; set; }

		[Inject]
		public global::Kampai.Common.AppEarlyPauseSignal earlyPauseSignal { get; set; }

		public override void OnRegister()
		{
			logger.Debug("AppTrackerMediator.OnRegister");
			view.pauseSignal = pauseSignal;
			view.resumeSignal = resumeSignal;
			view.quitSignal = quitSignal;
			view.focusGainedSignal = focusGainedSignal;
			view.earlyPauseSignal = earlyPauseSignal;
			view.logger = logger;
			view.SetIsInitialized(true);
		}
	}
}
