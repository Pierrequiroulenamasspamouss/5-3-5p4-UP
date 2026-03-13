public class LoadDevicePrefsCommand : global::strange.extensions.command.impl.Command
{
	public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("LoadDevicePrefsCommand") as global::Kampai.Util.IKampaiLogger;

	[Inject]
	public global::Kampai.Game.IDevicePrefsService deviceService { get; set; }

	[Inject]
	public ILocalPersistanceService localPersistService { get; set; }

	public override void Execute()
	{
		string text = localPersistService.GetData("DevicePrefs");
		if (text.Length != 0)
		{
			deviceService.Deserialize(text);
		}
		else
		{
			logger.Log(global::Kampai.Util.KampaiLogLevel.Info, "Couldn't load device prefs. This could be because it's first time playing the game.");
		}
	}
}
