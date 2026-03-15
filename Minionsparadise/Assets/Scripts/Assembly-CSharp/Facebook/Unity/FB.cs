namespace Facebook.Unity
{
	public sealed class FB : global::UnityEngine.ScriptableObject
	{
		public sealed class Canvas
		{
			private static global::Facebook.Unity.IPayFacebook FacebookPayImpl
			{
				get
				{
					global::Facebook.Unity.IPayFacebook payFacebook = FacebookImpl as global::Facebook.Unity.IPayFacebook;
					if (payFacebook == null)
					{
						throw new global::System.InvalidOperationException("Attempt to call Facebook pay interface on unsupported platform");
					}
					return payFacebook;
				}
			}

			public static void Pay(string product, string action = "purchaseitem", int quantity = 1, int? quantityMin = null, int? quantityMax = null, string requestId = null, string pricepointId = null, string testCurrency = null, global::Facebook.Unity.FacebookDelegate<global::Facebook.Unity.IPayResult> callback = null)
			{
				FacebookPayImpl.Pay(product, action, quantity, quantityMin, quantityMax, requestId, pricepointId, testCurrency, callback);
			}
		}

		public sealed class Mobile
		{
			public static global::Facebook.Unity.ShareDialogMode ShareDialogMode
			{
				get
				{
					return MobileFacebookImpl.ShareDialogMode;
				}
				set
				{
					MobileFacebookImpl.ShareDialogMode = value;
				}
			}

			private static global::Facebook.Unity.Mobile.IMobileFacebook MobileFacebookImpl
			{
				get
				{
					global::Facebook.Unity.Mobile.IMobileFacebook mobileFacebook = FacebookImpl as global::Facebook.Unity.Mobile.IMobileFacebook;
					if (mobileFacebook == null)
					{
						throw new global::System.InvalidOperationException("Attempt to call Mobile interface on non mobile platform");
					}
					return mobileFacebook;
				}
			}

			public static void AppInvite(global::System.Uri appLinkUrl, global::System.Uri previewImageUrl = null, global::Facebook.Unity.FacebookDelegate<global::Facebook.Unity.IAppInviteResult> callback = null)
			{
				MobileFacebookImpl.AppInvite(appLinkUrl, previewImageUrl, callback);
			}

			public static void FetchDeferredAppLinkData(global::Facebook.Unity.FacebookDelegate<global::Facebook.Unity.IAppLinkResult> callback = null)
			{
				if (callback != null)
				{
					MobileFacebookImpl.FetchDeferredAppLink(callback);
				}
			}

			public static void RefreshCurrentAccessToken(global::Facebook.Unity.FacebookDelegate<global::Facebook.Unity.IAccessTokenRefreshResult> callback = null)
			{
				MobileFacebookImpl.RefreshCurrentAccessToken(callback);
			}
		}

		public sealed class Android
		{
			public static string KeyHash
			{
				get
				{
					global::Facebook.Unity.Mobile.Android.AndroidFacebook androidFacebook = FacebookImpl as global::Facebook.Unity.Mobile.Android.AndroidFacebook;
					return (androidFacebook == null) ? string.Empty : androidFacebook.KeyHash;
				}
			}
		}

		internal abstract class CompiledFacebookLoader : global::UnityEngine.MonoBehaviour
		{
			protected abstract global::Facebook.Unity.FacebookGameObject FBGameObject { get; }

			public void Start()
			{
				facebook = FBGameObject.Facebook;
				OnDLLLoadedDelegate();
				LogVersion();
				global::UnityEngine.Object.Destroy(this);
			}
		}

		private delegate void OnDLLLoaded();

		private const string DefaultJSSDKLocale = "en_US";

		private static global::Facebook.Unity.IFacebook facebook;

		private static bool isInitCalled;

		private static string facebookDomain = "facebook.com";

		private static string graphApiVersion = "v2.5";

		public static string AppId { get; private set; }

		public static string GraphApiVersion
		{
			get
			{
				return graphApiVersion;
			}
			set
			{
				graphApiVersion = value;
			}
		}

		public static bool IsLoggedIn
		{
			get
			{
				return facebook != null && FacebookImpl.LoggedIn;
			}
		}

		public static bool IsInitialized
		{
			get
			{
				return facebook != null && facebook.Initialized;
			}
		}

		public static bool LimitAppEventUsage
		{
			get
			{
				return facebook != null && facebook.LimitEventUsage;
			}
			set
			{
				if (facebook != null)
				{
					facebook.LimitEventUsage = value;
				}
			}
		}

		internal static global::Facebook.Unity.IFacebook FacebookImpl
		{
			get
			{
				if (facebook == null)
				{
					throw new global::System.NullReferenceException("Facebook object is not yet loaded.  Did you call FB.Init()?");
				}
				return facebook;
			}
			set
			{
				facebook = value;
			}
		}

		internal static string FacebookDomain
		{
			get
			{
				return facebookDomain;
			}
			set
			{
				facebookDomain = value;
			}
		}

		private static global::Facebook.Unity.FB.OnDLLLoaded OnDLLLoadedDelegate { get; set; }

		public static void Init(global::Facebook.Unity.InitDelegate onInitComplete = null, global::Facebook.Unity.HideUnityDelegate onHideUnity = null, string authResponse = null)
		{
			Init(global::Facebook.Unity.FacebookSettings.AppId, global::Facebook.Unity.FacebookSettings.Cookie, global::Facebook.Unity.FacebookSettings.Logging, global::Facebook.Unity.FacebookSettings.Status, global::Facebook.Unity.FacebookSettings.Xfbml, global::Facebook.Unity.FacebookSettings.FrictionlessRequests, authResponse, "en_US", onHideUnity, onInitComplete);
		}

		public static void Init(string appId, bool cookie = true, bool logging = true, bool status = true, bool xfbml = false, bool frictionlessRequests = true, string authResponse = null, string javascriptSDKLocale = "en_US", global::Facebook.Unity.HideUnityDelegate onHideUnity = null, global::Facebook.Unity.InitDelegate onInitComplete = null)
		{
			if (string.IsNullOrEmpty(appId))
			{
				throw new global::System.ArgumentException("appId cannot be null or empty!");
			}
			AppId = appId;
			if (!isInitCalled)
			{
				isInitCalled = true;
				if (global::Facebook.Unity.Constants.IsEditor)
				{
					OnDLLLoadedDelegate = delegate
					{
						((global::Facebook.Unity.Editor.EditorFacebook)facebook).Init(onHideUnity, onInitComplete);
					};
					global::Facebook.Unity.ComponentFactory.GetComponent<global::Facebook.Unity.Editor.EditorFacebookLoader>();
					return;
				}
				switch (global::Facebook.Unity.Constants.CurrentPlatform)
				{
				case global::Facebook.Unity.FacebookUnityPlatform.WebGL:
				case global::Facebook.Unity.FacebookUnityPlatform.WebPlayer:
					OnDLLLoadedDelegate = delegate
					{
						((global::Facebook.Unity.Canvas.CanvasFacebook)facebook).Init(appId, cookie, logging, status, xfbml, global::Facebook.Unity.FacebookSettings.ChannelUrl, authResponse, frictionlessRequests, javascriptSDKLocale, global::Facebook.Unity.Constants.DebugMode, onHideUnity, onInitComplete);
					};
					global::Facebook.Unity.ComponentFactory.GetComponent<global::Facebook.Unity.Canvas.CanvasFacebookLoader>();
					break;
				case global::Facebook.Unity.FacebookUnityPlatform.IOS:
					OnDLLLoadedDelegate = delegate
					{
						((global::Facebook.Unity.Mobile.IOS.IOSFacebook)facebook).Init(appId, frictionlessRequests, global::Facebook.Unity.FacebookSettings.IosURLSuffix, onHideUnity, onInitComplete);
					};
					global::Facebook.Unity.ComponentFactory.GetComponent<global::Facebook.Unity.Mobile.IOS.IOSFacebookLoader>();
					break;
				case global::Facebook.Unity.FacebookUnityPlatform.Android:
					OnDLLLoadedDelegate = delegate
					{
						((global::Facebook.Unity.Mobile.Android.AndroidFacebook)facebook).Init(appId, onHideUnity, onInitComplete);
					};
					global::Facebook.Unity.ComponentFactory.GetComponent<global::Facebook.Unity.Mobile.Android.AndroidFacebookLoader>();
					break;
				default:
					throw new global::System.NotImplementedException("Facebook API does not yet support this platform");
				}
			}
			else
			{
				global::Facebook.Unity.FacebookLogger.Warn("FB.Init() has already been called.  You only need to call this once and only once.");
			}
		}

		public static void LogInWithPublishPermissions(global::System.Collections.Generic.IEnumerable<string> permissions = null, global::Facebook.Unity.FacebookDelegate<global::Facebook.Unity.ILoginResult> callback = null)
		{
			FacebookImpl.LogInWithPublishPermissions(permissions, callback);
		}

		public static void LogInWithReadPermissions(global::System.Collections.Generic.IEnumerable<string> permissions = null, global::Facebook.Unity.FacebookDelegate<global::Facebook.Unity.ILoginResult> callback = null)
		{
			FacebookImpl.LogInWithReadPermissions(permissions, callback);
		}

		public static void LogOut()
		{
			FacebookImpl.LogOut();
		}

		public static void AppRequest(string message, global::Facebook.Unity.OGActionType actionType, string objectId, global::System.Collections.Generic.IEnumerable<string> to, string data = "", string title = "", global::Facebook.Unity.FacebookDelegate<global::Facebook.Unity.IAppRequestResult> callback = null)
		{
			FacebookImpl.AppRequest(message, actionType, objectId, to, null, null, null, data, title, callback);
		}

		public static void AppRequest(string message, global::Facebook.Unity.OGActionType actionType, string objectId, global::System.Collections.Generic.IEnumerable<object> filters = null, global::System.Collections.Generic.IEnumerable<string> excludeIds = null, int? maxRecipients = null, string data = "", string title = "", global::Facebook.Unity.FacebookDelegate<global::Facebook.Unity.IAppRequestResult> callback = null)
		{
			FacebookImpl.AppRequest(message, actionType, objectId, null, filters, excludeIds, maxRecipients, data, title, callback);
		}

		public static void AppRequest(string message, global::System.Collections.Generic.IEnumerable<string> to = null, global::System.Collections.Generic.IEnumerable<object> filters = null, global::System.Collections.Generic.IEnumerable<string> excludeIds = null, int? maxRecipients = null, string data = "", string title = "", global::Facebook.Unity.FacebookDelegate<global::Facebook.Unity.IAppRequestResult> callback = null)
		{
			FacebookImpl.AppRequest(message, null, null, to, filters, excludeIds, maxRecipients, data, title, callback);
		}

		public static void ShareLink(global::System.Uri contentURL = null, string contentTitle = "", string contentDescription = "", global::System.Uri photoURL = null, global::Facebook.Unity.FacebookDelegate<global::Facebook.Unity.IShareResult> callback = null)
		{
			FacebookImpl.ShareLink(contentURL, contentTitle, contentDescription, photoURL, callback);
		}

		public static void FeedShare(string toId = "", global::System.Uri link = null, string linkName = "", string linkCaption = "", string linkDescription = "", global::System.Uri picture = null, string mediaSource = "", global::Facebook.Unity.FacebookDelegate<global::Facebook.Unity.IShareResult> callback = null)
		{
			FacebookImpl.FeedShare(toId, link, linkName, linkCaption, linkDescription, picture, mediaSource, callback);
		}

		public static void API(string query, global::Facebook.Unity.HttpMethod method, global::Facebook.Unity.FacebookDelegate<global::Facebook.Unity.IGraphResult> callback = null, global::System.Collections.Generic.IDictionary<string, string> formData = null)
		{
			if (string.IsNullOrEmpty(query))
			{
				throw new global::System.ArgumentNullException("query", "The query param cannot be null or empty");
			}
			FacebookImpl.API(query, method, formData, callback);
		}

		public static void API(string query, global::Facebook.Unity.HttpMethod method, global::Facebook.Unity.FacebookDelegate<global::Facebook.Unity.IGraphResult> callback, global::UnityEngine.WWWForm formData)
		{
			if (string.IsNullOrEmpty(query))
			{
				throw new global::System.ArgumentNullException("query", "The query param cannot be null or empty");
			}
			FacebookImpl.API(query, method, formData, callback);
		}

		public static void ActivateApp()
		{
			FacebookImpl.ActivateApp(AppId);
		}

		public static void GetAppLink(global::Facebook.Unity.FacebookDelegate<global::Facebook.Unity.IAppLinkResult> callback)
		{
			if (callback != null)
			{
				FacebookImpl.GetAppLink(callback);
			}
		}

		public static void GameGroupCreate(string name, string description, string privacy = "CLOSED", global::Facebook.Unity.FacebookDelegate<global::Facebook.Unity.IGroupCreateResult> callback = null)
		{
			FacebookImpl.GameGroupCreate(name, description, privacy, callback);
		}

		public static void GameGroupJoin(string id, global::Facebook.Unity.FacebookDelegate<global::Facebook.Unity.IGroupJoinResult> callback = null)
		{
			FacebookImpl.GameGroupJoin(id, callback);
		}

		public static void LogAppEvent(string logEvent, float? valueToSum = null, global::System.Collections.Generic.Dictionary<string, object> parameters = null)
		{
			FacebookImpl.AppEventsLogEvent(logEvent, valueToSum, parameters);
		}

		public static void LogPurchase(float logPurchase, string currency = null, global::System.Collections.Generic.Dictionary<string, object> parameters = null)
		{
			if (string.IsNullOrEmpty(currency))
			{
				currency = "USD";
			}
			FacebookImpl.AppEventsLogPurchase(logPurchase, currency, parameters);
		}

		private static void LogVersion()
		{
			if (facebook != null)
			{
				global::Facebook.Unity.FacebookLogger.Info(string.Format("Using Facebook Unity SDK v{0} with {1}", global::Facebook.Unity.FacebookSdkVersion.Build, FacebookImpl.SDKUserAgent));
			}
			else
			{
				global::Facebook.Unity.FacebookLogger.Info(string.Format("Using Facebook Unity SDK v{0}", global::Facebook.Unity.FacebookSdkVersion.Build));
			}
		}
	}
}
