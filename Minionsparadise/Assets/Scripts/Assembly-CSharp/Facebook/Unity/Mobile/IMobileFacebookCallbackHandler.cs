namespace Facebook.Unity.Mobile
{
	internal interface IMobileFacebookCallbackHandler : global::Facebook.Unity.IFacebookCallbackHandler
	{
		void OnAppInviteComplete(string message);

		void OnFetchDeferredAppLinkComplete(string message);

		void OnRefreshCurrentAccessTokenComplete(string message);
	}
}
