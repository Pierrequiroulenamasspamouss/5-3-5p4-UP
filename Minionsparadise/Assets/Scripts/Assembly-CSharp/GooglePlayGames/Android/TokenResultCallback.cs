namespace GooglePlayGames.Android
{
	internal class TokenResultCallback : global::Com.Google.Android.Gms.Common.Api.ResultCallbackProxy<global::GooglePlayGames.Android.TokenResult>
	{
		private global::System.Action<int, string, string, string> callback;

		public TokenResultCallback(global::System.Action<int, string, string, string> callback)
		{
			this.callback = callback;
		}

		public override void OnResult(global::GooglePlayGames.Android.TokenResult arg_Result_1)
		{
			callback(arg_Result_1.getStatus().getStatusCode(), arg_Result_1.getAccessToken(), arg_Result_1.getIdToken(), arg_Result_1.getEmail());
		}
	}
}
