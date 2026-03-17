namespace GooglePlayGames
{
	internal interface TokenClient
	{
		string GetEmail();

		void GetEmail(global::System.Action<global::GooglePlayGames.BasicApi.CommonStatusCodes, string> callback);

		string GetAccessToken();

		void GetIdToken(string serverClientId, global::System.Action<string> idTokenCallback);

		void SetRationale(string rationale);
	}
}
