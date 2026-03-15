namespace Facebook.Unity.Mobile
{
	internal interface IMobileFacebookResultHandler : global::Facebook.Unity.IFacebookResultHandler
	{
		void OnAppInviteComplete(global::Facebook.Unity.ResultContainer resultContainer);

		void OnFetchDeferredAppLinkComplete(global::Facebook.Unity.ResultContainer resultContainer);

		void OnRefreshCurrentAccessTokenComplete(global::Facebook.Unity.ResultContainer resultContainer);
	}
}
