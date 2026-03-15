namespace Facebook.Unity.Editor
{
	internal class EditorFacebook : global::Facebook.Unity.FacebookBase, global::Facebook.Unity.Canvas.ICanvasFacebook, global::Facebook.Unity.Canvas.ICanvasFacebookImplementation, global::Facebook.Unity.Canvas.ICanvasFacebookResultHandler, global::Facebook.Unity.IFacebook, global::Facebook.Unity.IFacebookResultHandler, global::Facebook.Unity.IPayFacebook, global::Facebook.Unity.Mobile.IMobileFacebook, global::Facebook.Unity.Mobile.IMobileFacebookImplementation, global::Facebook.Unity.Mobile.IMobileFacebookResultHandler
	{
		private const string WarningMessage = "You are using the facebook SDK in the Unity Editor. Behavior may not be the same as when used on iOS, Android, or Web.";

		private const string AccessTokenKey = "com.facebook.unity.editor.accesstoken";

		private global::Facebook.Unity.Editor.IEditorWrapper editorWrapper;

		public override bool LimitEventUsage { get; set; }

		public global::Facebook.Unity.ShareDialogMode ShareDialogMode { get; set; }

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
				return global::Facebook.Unity.FacebookSdkVersion.Build;
			}
		}

		private static global::Facebook.Unity.IFacebookCallbackHandler EditorGameObject
		{
			get
			{
				return global::Facebook.Unity.ComponentFactory.GetComponent<global::Facebook.Unity.Editor.EditorFacebookGameObject>();
			}
		}

		public EditorFacebook(global::Facebook.Unity.Editor.IEditorWrapper wrapper, global::Facebook.Unity.CallbackManager callbackManager)
			: base(callbackManager)
		{
			editorWrapper = wrapper;
		}

		public EditorFacebook()
			: this(new global::Facebook.Unity.Editor.EditorWrapper(EditorGameObject), new global::Facebook.Unity.CallbackManager())
		{
		}

		public override void Init(global::Facebook.Unity.HideUnityDelegate hideUnityDelegate, global::Facebook.Unity.InitDelegate onInitComplete)
		{
			global::Facebook.Unity.FacebookLogger.Warn("You are using the facebook SDK in the Unity Editor. Behavior may not be the same as when used on iOS, Android, or Web.");
			base.Init(hideUnityDelegate, onInitComplete);
			editorWrapper.Init();
		}

		public override void LogInWithReadPermissions(global::System.Collections.Generic.IEnumerable<string> permissions, global::Facebook.Unity.FacebookDelegate<global::Facebook.Unity.ILoginResult> callback)
		{
			LogInWithPublishPermissions(permissions, callback);
		}

		public override void LogInWithPublishPermissions(global::System.Collections.Generic.IEnumerable<string> permissions, global::Facebook.Unity.FacebookDelegate<global::Facebook.Unity.ILoginResult> callback)
		{
			editorWrapper.ShowLoginMockDialog(OnLoginComplete, base.CallbackManager.AddFacebookDelegate(callback), permissions.ToCommaSeparateList());
		}

		public override void AppRequest(string message, global::Facebook.Unity.OGActionType? actionType, string objectId, global::System.Collections.Generic.IEnumerable<string> to, global::System.Collections.Generic.IEnumerable<object> filters, global::System.Collections.Generic.IEnumerable<string> excludeIds, int? maxRecipients, string data, string title, global::Facebook.Unity.FacebookDelegate<global::Facebook.Unity.IAppRequestResult> callback)
		{
			editorWrapper.ShowAppRequestMockDialog(OnAppRequestsComplete, base.CallbackManager.AddFacebookDelegate(callback));
		}

		public override void ShareLink(global::System.Uri contentURL, string contentTitle, string contentDescription, global::System.Uri photoURL, global::Facebook.Unity.FacebookDelegate<global::Facebook.Unity.IShareResult> callback)
		{
			editorWrapper.ShowMockShareDialog(OnShareLinkComplete, "ShareLink", base.CallbackManager.AddFacebookDelegate(callback));
		}

		public override void FeedShare(string toId, global::System.Uri link, string linkName, string linkCaption, string linkDescription, global::System.Uri picture, string mediaSource, global::Facebook.Unity.FacebookDelegate<global::Facebook.Unity.IShareResult> callback)
		{
			editorWrapper.ShowMockShareDialog(OnShareLinkComplete, "FeedShare", base.CallbackManager.AddFacebookDelegate(callback));
		}

		public override void GameGroupCreate(string name, string description, string privacy, global::Facebook.Unity.FacebookDelegate<global::Facebook.Unity.IGroupCreateResult> callback)
		{
			editorWrapper.ShowGameGroupCreateMockDialog(OnGroupCreateComplete, base.CallbackManager.AddFacebookDelegate(callback));
		}

		public override void GameGroupJoin(string id, global::Facebook.Unity.FacebookDelegate<global::Facebook.Unity.IGroupJoinResult> callback)
		{
			editorWrapper.ShowGameGroupJoinMockDialog(OnGroupJoinComplete, base.CallbackManager.AddFacebookDelegate(callback));
		}

		public override void ActivateApp(string appId)
		{
			global::Facebook.Unity.FacebookLogger.Info("This only needs to be called for iOS or Android.");
		}

		public override void GetAppLink(global::Facebook.Unity.FacebookDelegate<global::Facebook.Unity.IAppLinkResult> callback)
		{
			global::System.Collections.Generic.Dictionary<string, object> dictionary = new global::System.Collections.Generic.Dictionary<string, object>();
			dictionary["url"] = "mockurl://testing.url";
			dictionary["callback_id"] = base.CallbackManager.AddFacebookDelegate(callback);
			OnGetAppLinkComplete(new global::Facebook.Unity.ResultContainer(dictionary));
		}

		public override void AppEventsLogEvent(string logEvent, float? valueToSum, global::System.Collections.Generic.Dictionary<string, object> parameters)
		{
			global::Facebook.Unity.FacebookLogger.Log("Pew! Pretending to send this off.  Doesn't actually work in the editor");
		}

		public override void AppEventsLogPurchase(float logPurchase, string currency, global::System.Collections.Generic.Dictionary<string, object> parameters)
		{
			global::Facebook.Unity.FacebookLogger.Log("Pew! Pretending to send this off.  Doesn't actually work in the editor");
		}

		public void AppInvite(global::System.Uri appLinkUrl, global::System.Uri previewImageUrl, global::Facebook.Unity.FacebookDelegate<global::Facebook.Unity.IAppInviteResult> callback)
		{
			editorWrapper.ShowAppInviteMockDialog(OnAppInviteComplete, base.CallbackManager.AddFacebookDelegate(callback));
		}

		public void FetchDeferredAppLink(global::Facebook.Unity.FacebookDelegate<global::Facebook.Unity.IAppLinkResult> callback)
		{
			global::System.Collections.Generic.Dictionary<string, object> dictionary = new global::System.Collections.Generic.Dictionary<string, object>();
			dictionary["url"] = "mockurl://testing.url";
			dictionary["ref"] = "mock ref";
			dictionary["extras"] = new global::System.Collections.Generic.Dictionary<string, object> { { "mock extra key", "mock extra value" } };
			dictionary["target_url"] = "mocktargeturl://mocktarget.url";
			dictionary["callback_id"] = base.CallbackManager.AddFacebookDelegate(callback);
			OnFetchDeferredAppLinkComplete(new global::Facebook.Unity.ResultContainer(dictionary));
		}

		public void Pay(string product, string action, int quantity, int? quantityMin, int? quantityMax, string requestId, string pricepointId, string testCurrency, global::Facebook.Unity.FacebookDelegate<global::Facebook.Unity.IPayResult> callback)
		{
			editorWrapper.ShowPayMockDialog(OnPayComplete, base.CallbackManager.AddFacebookDelegate(callback));
		}

		public void RefreshCurrentAccessToken(global::Facebook.Unity.FacebookDelegate<global::Facebook.Unity.IAccessTokenRefreshResult> callback)
		{
			if (callback != null)
			{
				global::System.Collections.Generic.Dictionary<string, object> dictionary = new global::System.Collections.Generic.Dictionary<string, object>();
				dictionary.Add("callback_id", base.CallbackManager.AddFacebookDelegate(callback));
				global::System.Collections.Generic.Dictionary<string, object> dictionary2 = dictionary;
				if (global::Facebook.Unity.AccessToken.CurrentAccessToken == null)
				{
					dictionary2["error"] = "No current access token";
				}
				else
				{
					global::System.Collections.Generic.IDictionary<string, object> source = (global::System.Collections.Generic.IDictionary<string, object>)global::Facebook.MiniJSON.Json.Deserialize(global::Facebook.Unity.AccessToken.CurrentAccessToken.ToJson());
					dictionary2.AddAllKVPFrom(source);
				}
				OnRefreshCurrentAccessTokenComplete(new global::Facebook.Unity.ResultContainer(dictionary2));
			}
		}

		public override void OnAppRequestsComplete(global::Facebook.Unity.ResultContainer resultContainer)
		{
			global::Facebook.Unity.AppRequestResult result = new global::Facebook.Unity.AppRequestResult(resultContainer);
			base.CallbackManager.OnFacebookResponse(result);
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

		public override void OnLoginComplete(global::Facebook.Unity.ResultContainer resultContainer)
		{
			global::Facebook.Unity.LoginResult result = new global::Facebook.Unity.LoginResult(resultContainer);
			OnAuthResponse(result);
		}

		public override void OnShareLinkComplete(global::Facebook.Unity.ResultContainer resultContainer)
		{
			global::Facebook.Unity.ShareResult result = new global::Facebook.Unity.ShareResult(resultContainer);
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

		public void OnPayComplete(global::Facebook.Unity.ResultContainer resultContainer)
		{
			global::Facebook.Unity.PayResult result = new global::Facebook.Unity.PayResult(resultContainer);
			base.CallbackManager.OnFacebookResponse(result);
		}

		public void OnRefreshCurrentAccessTokenComplete(global::Facebook.Unity.ResultContainer resultContainer)
		{
			global::Facebook.Unity.AccessTokenRefreshResult result = new global::Facebook.Unity.AccessTokenRefreshResult(resultContainer);
			base.CallbackManager.OnFacebookResponse(result);
		}

		public void OnFacebookAuthResponseChange(global::Facebook.Unity.ResultContainer resultContainer)
		{
			throw new global::System.NotSupportedException();
		}

		public void OnUrlResponse(string message)
		{
			throw new global::System.NotSupportedException();
		}
	}
}
