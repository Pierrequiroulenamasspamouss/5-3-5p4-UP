namespace Kampai.Audio
{
	internal static class AudioSettingsModel
	{
		public static bool MusicMuted { get; set; }

		public static bool MuteIfBackgoundMusic { get; set; }

		static AudioSettingsModel()
		{
			MusicMuted = false;
			MuteIfBackgoundMusic = true;
		}
	}
}
