namespace Facebook.Unity.Mobile
{
	internal abstract class MobileFacebook : global::Facebook.Unity.FacebookBase, global::Facebook.Unity.IFacebook, global::Facebook.Unity.IFacebookResultHandler, global::Facebook.Unity.Mobile.IMobileFacebook, global::Facebook.Unity.Mobile.IMobileFacebookImplementation, global::Facebook.Unity.Mobile.IMobileFacebookResultHandler
	{
		private const string CallbackIdKey = "callback_id";

		private global::Facebook.Unity.ShareDialogMode shareDialogMode;

		public global::Facebook.Unity.ShareDialogMode ShareDialogMode
		{
			get
			{
				return shareDialogMode;
			}
			set
			{
				shareDialogMode = value;
				SetShareDialogMode(shareDialogMode);
			}
		}

		protected MobileFacebook(global::Facebook.Unity.CallbackManager callbackManager)
			: base(callbackManager)
		{
		}

		public abstract void AppInvite(global::System.Uri appLinkUrl, global::System.Uri previewImageUrl, global::Facebook.Unity.FacebookDelegate<global::Facebook.Unity.IAppInviteResult> callback);

		public abstract void FetchDeferredAppLink(global::Facebook.Unity.FacebookDelegate<global::Facebook.Unity.IAppLinkResult> callback);

		public abstract void RefreshCurrentAccessToken(global::Facebook.Unity.FacebookDelegate<global::Facebook.Unity.IAccessTokenRefreshResult> callback);

		public override void OnLoginComplete(global::Facebook.Unity.ResultContainer resultContainer)
		{
			global::Facebook.Unity.LoginResult result = new global::Facebook.Unity.LoginResult(resultContainer);
			OnAuthResponse(result);
		}

		public override void OnGetAppLinkComplete(global::Facebook.Unity.ResultContainer resultContainer)
		{
			global::Facebook.Unity.AppLinkResult result = new global::Facebook.Unity.AppLinkResult(resultContainer);
			base.CallbackManager.OnFacebookResponse(result);
		}

		public override void OnGroupCreateComplete(global::Facebook.Unity.ResultContainer resultContainer)
		{
			global::Facebook.Unity.GroupCreateResult result = new global::Facebook.Unity.GroupCreateResult(resultContainer);
			base.CallbackManager.OnFacebookResponse(result);
		}

		public override void OnGroupJoinComplete(global::Facebook.Unity.ResultContainer resultContainer)
		{
			global::Facebook.Unity.GroupJoinResult result = new global::Facebook.Unity.GroupJoinResult(resultContainer);
			base.CallbackManager.OnFacebookResponse(result);
		}

		public override void OnAppRequestsComplete(global::Facebook.Unity.ResultContainer resultContainer)
		{
			global::Facebook.Unity.AppRequestResult result = new global::Facebook.Unity.AppRequestResult(resultContainer);
			base.CallbackManager.OnFacebookResponse(result);
		}

		public void OnAppInviteComplete(global::Facebook.Unity.ResultContainer resultContainer)
		{
			global::Facebook.Unity.AppInviteResult result = new global::Facebook.Unity.AppInviteResult(resultContainer);
			base.CallbackManager.OnFacebookResponse(result);
		}

		public void OnFetchDeferredAppLinkComplete(global::Facebook.Unity.ResultContainer resultContainer)
		{
			global::Facebook.Unity.AppLinkResult result = new global::Facebook.Unity.AppLinkResult(resultContainer);
			base.CallbackManager.OnFacebookResponse(result);
		}

		public override void OnShareLinkComplete(global::Facebook.Unity.ResultContainer resultContainer)
		{
			global::Facebook.Unity.ShareResult result = new global::Facebook.Unity.ShareResult(resultContainer);
			base.CallbackManager.OnFacebookResponse(result);
		}

		public void OnRefreshCurrentAccessTokenComplete(global::Facebook.Unity.ResultContainer resultContainer)
		{
			global::Facebook.Unity.AccessTokenRefreshResult accessTokenRefreshResult = new global::Facebook.Unity.AccessTokenRefreshResult(resultContainer);
			if (accessTokenRefreshResult.AccessToken != null)
			{
				global::Facebook.Unity.AccessToken.CurrentAccessToken = accessTokenRefreshResult.AccessToken;
			}
			base.CallbackManager.OnFacebookResponse(accessTokenRefreshResult);
		}

		protected abstract void SetShareDialogMode(global::Facebook.Unity.ShareDialogMode mode);

		private static global::System.Collections.Generic.IDictionary<string, object> DeserializeMessage(string message)
		{
			return (global::System.Collections.Generic.Dictionary<string, object>)global::Facebook.MiniJSON.Json.Deserialize(message);
		}

		private static string SerializeDictionary(global::System.Collections.Generic.IDictionary<string, object> dict)
		{
			return global::Facebook.MiniJSON.Json.Serialize(dict);
		}

		private static bool TryGetCallbackId(global::System.Collections.Generic.IDictionary<string, object> result, out string callbackId)
		{
			callbackId = null;
			object value;
			if (result.TryGetValue("callback_id", out value))
			{
				callbackId = value as string;
				return true;
			}
			return false;
		}

		private static bool TryGetError(global::System.Collections.Generic.IDictionary<string, object> result, out string errorMessage)
		{
			errorMessage = null;
			object value;
			if (result.TryGetValue("error", out value))
			{
				errorMessage = value as string;
				return true;
			}
			return false;
		}
	}
}
