namespace Kampai.Splash
{
	public class HideSplashCommand : global::strange.extensions.command.impl.Command
	{
		private global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("HideSplashCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject(global::strange.extensions.context.api.ContextKeys.CONTEXT_VIEW)]
		public global::UnityEngine.GameObject contextView { get; set; }

		[Inject]
		public global::Kampai.Main.AppStartCompleteSignal appStartCompleteSignal { get; set; }

		[Inject]
		public global::Kampai.Game.ITimeService timeService { get; set; }

		public override void Execute()
		{
			logger.EventStart("HideSplashCommand.Execute");
			global::Kampai.Util.Native.LogInfo(string.Format("HideSplash, realtimeSinceStartup: {0}", timeService.RealtimeSinceStartup()));
			for (int i = 0; i < contextView.transform.childCount; i++)
			{
				global::UnityEngine.Object.Destroy(contextView.transform.GetChild(i).gameObject);
			}
			logger.EventStop("HideSplashCommand.Execute");
			global::Kampai.Util.TimeProfiler.EndSection("main");
			appStartCompleteSignal.Dispatch();
			global::Kampai.Util.TimeProfiler.StopMonoProfiler();
		}
	}
}
