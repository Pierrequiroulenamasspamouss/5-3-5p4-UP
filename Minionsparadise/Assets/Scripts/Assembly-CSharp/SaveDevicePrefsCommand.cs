public class SaveDevicePrefsCommand : global::strange.extensions.command.impl.Command
{
	public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("SaveDevicePrefsCommand") as global::Kampai.Util.IKampaiLogger;

	[Inject]
	public global::Kampai.Game.IDevicePrefsService prefsService { get; set; }

	[Inject]
	public ILocalPersistanceService persistService { get; set; }

	public override void Execute()
	{
		string value = prefsService.Serialize();
		if (string.IsNullOrEmpty(value))
		{
			logger.Log(global::Kampai.Util.KampaiLogLevel.Debug, "Problem serializing device prefs");
		}
		else
		{
			persistService.PutData("DevicePrefs", value);
		}
	}
}
