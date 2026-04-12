namespace Discord.Unity.Mobile
{
	internal interface IMobileFacebook : global::Discord.Unity.IFacebook
	{
		global::Discord.Unity.ShareDialogMode ShareDialogMode { get; set; }

		void AppInvite(global::System.Uri appLinkUrl, global::System.Uri previewImageUrl, global::Discord.Unity.FacebookDelegate<global::Discord.Unity.IAppInviteResult> callback);

		void FetchDeferredAppLink(global::Discord.Unity.FacebookDelegate<global::Discord.Unity.IAppLinkResult> callback);

		void RefreshCurrentAccessToken(global::Discord.Unity.FacebookDelegate<global::Discord.Unity.IAccessTokenRefreshResult> callback);
	}
}
