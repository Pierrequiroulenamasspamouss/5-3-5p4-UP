namespace Discord.Unity.Mobile.Android
{
	internal sealed class AndroidFacebook : global::Discord.Unity.Mobile.MobileFacebook
	{
		private class JavaMethodCall<T> : global::Discord.Unity.MethodCall<T> where T : global::Discord.Unity.IResult
		{
			private global::Discord.Unity.Mobile.Android.AndroidFacebook androidImpl;

			public JavaMethodCall(global::Discord.Unity.Mobile.Android.AndroidFacebook androidImpl, string methodName)
				: base((global::Discord.Unity.FacebookBase)androidImpl, methodName)
			{
				this.androidImpl = androidImpl;
			}

			public override void Call(global::Discord.Unity.MethodArguments args = null)
			{
				global::Discord.Unity.MethodArguments methodArguments = ((args != null) ? new global::Discord.Unity.MethodArguments(args) : new global::Discord.Unity.MethodArguments());
				if (base.Callback != null)
				{
					methodArguments.AddString("callback_id", androidImpl.CallbackManager.AddFacebookDelegate(base.Callback));
				}
				androidImpl.CallFB(base.MethodName, methodArguments.ToJsonString());
			}
		}

		public const string LoginPermissionsKey = "scope";

		private bool limitEventUsage;

		private global::Discord.Unity.Mobile.Android.IAndroidJavaClass facebookJava;

		public string KeyHash { get; private set; }

		public override bool LimitEventUsage
		{
			get
			{
				return limitEventUsage;
			}
			set
			{
				limitEventUsage = value;
				CallFB("SetLimitEventUsage", value.ToString());
			}
		}

		public override string SDKName
		{
			get
			{
				return "FBAndroidSDK";
			}
		}

		public override string SDKVersion
		{
			get
			{
				return facebookJava.CallStatic<string>("GetSdkVersion");
			}
		}

		public AndroidFacebook()
			: this(new global::Discord.Unity.Mobile.Android.FBJavaClass(), new global::Discord.Unity.CallbackManager())
		{
		}

		public AndroidFacebook(global::Discord.Unity.Mobile.Android.IAndroidJavaClass facebookJavaClass, global::Discord.Unity.CallbackManager callbackManager)
			: base(callbackManager)
		{
			KeyHash = string.Empty;
			facebookJava = facebookJavaClass;
		}

		public void Init(string appId, global::Discord.Unity.HideUnityDelegate hideUnityDelegate, global::Discord.Unity.InitDelegate onInitComplete)
		{
			CallFB("SetUserAgentSuffix", string.Format("Unity.{0}", global::Discord.Unity.Constants.UnitySDKUserAgentSuffixLegacy));
			base.Init(hideUnityDelegate, onInitComplete);
			global::Discord.Unity.MethodArguments methodArguments = new global::Discord.Unity.MethodArguments();
			methodArguments.AddString("appId", appId);
			global::Discord.Unity.Mobile.Android.AndroidFacebook.JavaMethodCall<global::Discord.Unity.IResult> javaMethodCall = new global::Discord.Unity.Mobile.Android.AndroidFacebook.JavaMethodCall<global::Discord.Unity.IResult>(this, "Init");
			javaMethodCall.Call(methodArguments);
		}

		public override void LogInWithReadPermissions(global::System.Collections.Generic.IEnumerable<string> permissions, global::Discord.Unity.FacebookDelegate<global::Discord.Unity.ILoginResult> callback)
		{
			global::Discord.Unity.MethodArguments methodArguments = new global::Discord.Unity.MethodArguments();
			methodArguments.AddCommaSeparatedList("scope", permissions);
			global::Discord.Unity.Mobile.Android.AndroidFacebook.JavaMethodCall<global::Discord.Unity.ILoginResult> javaMethodCall = new global::Discord.Unity.Mobile.Android.AndroidFacebook.JavaMethodCall<global::Discord.Unity.ILoginResult>(this, "LoginWithReadPermissions");
			javaMethodCall.Callback = callback;
			javaMethodCall.Call(methodArguments);
		}

		public override void LogInWithPublishPermissions(global::System.Collections.Generic.IEnumerable<string> permissions, global::Discord.Unity.FacebookDelegate<global::Discord.Unity.ILoginResult> callback)
		{
			global::Discord.Unity.MethodArguments methodArguments = new global::Discord.Unity.MethodArguments();
			methodArguments.AddCommaSeparatedList("scope", permissions);
			global::Discord.Unity.Mobile.Android.AndroidFacebook.JavaMethodCall<global::Discord.Unity.ILoginResult> javaMethodCall = new global::Discord.Unity.Mobile.Android.AndroidFacebook.JavaMethodCall<global::Discord.Unity.ILoginResult>(this, "LoginWithPublishPermissions");
			javaMethodCall.Callback = callback;
			javaMethodCall.Call(methodArguments);
		}

		public override void LogOut()
		{
			global::Discord.Unity.Mobile.Android.AndroidFacebook.JavaMethodCall<global::Discord.Unity.IResult> javaMethodCall = new global::Discord.Unity.Mobile.Android.AndroidFacebook.JavaMethodCall<global::Discord.Unity.IResult>(this, "Logout");
			javaMethodCall.Call();
		}

		public override void AppRequest(string message, global::Discord.Unity.OGActionType? actionType, string objectId, global::System.Collections.Generic.IEnumerable<string> to, global::System.Collections.Generic.IEnumerable<object> filters, global::System.Collections.Generic.IEnumerable<string> excludeIds, int? maxRecipients, string data, string title, global::Discord.Unity.FacebookDelegate<global::Discord.Unity.IAppRequestResult> callback)
		{
			ValidateAppRequestArgs(message, actionType, objectId, to, filters, excludeIds, maxRecipients, data, title, callback);
			global::Discord.Unity.MethodArguments methodArguments = new global::Discord.Unity.MethodArguments();
			methodArguments.AddString("message", message);
			methodArguments.AddNullablePrimitive("action_type", actionType);
			methodArguments.AddString("object_id", objectId);
			methodArguments.AddCommaSeparatedList("to", to);
			if (filters != null && global::System.Linq.Enumerable.Any(filters))
			{
				string text = global::System.Linq.Enumerable.First(filters) as string;
				if (text != null)
				{
					methodArguments.AddString("filters", text);
				}
			}
			methodArguments.AddNullablePrimitive("max_recipients", maxRecipients);
			methodArguments.AddString("data", data);
			methodArguments.AddString("title", title);
			global::Discord.Unity.Mobile.Android.AndroidFacebook.JavaMethodCall<global::Discord.Unity.IAppRequestResult> javaMethodCall = new global::Discord.Unity.Mobile.Android.AndroidFacebook.JavaMethodCall<global::Discord.Unity.IAppRequestResult>(this, "AppRequest");
			javaMethodCall.Callback = callback;
			javaMethodCall.Call(methodArguments);
		}

		public override void AppInvite(global::System.Uri appLinkUrl, global::System.Uri previewImageUrl, global::Discord.Unity.FacebookDelegate<global::Discord.Unity.IAppInviteResult> callback)
		{
			global::Discord.Unity.MethodArguments methodArguments = new global::Discord.Unity.MethodArguments();
			methodArguments.AddUri("appLinkUrl", appLinkUrl);
			methodArguments.AddUri("previewImageUrl", previewImageUrl);
			global::Discord.Unity.Mobile.Android.AndroidFacebook.JavaMethodCall<global::Discord.Unity.IAppInviteResult> javaMethodCall = new global::Discord.Unity.Mobile.Android.AndroidFacebook.JavaMethodCall<global::Discord.Unity.IAppInviteResult>(this, "AppInvite");
			javaMethodCall.Callback = callback;
			javaMethodCall.Call(methodArguments);
		}

		public override void ShareLink(global::System.Uri contentURL, string contentTitle, string contentDescription, global::System.Uri photoURL, global::Discord.Unity.FacebookDelegate<global::Discord.Unity.IShareResult> callback)
		{
			global::Discord.Unity.MethodArguments methodArguments = new global::Discord.Unity.MethodArguments();
			methodArguments.AddUri("content_url", contentURL);
			methodArguments.AddString("content_title", contentTitle);
			methodArguments.AddString("content_description", contentDescription);
			methodArguments.AddUri("photo_url", photoURL);
			global::Discord.Unity.Mobile.Android.AndroidFacebook.JavaMethodCall<global::Discord.Unity.IShareResult> javaMethodCall = new global::Discord.Unity.Mobile.Android.AndroidFacebook.JavaMethodCall<global::Discord.Unity.IShareResult>(this, "ShareLink");
			javaMethodCall.Callback = callback;
			javaMethodCall.Call(methodArguments);
		}

		public override void FeedShare(string toId, global::System.Uri link, string linkName, string linkCaption, string linkDescription, global::System.Uri picture, string mediaSource, global::Discord.Unity.FacebookDelegate<global::Discord.Unity.IShareResult> callback)
		{
			global::Discord.Unity.MethodArguments methodArguments = new global::Discord.Unity.MethodArguments();
			methodArguments.AddString("toId", toId);
			methodArguments.AddUri("link", link);
			methodArguments.AddString("linkName", linkName);
			methodArguments.AddString("linkCaption", linkCaption);
			methodArguments.AddString("linkDescription", linkDescription);
			methodArguments.AddUri("picture", picture);
			methodArguments.AddString("mediaSource", mediaSource);
			global::Discord.Unity.Mobile.Android.AndroidFacebook.JavaMethodCall<global::Discord.Unity.IShareResult> javaMethodCall = new global::Discord.Unity.Mobile.Android.AndroidFacebook.JavaMethodCall<global::Discord.Unity.IShareResult>(this, "FeedShare");
			javaMethodCall.Callback = callback;
			javaMethodCall.Call(methodArguments);
		}

		public override void GameGroupCreate(string name, string description, string privacy, global::Discord.Unity.FacebookDelegate<global::Discord.Unity.IGroupCreateResult> callback)
		{
			global::Discord.Unity.MethodArguments methodArguments = new global::Discord.Unity.MethodArguments();
			methodArguments.AddString("name", name);
			methodArguments.AddString("description", description);
			methodArguments.AddString("privacy", privacy);
			global::Discord.Unity.Mobile.Android.AndroidFacebook.JavaMethodCall<global::Discord.Unity.IGroupCreateResult> javaMethodCall = new global::Discord.Unity.Mobile.Android.AndroidFacebook.JavaMethodCall<global::Discord.Unity.IGroupCreateResult>(this, "GameGroupCreate");
			javaMethodCall.Callback = callback;
			javaMethodCall.Call(methodArguments);
		}

		public override void GameGroupJoin(string id, global::Discord.Unity.FacebookDelegate<global::Discord.Unity.IGroupJoinResult> callback)
		{
			global::Discord.Unity.MethodArguments methodArguments = new global::Discord.Unity.MethodArguments();
			methodArguments.AddString("id", id);
			global::Discord.Unity.Mobile.Android.AndroidFacebook.JavaMethodCall<global::Discord.Unity.IGroupJoinResult> javaMethodCall = new global::Discord.Unity.Mobile.Android.AndroidFacebook.JavaMethodCall<global::Discord.Unity.IGroupJoinResult>(this, "GameGroupJoin");
			javaMethodCall.Callback = callback;
			javaMethodCall.Call(methodArguments);
		}

		public override void GetAppLink(global::Discord.Unity.FacebookDelegate<global::Discord.Unity.IAppLinkResult> callback)
		{
			global::Discord.Unity.Mobile.Android.AndroidFacebook.JavaMethodCall<global::Discord.Unity.IAppLinkResult> javaMethodCall = new global::Discord.Unity.Mobile.Android.AndroidFacebook.JavaMethodCall<global::Discord.Unity.IAppLinkResult>(this, "GetAppLink");
			javaMethodCall.Callback = callback;
			javaMethodCall.Call();
		}

		public override void AppEventsLogEvent(string logEvent, float? valueToSum, global::System.Collections.Generic.Dictionary<string, object> parameters)
		{
			global::Discord.Unity.MethodArguments methodArguments = new global::Discord.Unity.MethodArguments();
			methodArguments.AddString("logEvent", logEvent);
			methodArguments.AddNullablePrimitive("valueToSum", valueToSum);
			methodArguments.AddDictionary("parameters", parameters);
			global::Discord.Unity.Mobile.Android.AndroidFacebook.JavaMethodCall<global::Discord.Unity.IResult> javaMethodCall = new global::Discord.Unity.Mobile.Android.AndroidFacebook.JavaMethodCall<global::Discord.Unity.IResult>(this, "LogAppEvent");
			javaMethodCall.Call(methodArguments);
		}

		public override void AppEventsLogPurchase(float logPurchase, string currency, global::System.Collections.Generic.Dictionary<string, object> parameters)
		{
			global::Discord.Unity.MethodArguments methodArguments = new global::Discord.Unity.MethodArguments();
			methodArguments.AddPrimative("logPurchase", logPurchase);
			methodArguments.AddString("currency", currency);
			methodArguments.AddDictionary("parameters", parameters);
			global::Discord.Unity.Mobile.Android.AndroidFacebook.JavaMethodCall<global::Discord.Unity.IResult> javaMethodCall = new global::Discord.Unity.Mobile.Android.AndroidFacebook.JavaMethodCall<global::Discord.Unity.IResult>(this, "LogAppEvent");
			javaMethodCall.Call(methodArguments);
		}

		public override void ActivateApp(string appId)
		{
		}

		public override void FetchDeferredAppLink(global::Discord.Unity.FacebookDelegate<global::Discord.Unity.IAppLinkResult> callback)
		{
			global::Discord.Unity.MethodArguments args = new global::Discord.Unity.MethodArguments();
			global::Discord.Unity.Mobile.Android.AndroidFacebook.JavaMethodCall<global::Discord.Unity.IAppLinkResult> javaMethodCall = new global::Discord.Unity.Mobile.Android.AndroidFacebook.JavaMethodCall<global::Discord.Unity.IAppLinkResult>(this, "FetchDeferredAppLinkData");
			javaMethodCall.Callback = callback;
			javaMethodCall.Call(args);
		}

		public override void RefreshCurrentAccessToken(global::Discord.Unity.FacebookDelegate<global::Discord.Unity.IAccessTokenRefreshResult> callback)
		{
			global::Discord.Unity.Mobile.Android.AndroidFacebook.JavaMethodCall<global::Discord.Unity.IAccessTokenRefreshResult> javaMethodCall = new global::Discord.Unity.Mobile.Android.AndroidFacebook.JavaMethodCall<global::Discord.Unity.IAccessTokenRefreshResult>(this, "RefreshCurrentAccessToken");
			javaMethodCall.Callback = callback;
			javaMethodCall.Call();
		}

		protected override void SetShareDialogMode(global::Discord.Unity.ShareDialogMode mode)
		{
			CallFB("SetShareDialogMode", mode.ToString());
		}

		private void CallFB(string method, string args)
		{
			facebookJava.CallStatic(method, args);
		}
	}
}
