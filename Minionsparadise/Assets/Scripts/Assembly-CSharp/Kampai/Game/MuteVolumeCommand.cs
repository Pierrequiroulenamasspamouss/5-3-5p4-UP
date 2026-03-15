namespace Kampai.Game
{
	public class MuteVolumeCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("MuteVolumeCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Kampai.Game.MuteMusicBusSignal muteMusicBusSignal { get; set; }

		public override void Execute()
		{
			CheckForOtherAudio();
		}

		private void MuteAudio(bool mute)
		{
			logger.Error("Music Muted: {0}", mute);
			muteMusicBusSignal.Dispatch(mute);
		}

		private void CheckForOtherAudio()
		{
			if (global::Kampai.Audio.AudioSettingsModel.MuteIfBackgoundMusic && global::Kampai.Util.Native.IsUserMusicPlaying() && !global::Kampai.Audio.AudioSettingsModel.MusicMuted)
			{
				muteMusicBusSignal.Dispatch(true);
			}
		}
	}
}
