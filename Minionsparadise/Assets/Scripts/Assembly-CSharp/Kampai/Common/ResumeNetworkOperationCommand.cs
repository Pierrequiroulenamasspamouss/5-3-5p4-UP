namespace Kampai.Common
{
	public class ResumeNetworkOperationCommand : global::strange.extensions.command.impl.Command
	{
		private global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("ResumeNetworkOperationCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Kampai.Main.ReloadGameSignal reloadGameSignal { get; set; }

		[Inject]
		public global::Kampai.Splash.DLCModel dlcModel { get; set; }

		public override void Execute()
		{
			global::Kampai.Main.LoadStateType loadStateType = global::Kampai.Main.LoadState.Get();
			logger.Info("ResumeNetworkOperationCommand: {0}", loadStateType.ToString());
			switch (loadStateType)
			{
			case global::Kampai.Main.LoadStateType.BOOTING:
				reloadGameSignal.Dispatch();
				break;
			case global::Kampai.Main.LoadStateType.INTRO:
				logger.Info("IntroPlaying");
				break;
			case global::Kampai.Main.LoadStateType.DLC:
				if (dlcModel.PendingRequests == null || dlcModel.RunningRequests == null)
				{
					reloadGameSignal.Dispatch();
				}
				break;
			default:
				logger.Info("GameRunning");
				break;
			}
		}
	}
}
