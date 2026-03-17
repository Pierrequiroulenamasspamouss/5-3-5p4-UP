namespace Kampai.Main
{
	public class ReloadGameCommand : global::strange.extensions.command.impl.Command
	{
		private global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("ReloadGameCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Kampai.Common.ReInitializeGameSignal reInitializeGameSignal { get; set; }

		public override void Execute()
		{
			logger.Warning("Reloading Game");
			reInitializeGameSignal.Dispatch(string.Empty);
		}
	}
}
