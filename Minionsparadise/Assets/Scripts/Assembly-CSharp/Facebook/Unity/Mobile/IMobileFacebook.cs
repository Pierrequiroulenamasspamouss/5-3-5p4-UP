namespace Facebook.Unity.Mobile
{
	internal interface IMobileFacebook : global::Facebook.Unity.IFacebook
	{
		global::Facebook.Unity.ShareDialogMode ShareDialogMode { get; set; }

		void AppInvite(global::System.Uri appLinkUrl, global::System.Uri previewImageUrl, global::Facebook.Unity.FacebookDelegate<global::Facebook.Unity.IAppInviteResult> callback);

		void FetchDeferredAppLink(global::Facebook.Unity.FacebookDelegate<global::Facebook.Unity.IAppLinkResult> callback);

		void RefreshCurrentAccessToken(global::Facebook.Unity.FacebookDelegate<global::Facebook.Unity.IAccessTokenRefreshResult> callback);
	}
}
