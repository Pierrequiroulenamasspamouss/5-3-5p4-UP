namespace Facebook.Unity.Mobile.Android
{
	internal sealed class AndroidFacebook : global::Facebook.Unity.Mobile.MobileFacebook
	{
		private class JavaMethodCall<T> : global::Facebook.Unity.MethodCall<T> where T : global::Facebook.Unity.IResult
		{
			private global::Facebook.Unity.Mobile.Android.AndroidFacebook androidImpl;

			public JavaMethodCall(global::Facebook.Unity.Mobile.Android.AndroidFacebook androidImpl, string methodName)
				: base((global::Facebook.Unity.FacebookBase)androidImpl, methodName)
			{
				this.androidImpl = androidImpl;
			}

			public override void Call(global::Facebook.Unity.MethodArguments args = null)
			{
				global::Facebook.Unity.MethodArguments methodArguments = ((args != null) ? new global::Facebook.Unity.MethodArguments(args) : new global::Facebook.Unity.MethodArguments());
				if (base.Callback != null)
				{
					methodArguments.AddString("callback_id", androidImpl.CallbackManager.AddFacebookDelegate(base.Callback));
				}
				androidImpl.CallFB(base.MethodName, methodArguments.ToJsonString());
			}
		}

		public const string LoginPermissionsKey = "scope";

		private bool limitEventUsage;

		private global::Facebook.Unity.Mobile.Android.IAndroidJavaClass facebookJava;

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
			: this(new global::Facebook.Unity.Mobile.Android.FBJavaClass(), new global::Facebook.Unity.CallbackManager())
		{
		}

		public AndroidFacebook(global::Facebook.Unity.Mobile.Android.IAndroidJavaClass facebookJavaClass, global::Facebook.Unity.CallbackManager callbackManager)
			: base(callbackManager)
		{
			KeyHash = string.Empty;
			facebookJava = facebookJavaClass;
		}

		public void Init(string appId, global::Facebook.Unity.HideUnityDelegate hideUnityDelegate, global::Facebook.Unity.InitDelegate onInitComplete)
		{
			CallFB("SetUserAgentSuffix", string.Format("Unity.{0}", global::Facebook.Unity.Constants.UnitySDKUserAgentSuffixLegacy));
			base.Init(hideUnityDelegate, onInitComplete);
			global::Facebook.Unity.MethodArguments methodArguments = new global::Facebook.Unity.MethodArguments();
			methodArguments.AddString("appId", appId);
			global::Facebook.Unity.Mobile.Android.AndroidFacebook.JavaMethodCall<global::Facebook.Unity.IResult> javaMethodCall = new global::Facebook.Unity.Mobile.Android.AndroidFacebook.JavaMethodCall<global::Facebook.Unity.IResult>(this, "Init");
			javaMethodCall.Call(methodArguments);
		}

		public override void LogInWithReadPermissions(global::System.Collections.Generic.IEnumerable<string> permissions, global::Facebook.Unity.FacebookDelegate<global::Facebook.Unity.ILoginResult> callback)
		{
			global::Facebook.Unity.MethodArguments methodArguments = new global::Facebook.Unity.MethodArguments();
			methodArguments.AddCommaSeparatedList("scope", permissions);
			global::Facebook.Unity.Mobile.Android.AndroidFacebook.JavaMethodCall<global::Facebook.Unity.ILoginResult> javaMethodCall = new global::Facebook.Unity.Mobile.Android.AndroidFacebook.JavaMethodCall<global::Facebook.Unity.ILoginResult>(this, "LoginWithReadPermissions");
			javaMethodCall.Callback = callback;
			javaMethodCall.Call(methodArguments);
		}

		public override void LogInWithPublishPermissions(global::System.Collections.Generic.IEnumerable<string> permissions, global::Facebook.Unity.FacebookDelegate<global::Facebook.Unity.ILoginResult> callback)
		{
			global::Facebook.Unity.MethodArguments methodArguments = new global::Facebook.Unity.MethodArguments();
			methodArguments.AddCommaSeparatedList("scope", permissions);
			global::Facebook.Unity.Mobile.Android.AndroidFacebook.JavaMethodCall<global::Facebook.Unity.ILoginResult> javaMethodCall = new global::Facebook.Unity.Mobile.Android.AndroidFacebook.JavaMethodCall<global::Facebook.Unity.ILoginResult>(this, "LoginWithPublishPermissions");
			javaMethodCall.Callback = callback;
			javaMethodCall.Call(methodArguments);
		}

		public override void LogOut()
		{
			global::Facebook.Unity.Mobile.Android.AndroidFacebook.JavaMethodCall<global::Facebook.Unity.IResult> javaMethodCall = new global::Facebook.Unity.Mobile.Android.AndroidFacebook.JavaMethodCall<global::Facebook.Unity.IResult>(this, "Logout");
			javaMethodCall.Call();
		}

		public override void AppRequest(string message, global::Facebook.Unity.OGActionType? actionType, string objectId, global::System.Collections.Generic.IEnumerable<string> to, global::System.Collections.Generic.IEnumerable<object> filters, global::System.Collections.Generic.IEnumerable<string> excludeIds, int? maxRecipients, string data, string title, global::Facebook.Unity.FacebookDelegate<global::Facebook.Unity.IAppRequestResult> callback)
		{
			ValidateAppRequestArgs(message, actionType, objectId, to, filters, excludeIds, maxRecipients, data, title, callback);
			global::Facebook.Unity.MethodArguments methodArguments = new global::Facebook.Unity.MethodArguments();
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
			global::Facebook.Unity.Mobile.Android.AndroidFacebook.JavaMethodCall<global::Facebook.Unity.IAppRequestResult> javaMethodCall = new global::Facebook.Unity.Mobile.Android.AndroidFacebook.JavaMethodCall<global::Facebook.Unity.IAppRequestResult>(this, "AppRequest");
			javaMethodCall.Callback = callback;
			javaMethodCall.Call(methodArguments);
		}

		public override void AppInvite(global::System.Uri appLinkUrl, global::System.Uri previewImageUrl, global::Facebook.Unity.FacebookDelegate<global::Facebook.Unity.IAppInviteResult> callback)
		{
			global::Facebook.Unity.MethodArguments methodArguments = new global::Facebook.Unity.MethodArguments();
			methodArguments.AddUri("appLinkUrl", appLinkUrl);
			methodArguments.AddUri("previewImageUrl", previewImageUrl);
			global::Facebook.Unity.Mobile.Android.AndroidFacebook.JavaMethodCall<global::Facebook.Unity.IAppInviteResult> javaMethodCall = new global::Facebook.Unity.Mobile.Android.AndroidFacebook.JavaMethodCall<global::Facebook.Unity.IAppInviteResult>(this, "AppInvite");
			javaMethodCall.Callback = callback;
			javaMethodCall.Call(methodArguments);
		}

		public override void ShareLink(global::System.Uri contentURL, string contentTitle, string contentDescription, global::System.Uri photoURL, global::Facebook.Unity.FacebookDelegate<global::Facebook.Unity.IShareResult> callback)
		{
			global::Facebook.Unity.MethodArguments methodArguments = new global::Facebook.Unity.MethodArguments();
			methodArguments.AddUri("content_url", contentURL);
			methodArguments.AddString("content_title", contentTitle);
			methodArguments.AddString("content_description", contentDescription);
			methodArguments.AddUri("photo_url", photoURL);
			global::Facebook.Unity.Mobile.Android.AndroidFacebook.JavaMethodCall<global::Facebook.Unity.IShareResult> javaMethodCall = new global::Facebook.Unity.Mobile.Android.AndroidFacebook.JavaMethodCall<global::Facebook.Unity.IShareResult>(this, "ShareLink");
			javaMethodCall.Callback = callback;
			javaMethodCall.Call(methodArguments);
		}

		public override void FeedShare(string toId, global::System.Uri link, string linkName, string linkCaption, string linkDescription, global::System.Uri picture, string mediaSource, global::Facebook.Unity.FacebookDelegate<global::Facebook.Unity.IShareResult> callback)
		{
			global::Facebook.Unity.MethodArguments methodArguments = new global::Facebook.Unity.MethodArguments();
			methodArguments.AddString("toId", toId);
			methodArguments.AddUri("link", link);
			methodArguments.AddString("linkName", linkName);
			methodArguments.AddString("linkCaption", linkCaption);
			methodArguments.AddString("linkDescription", linkDescription);
			methodArguments.AddUri("picture", picture);
			methodArguments.AddString("mediaSource", mediaSource);
			global::Facebook.Unity.Mobile.Android.AndroidFacebook.JavaMethodCall<global::Facebook.Unity.IShareResult> javaMethodCall = new global::Facebook.Unity.Mobile.Android.AndroidFacebook.JavaMethodCall<global::Facebook.Unity.IShareResult>(this, "FeedShare");
			javaMethodCall.Callback = callback;
			javaMethodCall.Call(methodArguments);
		}

		public override void GameGroupCreate(string name, string description, string privacy, global::Facebook.Unity.FacebookDelegate<global::Facebook.Unity.IGroupCreateResult> callback)
		{
			global::Facebook.Unity.MethodArguments methodArguments = new global::Facebook.Unity.MethodArguments();
			methodArguments.AddString("name", name);
			methodArguments.AddString("description", description);
			methodArguments.AddString("privacy", privacy);
			global::Facebook.Unity.Mobile.Android.AndroidFacebook.JavaMethodCall<global::Facebook.Unity.IGroupCreateResult> javaMethodCall = new global::Facebook.Unity.Mobile.Android.AndroidFacebook.JavaMethodCall<global::Facebook.Unity.IGroupCreateResult>(this, "GameGroupCreate");
			javaMethodCall.Callback = callback;
			javaMethodCall.Call(methodArguments);
		}

		public override void GameGroupJoin(string id, global::Facebook.Unity.FacebookDelegate<global::Facebook.Unity.IGroupJoinResult> callback)
		{
			global::Facebook.Unity.MethodArguments methodArguments = new global::Facebook.Unity.MethodArguments();
			methodArguments.AddString("id", id);
			global::Facebook.Unity.Mobile.Android.AndroidFacebook.JavaMethodCall<global::Facebook.Unity.IGroupJoinResult> javaMethodCall = new global::Facebook.Unity.Mobile.Android.AndroidFacebook.JavaMethodCall<global::Facebook.Unity.IGroupJoinResult>(this, "GameGroupJoin");
			javaMethodCall.Callback = callback;
			javaMethodCall.Call(methodArguments);
		}

		public override void GetAppLink(global::Facebook.Unity.FacebookDelegate<global::Facebook.Unity.IAppLinkResult> callback)
		{
			global::Facebook.Unity.Mobile.Android.AndroidFacebook.JavaMethodCall<global::Facebook.Unity.IAppLinkResult> javaMethodCall = new global::Facebook.Unity.Mobile.Android.AndroidFacebook.JavaMethodCall<global::Facebook.Unity.IAppLinkResult>(this, "GetAppLink");
			javaMethodCall.Callback = callback;
			javaMethodCall.Call();
		}

		public override void AppEventsLogEvent(string logEvent, float? valueToSum, global::System.Collections.Generic.Dictionary<string, object> parameters)
		{
			global::Facebook.Unity.MethodArguments methodArguments = new global::Facebook.Unity.MethodArguments();
			methodArguments.AddString("logEvent", logEvent);
			methodArguments.AddNullablePrimitive("valueToSum", valueToSum);
			methodArguments.AddDictionary("parameters", parameters);
			global::Facebook.Unity.Mobile.Android.AndroidFacebook.JavaMethodCall<global::Facebook.Unity.IResult> javaMethodCall = new global::Facebook.Unity.Mobile.Android.AndroidFacebook.JavaMethodCall<global::Facebook.Unity.IResult>(this, "LogAppEvent");
			javaMethodCall.Call(methodArguments);
		}

		public override void AppEventsLogPurchase(float logPurchase, string currency, global::System.Collections.Generic.Dictionary<string, object> parameters)
		{
			global::Facebook.Unity.MethodArguments methodArguments = new global::Facebook.Unity.MethodArguments();
			methodArguments.AddPrimative("logPurchase", logPurchase);
			methodArguments.AddString("currency", currency);
			methodArguments.AddDictionary("parameters", parameters);
			global::Facebook.Unity.Mobile.Android.AndroidFacebook.JavaMethodCall<global::Facebook.Unity.IResult> javaMethodCall = new global::Facebook.Unity.Mobile.Android.AndroidFacebook.JavaMethodCall<global::Facebook.Unity.IResult>(this, "LogAppEvent");
			javaMethodCall.Call(methodArguments);
		}

		public override void ActivateApp(string appId)
		{
		}

		public override void FetchDeferredAppLink(global::Facebook.Unity.FacebookDelegate<global::Facebook.Unity.IAppLinkResult> callback)
		{
			global::Facebook.Unity.MethodArguments args = new global::Facebook.Unity.MethodArguments();
			global::Facebook.Unity.Mobile.Android.AndroidFacebook.JavaMethodCall<global::Facebook.Unity.IAppLinkResult> javaMethodCall = new global::Facebook.Unity.Mobile.Android.AndroidFacebook.JavaMethodCall<global::Facebook.Unity.IAppLinkResult>(this, "FetchDeferredAppLinkData");
			javaMethodCall.Callback = callback;
			javaMethodCall.Call(args);
		}

		public override void RefreshCurrentAccessToken(global::Facebook.Unity.FacebookDelegate<global::Facebook.Unity.IAccessTokenRefreshResult> callback)
		{
			global::Facebook.Unity.Mobile.Android.AndroidFacebook.JavaMethodCall<global::Facebook.Unity.IAccessTokenRefreshResult> javaMethodCall = new global::Facebook.Unity.Mobile.Android.AndroidFacebook.JavaMethodCall<global::Facebook.Unity.IAccessTokenRefreshResult>(this, "RefreshCurrentAccessToken");
			javaMethodCall.Callback = callback;
			javaMethodCall.Call();
		}

		protected override void SetShareDialogMode(global::Facebook.Unity.ShareDialogMode mode)
		{
			CallFB("SetShareDialogMode", mode.ToString());
		}

		private void CallFB(string method, string args)
		{
			facebookJava.CallStatic(method, args);
		}
	}
}
