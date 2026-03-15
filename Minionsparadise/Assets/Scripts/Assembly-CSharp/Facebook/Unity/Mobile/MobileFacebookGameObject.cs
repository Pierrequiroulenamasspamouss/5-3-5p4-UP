namespace Facebook.Unity.Mobile
{
	internal abstract class MobileFacebookGameObject : global::Facebook.Unity.FacebookGameObject, global::Facebook.Unity.IFacebookCallbackHandler, global::Facebook.Unity.Mobile.IMobileFacebookCallbackHandler
	{
		private global::Facebook.Unity.Mobile.IMobileFacebookImplementation MobileFacebook
		{
			get
			{
				return (global::Facebook.Unity.Mobile.IMobileFacebookImplementation)base.Facebook;
			}
		}

		public void OnAppInviteComplete(string message)
		{
			MobileFacebook.OnAppInviteComplete(new global::Facebook.Unity.ResultContainer(message));
		}

		public void OnFetchDeferredAppLinkComplete(string message)
		{
			MobileFacebook.OnFetchDeferredAppLinkComplete(new global::Facebook.Unity.ResultContainer(message));
		}

		public void OnRefreshCurrentAccessTokenComplete(string message)
		{
			MobileFacebook.OnRefreshCurrentAccessTokenComplete(new global::Facebook.Unity.ResultContainer(message));
		}
	}
}
