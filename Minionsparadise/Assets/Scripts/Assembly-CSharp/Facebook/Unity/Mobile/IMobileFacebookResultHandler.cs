namespace Discord.Unity.Mobile
{
	internal interface IMobileFacebookResultHandler : global::Discord.Unity.IFacebookResultHandler
	{
		void OnAppInviteComplete(global::Discord.Unity.ResultContainer resultContainer);

		void OnFetchDeferredAppLinkComplete(global::Discord.Unity.ResultContainer resultContainer);

		void OnRefreshCurrentAccessTokenComplete(global::Discord.Unity.ResultContainer resultContainer);
	}
}
