namespace GooglePlayGames
{
	internal interface IClientImpl
	{
		global::GooglePlayGames.Native.PInvoke.PlatformConfiguration CreatePlatformConfiguration();

		global::GooglePlayGames.TokenClient CreateTokenClient(string playerId, bool reset);

		void GetPlayerStats(global::System.IntPtr apiClientPtr, global::System.Action<global::GooglePlayGames.BasicApi.CommonStatusCodes, global::GooglePlayGames.BasicApi.PlayerStats> callback);
	}
}
