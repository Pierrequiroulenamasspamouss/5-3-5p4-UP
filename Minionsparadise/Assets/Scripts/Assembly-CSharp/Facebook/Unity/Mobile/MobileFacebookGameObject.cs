namespace Discord.Unity.Mobile
{
	internal abstract class MobileFacebookGameObject : global::Discord.Unity.FacebookGameObject, global::Discord.Unity.IFacebookCallbackHandler, global::Discord.Unity.Mobile.IMobileFacebookCallbackHandler
	{
		private global::Discord.Unity.Mobile.IMobileFacebookImplementation MobileFacebook
		{
			get
			{
				return (global::Discord.Unity.Mobile.IMobileFacebookImplementation)base.Discord;
			}
		}

		public void OnAppInviteComplete(string message)
		{
			MobileFacebook.OnAppInviteComplete(new global::Discord.Unity.ResultContainer(message));
		}

		public void OnFetchDeferredAppLinkComplete(string message)
		{
			MobileFacebook.OnFetchDeferredAppLinkComplete(new global::Discord.Unity.ResultContainer(message));
		}

		public void OnRefreshCurrentAccessTokenComplete(string message)
		{
			MobileFacebook.OnRefreshCurrentAccessTokenComplete(new global::Discord.Unity.ResultContainer(message));
		}
	}
}
