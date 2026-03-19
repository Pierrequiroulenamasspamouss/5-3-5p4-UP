namespace Discord.Unity.Mobile
{
	internal interface IMobileFacebookCallbackHandler : global::Discord.Unity.IFacebookCallbackHandler
	{
		void OnAppInviteComplete(string message);

		void OnFetchDeferredAppLinkComplete(string message);

		void OnRefreshCurrentAccessTokenComplete(string message);
	}
}
