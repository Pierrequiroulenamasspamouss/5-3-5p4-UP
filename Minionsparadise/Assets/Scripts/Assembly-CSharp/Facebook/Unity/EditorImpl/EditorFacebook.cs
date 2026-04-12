namespace Discord.Unity.Editor
{
	internal class EditorFacebook : global::Discord.Unity.FacebookBase, global::Discord.Unity.Canvas.ICanvasFacebook, global::Discord.Unity.Canvas.ICanvasFacebookImplementation, global::Discord.Unity.Canvas.ICanvasFacebookResultHandler, global::Discord.Unity.IFacebook, global::Discord.Unity.IFacebookResultHandler, global::Discord.Unity.IPayFacebook, global::Discord.Unity.Mobile.IMobileFacebook, global::Discord.Unity.Mobile.IMobileFacebookImplementation, global::Discord.Unity.Mobile.IMobileFacebookResultHandler
	{
		private const string WarningMessage = "You are using the discord SDK in the Unity Editor. Behavior may not be the same as when used on iOS, Android, or Web.";

		private const string AccessTokenKey = "com.discord.unity.editor.accesstoken";

		private global::Discord.Unity.Editor.IEditorWrapper editorWrapper;

		public override bool LimitEventUsage { get; set; }

		public global::Discord.Unity.ShareDialogMode ShareDialogMode { get; set; }

		public override string SDKName
		{
			get
			{
				return "FBUnityEditorSDK";
			}
		}

		public override string SDKVersion
		{
			get
			{
				return global::Discord.Unity.FacebookSdkVersion.Build;
			}
		}

		private static global::Discord.Unity.IFacebookCallbackHandler EditorGameObject
		{
			get
			{
				return global::Discord.Unity.ComponentFactory.GetComponent<global::Discord.Unity.Editor.EditorFacebookGameObject>();
			}
		}

		public EditorFacebook(global::Discord.Unity.Editor.IEditorWrapper wrapper, global::Discord.Unity.CallbackManager callbackManager)
			: base(callbackManager)
		{
			editorWrapper = wrapper;
		}

		public EditorFacebook()
			: this(new global::Discord.Unity.Editor.EditorWrapper(EditorGameObject), new global::Discord.Unity.CallbackManager())
		{
		}

		public override void Init(global::Discord.Unity.HideUnityDelegate hideUnityDelegate, global::Discord.Unity.InitDelegate onInitComplete)
		{
			global::Discord.Unity.FacebookLogger.Warn("You are using the discord SDK in the Unity Editor. Behavior may not be the same as when used on iOS, Android, or Web.");
			base.Init(hideUnityDelegate, onInitComplete);
			editorWrapper.Init();
		}

		public override void LogInWithReadPermissions(global::System.Collections.Generic.IEnumerable<string> permissions, global::Discord.Unity.FacebookDelegate<global::Discord.Unity.ILoginResult> callback)
		{
			LogInWithPublishPermissions(permissions, callback);
		}

		public override void LogInWithPublishPermissions(global::System.Collections.Generic.IEnumerable<string> permissions, global::Discord.Unity.FacebookDelegate<global::Discord.Unity.ILoginResult> callback)
		{
			editorWrapper.ShowLoginMockDialog(OnLoginComplete, base.CallbackManager.AddFacebookDelegate(callback), permissions.ToCommaSeparateList());
		}

		public override void AppRequest(string message, global::Discord.Unity.OGActionType? actionType, string objectId, global::System.Collections.Generic.IEnumerable<string> to, global::System.Collections.Generic.IEnumerable<object> filters, global::System.Collections.Generic.IEnumerable<string> excludeIds, int? maxRecipients, string data, string title, global::Discord.Unity.FacebookDelegate<global::Discord.Unity.IAppRequestResult> callback)
		{
			editorWrapper.ShowAppRequestMockDialog(OnAppRequestsComplete, base.CallbackManager.AddFacebookDelegate(callback));
		}

		public override void ShareLink(global::System.Uri contentURL, string contentTitle, string contentDescription, global::System.Uri photoURL, global::Discord.Unity.FacebookDelegate<global::Discord.Unity.IShareResult> callback)
		{
			editorWrapper.ShowMockShareDialog(OnShareLinkComplete, "ShareLink", base.CallbackManager.AddFacebookDelegate(callback));
		}

		public override void FeedShare(string toId, global::System.Uri link, string linkName, string linkCaption, string linkDescription, global::System.Uri picture, string mediaSource, global::Discord.Unity.FacebookDelegate<global::Discord.Unity.IShareResult> callback)
		{
			editorWrapper.ShowMockShareDialog(OnShareLinkComplete, "FeedShare", base.CallbackManager.AddFacebookDelegate(callback));
		}

		public override void GameGroupCreate(string name, string description, string privacy, global::Discord.Unity.FacebookDelegate<global::Discord.Unity.IGroupCreateResult> callback)
		{
			editorWrapper.ShowGameGroupCreateMockDialog(OnGroupCreateComplete, base.CallbackManager.AddFacebookDelegate(callback));
		}

		public override void GameGroupJoin(string id, global::Discord.Unity.FacebookDelegate<global::Discord.Unity.IGroupJoinResult> callback)
		{
			editorWrapper.ShowGameGroupJoinMockDialog(OnGroupJoinComplete, base.CallbackManager.AddFacebookDelegate(callback));
		}

		public override void ActivateApp(string appId)
		{
			global::Discord.Unity.FacebookLogger.Info("This only needs to be called for iOS or Android.");
		}

		public override void GetAppLink(global::Discord.Unity.FacebookDelegate<global::Discord.Unity.IAppLinkResult> callback)
		{
			global::System.Collections.Generic.Dictionary<string, object> dictionary = new global::System.Collections.Generic.Dictionary<string, object>();
			dictionary["url"] = "mockurl://testing.url";
			dictionary["callback_id"] = base.CallbackManager.AddFacebookDelegate(callback);
			OnGetAppLinkComplete(new global::Discord.Unity.ResultContainer(dictionary));
		}

		public override void AppEventsLogEvent(string logEvent, float? valueToSum, global::System.Collections.Generic.Dictionary<string, object> parameters)
		{
			global::Discord.Unity.FacebookLogger.Log("Pew! Pretending to send this off.  Doesn't actually work in the editor");
		}

		public override void AppEventsLogPurchase(float logPurchase, string currency, global::System.Collections.Generic.Dictionary<string, object> parameters)
		{
			global::Discord.Unity.FacebookLogger.Log("Pew! Pretending to send this off.  Doesn't actually work in the editor");
		}

		public void AppInvite(global::System.Uri appLinkUrl, global::System.Uri previewImageUrl, global::Discord.Unity.FacebookDelegate<global::Discord.Unity.IAppInviteResult> callback)
		{
			editorWrapper.ShowAppInviteMockDialog(OnAppInviteComplete, base.CallbackManager.AddFacebookDelegate(callback));
		}

		public void FetchDeferredAppLink(global::Discord.Unity.FacebookDelegate<global::Discord.Unity.IAppLinkResult> callback)
		{
			global::System.Collections.Generic.Dictionary<string, object> dictionary = new global::System.Collections.Generic.Dictionary<string, object>();
			dictionary["url"] = "mockurl://testing.url";
			dictionary["ref"] = "mock ref";
			dictionary["extras"] = new global::System.Collections.Generic.Dictionary<string, object> { { "mock extra key", "mock extra value" } };
			dictionary["target_url"] = "mocktargeturl://mocktarget.url";
			dictionary["callback_id"] = base.CallbackManager.AddFacebookDelegate(callback);
			OnFetchDeferredAppLinkComplete(new global::Discord.Unity.ResultContainer(dictionary));
		}

		public void Pay(string product, string action, int quantity, int? quantityMin, int? quantityMax, string requestId, string pricepointId, string testCurrency, global::Discord.Unity.FacebookDelegate<global::Discord.Unity.IPayResult> callback)
		{
			editorWrapper.ShowPayMockDialog(OnPayComplete, base.CallbackManager.AddFacebookDelegate(callback));
		}

		public void RefreshCurrentAccessToken(global::Discord.Unity.FacebookDelegate<global::Discord.Unity.IAccessTokenRefreshResult> callback)
		{
			if (callback != null)
			{
				global::System.Collections.Generic.Dictionary<string, object> dictionary = new global::System.Collections.Generic.Dictionary<string, object>();
				dictionary.Add("callback_id", base.CallbackManager.AddFacebookDelegate(callback));
				global::System.Collections.Generic.Dictionary<string, object> dictionary2 = dictionary;
				if (global::Discord.Unity.AccessToken.CurrentAccessToken == null)
				{
					dictionary2["error"] = "No current access token";
				}
				else
				{
					global::System.Collections.Generic.IDictionary<string, object> source = (global::System.Collections.Generic.IDictionary<string, object>)global::Discord.MiniJSON.Json.Deserialize(global::Discord.Unity.AccessToken.CurrentAccessToken.ToJson());
					dictionary2.AddAllKVPFrom(source);
				}
				OnRefreshCurrentAccessTokenComplete(new global::Discord.Unity.ResultContainer(dictionary2));
			}
		}

		public override void OnAppRequestsComplete(global::Discord.Unity.ResultContainer resultContainer)
		{
			global::Discord.Unity.AppRequestResult result = new global::Discord.Unity.AppRequestResult(resultContainer);
			base.CallbackManager.OnFacebookResponse(result);
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

		public override void OnLoginComplete(global::Discord.Unity.ResultContainer resultContainer)
		{
			global::Discord.Unity.LoginResult result = new global::Discord.Unity.LoginResult(resultContainer);
			OnAuthResponse(result);
		}

		public override void OnShareLinkComplete(global::Discord.Unity.ResultContainer resultContainer)
		{
			global::Discord.Unity.ShareResult result = new global::Discord.Unity.ShareResult(resultContainer);
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

		public void OnPayComplete(global::Discord.Unity.ResultContainer resultContainer)
		{
			global::Discord.Unity.PayResult result = new global::Discord.Unity.PayResult(resultContainer);
			base.CallbackManager.OnFacebookResponse(result);
		}

		public void OnRefreshCurrentAccessTokenComplete(global::Discord.Unity.ResultContainer resultContainer)
		{
			global::Discord.Unity.AccessTokenRefreshResult result = new global::Discord.Unity.AccessTokenRefreshResult(resultContainer);
			base.CallbackManager.OnFacebookResponse(result);
		}

		public void OnFacebookAuthResponseChange(global::Discord.Unity.ResultContainer resultContainer)
		{
			throw new global::System.NotSupportedException();
		}

		public void OnUrlResponse(string message)
		{
			throw new global::System.NotSupportedException();
		}
	}
}
