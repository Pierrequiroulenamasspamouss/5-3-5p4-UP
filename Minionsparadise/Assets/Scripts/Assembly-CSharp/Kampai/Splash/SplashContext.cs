namespace Kampai.Splash
{
	public class SplashContext : global::Kampai.Util.BaseContext
	{
		public SplashContext()
		{
		}

		public SplashContext(global::UnityEngine.MonoBehaviour view, bool autoStartup)
			: base(view, autoStartup)
		{
		}

		protected override void MapBindings()
		{
			MapInjections();
			MapCommands();
			MapMediations();
		}

		private void MapInjections()
		{
			injectionBinder.Bind<global::strange.extensions.context.api.ICrossContextCapable>().ToValue(this).ToName(global::Kampai.Splash.SplashElement.CONTEXT)
				.CrossContext();
			injectionBinder.Bind<global::Kampai.Splash.LaunchDownloadSignal>().ToSingleton().CrossContext();
			injectionBinder.Bind<global::Kampai.Common.ReconcileDLCSignal>().ToSingleton().CrossContext();
			injectionBinder.Bind<global::Kampai.Splash.DownloadInitializeSignal>().ToSingleton();
			injectionBinder.Bind<global::Kampai.Splash.DownloadProgressSignal>().ToSingleton();
			injectionBinder.Bind<global::Kampai.Splash.DLCLoadScreenModel>().ToSingleton();
			injectionBinder.Bind<global::Kampai.Main.PlayGlobalSoundFXSignal>().ToSingleton();
			injectionBinder.Bind<global::Kampai.Splash.ShowNoWiFiPanelSignal>().ToSingleton().CrossContext()
				.Weak();
		}

		private void MapCommands()
		{
			base.commandBinder.Bind<global::Kampai.Common.StartSignal>().To<global::Kampai.Splash.SplashStartCommand>();
			base.commandBinder.Bind<global::Kampai.Splash.HideSplashSignal>().To<global::Kampai.Splash.HideSplashCommand>();
			base.commandBinder.Bind<global::Kampai.Common.AppPauseSignal>().To<global::Kampai.Splash.DownloadPauseCommand>();
			base.commandBinder.Bind<global::Kampai.Common.AppResumeSignal>().To<global::Kampai.Splash.DownloadResumeCommand>();
			base.commandBinder.Bind<global::Kampai.Splash.LaunchDownloadSignal>().To<global::Kampai.Splash.LaunchDownloadCommand>();
			base.commandBinder.Bind<global::Kampai.Splash.DownloadResponseSignal>().To<global::Kampai.Splash.DownloadResponseCommand>();
			base.commandBinder.Bind<global::Kampai.Common.ReconcileDLCSignal>().To<global::Kampai.Main.ReconcileDLCCommand>();
			base.commandBinder.Bind<global::Kampai.Splash.DLCDownloadFinishedSignal>().To<global::Kampai.Splash.DLCDownloadFinishedCommand>();
		}

		private void MapMediations()
		{
			base.mediationBinder.Bind<global::Kampai.Splash.LoadInTipView>().To<global::Kampai.Splash.LoadInTipMediator>();
			base.mediationBinder.Bind<global::Kampai.Splash.LoadingBarView>().To<global::Kampai.Splash.LoadingBarMediator>();
			base.mediationBinder.Bind<global::Kampai.Splash.View.NoWiFiView>().To<global::Kampai.Download.View.NoWiFiMediator>();
			base.mediationBinder.Bind<global::Kampai.Splash.LogoPanelView>().To<global::Kampai.Splash.LogoPanelMediator>();
		}
	}
}
