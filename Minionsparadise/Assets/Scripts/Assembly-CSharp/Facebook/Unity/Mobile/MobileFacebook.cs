namespace Discord.Unity.Mobile
{
	internal abstract class MobileFacebook : global::Discord.Unity.FacebookBase, global::Discord.Unity.IFacebook, global::Discord.Unity.IFacebookResultHandler, global::Discord.Unity.Mobile.IMobileFacebook, global::Discord.Unity.Mobile.IMobileFacebookImplementation, global::Discord.Unity.Mobile.IMobileFacebookResultHandler
	{
		private const string CallbackIdKey = "callback_id";

		private global::Discord.Unity.ShareDialogMode shareDialogMode;

		public global::Discord.Unity.ShareDialogMode ShareDialogMode
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

		protected MobileFacebook(global::Discord.Unity.CallbackManager callbackManager)
			: base(callbackManager)
		{
		}

		public abstract void AppInvite(global::System.Uri appLinkUrl, global::System.Uri previewImageUrl, global::Discord.Unity.FacebookDelegate<global::Discord.Unity.IAppInviteResult> callback);

		public abstract void FetchDeferredAppLink(global::Discord.Unity.FacebookDelegate<global::Discord.Unity.IAppLinkResult> callback);

		public abstract void RefreshCurrentAccessToken(global::Discord.Unity.FacebookDelegate<global::Discord.Unity.IAccessTokenRefreshResult> callback);

		public override void OnLoginComplete(global::Discord.Unity.ResultContainer resultContainer)
		{
			global::Discord.Unity.LoginResult result = new global::Discord.Unity.LoginResult(resultContainer);
			OnAuthResponse(result);
		}

		public override void OnGetAppLinkComplete(global::Discord.Unity.ResultContainer resultContainer)
		{
			global::Discord.Unity.AppLinkResult result = new global::Discord.Unity.AppLinkResult(resultContainer);
			base.CallbackManager.OnFacebookResponse(result);
		}

		public override void OnGroupCreateComplete(global::Discord.Unity.ResultContainer resultContainer)
		{
			global::Discord.Unity.GroupCreateResult result = new global::Discord.Unity.GroupCreateResult(resultContainer);
			base.CallbackManager.OnFacebookResponse(result);
		}

		public override void OnGroupJoinComplete(global::Discord.Unity.ResultContainer resultContainer)
		{
			global::Discord.Unity.GroupJoinResult result = new global::Discord.Unity.GroupJoinResult(resultContainer);
			base.CallbackManager.OnFacebookResponse(result);
		}

		public override void OnAppRequestsComplete(global::Discord.Unity.ResultContainer resultContainer)
		{
			global::Discord.Unity.AppRequestResult result = new global::Discord.Unity.AppRequestResult(resultContainer);
			base.CallbackManager.OnFacebookResponse(result);
		}

		public void OnAppInviteComplete(global::Discord.Unity.ResultContainer resultContainer)
		{
			global::Discord.Unity.AppInviteResult result = new global::Discord.Unity.AppInviteResult(resultContainer);
			base.CallbackManager.OnFacebookResponse(result);
		}

		public void OnFetchDeferredAppLinkComplete(global::Discord.Unity.ResultContainer resultContainer)
		{
			global::Discord.Unity.AppLinkResult result = new global::Discord.Unity.AppLinkResult(resultContainer);
			base.CallbackManager.OnFacebookResponse(result);
		}

		public override void OnShareLinkComplete(global::Discord.Unity.ResultContainer resultContainer)
		{
			global::Discord.Unity.ShareResult result = new global::Discord.Unity.ShareResult(resultContainer);
			base.CallbackManager.OnFacebookResponse(result);
		}

		public void OnRefreshCurrentAccessTokenComplete(global::Discord.Unity.ResultContainer resultContainer)
		{
			global::Discord.Unity.AccessTokenRefreshResult accessTokenRefreshResult = new global::Discord.Unity.AccessTokenRefreshResult(resultContainer);
			if (accessTokenRefreshResult.AccessToken != null)
			{
				global::Discord.Unity.AccessToken.CurrentAccessToken = accessTokenRefreshResult.AccessToken;
			}
			base.CallbackManager.OnFacebookResponse(accessTokenRefreshResult);
		}

		protected abstract void SetShareDialogMode(global::Discord.Unity.ShareDialogMode mode);

		private static global::System.Collections.Generic.IDictionary<string, object> DeserializeMessage(string message)
		{
			return (global::System.Collections.Generic.Dictionary<string, object>)global::Discord.MiniJSON.Json.Deserialize(message);
		}

		private static string SerializeDictionary(global::System.Collections.Generic.IDictionary<string, object> dict)
		{
			return global::Discord.MiniJSON.Json.Serialize(dict);
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
