namespace Kampai.Common
{
	public class CheckAvailableStorageCommand : global::strange.extensions.command.impl.Command
	{
		private global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("CheckAvailableStorageCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public string path { get; set; }

		[Inject]
		public ulong requiredStorage { get; set; }

		[Inject]
		public global::System.Action availableCallback { get; set; }

		[Inject]
		public global::Kampai.Main.ILocalizationService localizationService { get; set; }

		public override void Execute()
		{
			ulong availableStorage = global::Kampai.Util.Native.GetAvailableStorage(path);
			if (availableStorage >= requiredStorage)
			{
				availableCallback();
				return;
			}
			logger.FatalNoThrow(global::Kampai.Util.FatalCode.EX_INSUFFICIENT_STORAGE, localizationService.GetString("InsufficientStorageMessage"), (ulong)((float)(requiredStorage - availableStorage) / 1024f / 1024f + 0.5f));
		}
	}
}
