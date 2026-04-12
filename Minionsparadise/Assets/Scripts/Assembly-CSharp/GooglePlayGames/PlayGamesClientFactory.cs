namespace GooglePlayGames
{
	internal class PlayGamesClientFactory
	{
		internal static global::GooglePlayGames.BasicApi.IPlayGamesClient GetPlatformPlayGamesClient(global::GooglePlayGames.BasicApi.PlayGamesClientConfiguration config)
		{
			if (global::UnityEngine.Application.isEditor)
			{
				global::GooglePlayGames.OurUtils.Logger.d("Creating IPlayGamesClient in editor, using DummyClient.");
				return new global::GooglePlayGames.BasicApi.DummyClient();
			}
			global::GooglePlayGames.OurUtils.Logger.d("Creating Android IPlayGamesClient Client");
			return new global::GooglePlayGames.Native.NativeClient(config, new global::GooglePlayGames.Android.AndroidClient());
		}
	}
}
