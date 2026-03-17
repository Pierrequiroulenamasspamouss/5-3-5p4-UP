public class MuteMusicBusCommand : global::strange.extensions.command.impl.Command
{
	public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("MuteMusicBusCommand") as global::Kampai.Util.IKampaiLogger;

	[Inject]
	public bool mute { get; set; }

	public override void Execute()
	{
		global::Kampai.Audio.AudioSettingsModel.MusicMuted = mute;
		global::FMOD.Studio.Bus bus;
		FMOD_StudioSystem.instance.System.getBus("bus:/Non-Diegetic/u_Music", out bus);
		if (bus != null)
		{
			bus.setMute(mute);
		}
		else
		{
			logger.Log(global::Kampai.Util.KampaiLogLevel.Error, "Could not find Music bus.");
		}
	}
}
