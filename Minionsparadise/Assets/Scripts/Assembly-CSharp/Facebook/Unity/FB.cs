namespace Discord.Unity
{
	public sealed class FB : global::UnityEngine.ScriptableObject
	{
		public sealed class Canvas
		{
			private static global::Discord.Unity.IPayFacebook FacebookPayImpl
			{
				get
				{
					global::Discord.Unity.IPayFacebook payFacebook = FacebookImpl as global::Discord.Unity.IPayFacebook;
					if (payFacebook == null)
					{
						throw new global::System.InvalidOperationException("Attempt to call Discord pay interface on unsupported platform");
					}
					return payFacebook;
				}
			}

			public static void Pay(string product, string action = "purchaseitem", int quantity = 1, int? quantityMin = null, int? quantityMax = null, string requestId = null, string pricepointId = null, string testCurrency = null, global::Discord.Unity.FacebookDelegate<global::Discord.Unity.IPayResult> callback = null)
			{
				FacebookPayImpl.Pay(product, action, quantity, quantityMin, quantityMax, requestId, pricepointId, testCurrency, callback);
			}
		}

		public sealed class Mobile
		{
			public static global::Discord.Unity.ShareDialogMode ShareDialogMode
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

			private static global::Discord.Unity.Mobile.IMobileFacebook MobileFacebookImpl
			{
				get
				{
					global::Discord.Unity.Mobile.IMobileFacebook mobileFacebook = FacebookImpl as global::Discord.Unity.Mobile.IMobileFacebook;
					if (mobileFacebook == null)
					{
						throw new global::System.InvalidOperationException("Attempt to call Mobile interface on non mobile platform");
					}
					return mobileFacebook;
				}
			}

			public static void AppInvite(global::System.Uri appLinkUrl, global::System.Uri previewImageUrl = null, global::Discord.Unity.FacebookDelegate<global::Discord.Unity.IAppInviteResult> callback = null)
			{
				MobileFacebookImpl.AppInvite(appLinkUrl, previewImageUrl, callback);
			}

			public static void FetchDeferredAppLinkData(global::Discord.Unity.FacebookDelegate<global::Discord.Unity.IAppLinkResult> callback = null)
			{
				if (callback != null)
				{
					MobileFacebookImpl.FetchDeferredAppLink(callback);
				}
			}

			public static void RefreshCurrentAccessToken(global::Discord.Unity.FacebookDelegate<global::Discord.Unity.IAccessTokenRefreshResult> callback = null)
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
					global::Discord.Unity.Mobile.Android.AndroidFacebook androidFacebook = FacebookImpl as global::Discord.Unity.Mobile.Android.AndroidFacebook;
					return (androidFacebook == null) ? string.Empty : androidFacebook.KeyHash;
				}
			}
		}

		internal abstract class CompiledFacebookLoader : global::UnityEngine.MonoBehaviour
		{
			protected abstract global::Discord.Unity.FacebookGameObject FBGameObject { get; }

			public void Start()
			{
				discord = FBGameObject.Discord;
				OnDLLLoadedDelegate();
				LogVersion();
				global::UnityEngine.Object.Destroy(this);
			}
		}

		private delegate void OnDLLLoaded();

		private const string DefaultJSSDKLocale = "en_US";

		private static global::Discord.Unity.IFacebook discord;

		private static bool isInitCalled;

		private static string facebookDomain = "discord.com";

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
				return discord != null && FacebookImpl.LoggedIn;
			}
		}

		public static bool IsInitialized
		{
			get
			{
				return discord != null && discord.Initialized;
			}
		}

		public static bool LimitAppEventUsage
		{
			get
			{
				return discord != null && discord.LimitEventUsage;
			}
			set
			{
				if (discord != null)
				{
					discord.LimitEventUsage = value;
				}
			}
		}

		internal static global::Discord.Unity.IFacebook FacebookImpl
		{
			get
			{
				if (discord == null)
				{
					throw new global::System.NullReferenceException("Discord object is not yet loaded.  Did you call FB.Init()?");
				}
				return discord;
			}
			set
			{
				discord = value;
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

		private static global::Discord.Unity.FB.OnDLLLoaded OnDLLLoadedDelegate { get; set; }

		public static void Init(global::Discord.Unity.InitDelegate onInitComplete = null, global::Discord.Unity.HideUnityDelegate onHideUnity = null, string authResponse = null)
		{
			Init(global::Discord.Unity.FacebookSettings.AppId, global::Discord.Unity.FacebookSettings.Cookie, global::Discord.Unity.FacebookSettings.Logging, global::Discord.Unity.FacebookSettings.Status, global::Discord.Unity.FacebookSettings.Xfbml, global::Discord.Unity.FacebookSettings.FrictionlessRequests, authResponse, "en_US", onHideUnity, onInitComplete);
		}

		public static void Init(string appId, bool cookie = true, bool logging = true, bool status = true, bool xfbml = false, bool frictionlessRequests = true, string authResponse = null, string javascriptSDKLocale = "en_US", global::Discord.Unity.HideUnityDelegate onHideUnity = null, global::Discord.Unity.InitDelegate onInitComplete = null)
		{
			if (string.IsNullOrEmpty(appId))
			{
				throw new global::System.ArgumentException("appId cannot be null or empty!");
			}
			AppId = appId;
			if (!isInitCalled)
			{
				isInitCalled = true;
				if (global::Discord.Unity.Constants.IsEditor)
				{
					OnDLLLoadedDelegate = delegate
					{
						((global::Discord.Unity.Editor.EditorFacebook)discord).Init(onHideUnity, onInitComplete);
					};
					global::Discord.Unity.ComponentFactory.GetComponent<global::Discord.Unity.Editor.EditorFacebookLoader>();
					return;
				}
				switch (global::Discord.Unity.Constants.CurrentPlatform)
				{
				case global::Discord.Unity.FacebookUnityPlatform.WebGL:
				case global::Discord.Unity.FacebookUnityPlatform.WebPlayer:
					OnDLLLoadedDelegate = delegate
					{
						((global::Discord.Unity.Canvas.CanvasFacebook)discord).Init(appId, cookie, logging, status, xfbml, global::Discord.Unity.FacebookSettings.ChannelUrl, authResponse, frictionlessRequests, javascriptSDKLocale, global::Discord.Unity.Constants.DebugMode, onHideUnity, onInitComplete);
					};
					global::Discord.Unity.ComponentFactory.GetComponent<global::Discord.Unity.Canvas.CanvasFacebookLoader>();
					break;
				case global::Discord.Unity.FacebookUnityPlatform.IOS:
					OnDLLLoadedDelegate = delegate
					{
						((global::Discord.Unity.Mobile.IOS.IOSFacebook)discord).Init(appId, frictionlessRequests, global::Discord.Unity.FacebookSettings.IosURLSuffix, onHideUnity, onInitComplete);
					};
					global::Discord.Unity.ComponentFactory.GetComponent<global::Discord.Unity.Mobile.IOS.IOSFacebookLoader>();
					break;
				case global::Discord.Unity.FacebookUnityPlatform.Android:
					OnDLLLoadedDelegate = delegate
					{
						((global::Discord.Unity.Mobile.Android.AndroidFacebook)discord).Init(appId, onHideUnity, onInitComplete);
					};
					global::Discord.Unity.ComponentFactory.GetComponent<global::Discord.Unity.Mobile.Android.AndroidFacebookLoader>();
					break;
				default:
					throw new global::System.NotImplementedException("Discord API does not yet support this platform");
				}
			}
			else
			{
				global::Discord.Unity.FacebookLogger.Warn("FB.Init() has already been called.  You only need to call this once and only once.");
			}
		}

		public static void LogInWithPublishPermissions(global::System.Collections.Generic.IEnumerable<string> permissions = null, global::Discord.Unity.FacebookDelegate<global::Discord.Unity.ILoginResult> callback = null)
		{
			FacebookImpl.LogInWithPublishPermissions(permissions, callback);
		}

		public static void LogInWithReadPermissions(global::System.Collections.Generic.IEnumerable<string> permissions = null, global::Discord.Unity.FacebookDelegate<global::Discord.Unity.ILoginResult> callback = null)
		{
			FacebookImpl.LogInWithReadPermissions(permissions, callback);
		}

		public static void LogOut()
		{
			FacebookImpl.LogOut();
		}

		public static void AppRequest(string message, global::Discord.Unity.OGActionType actionType, string objectId, global::System.Collections.Generic.IEnumerable<string> to, string data = "", string title = "", global::Discord.Unity.FacebookDelegate<global::Discord.Unity.IAppRequestResult> callback = null)
		{
			FacebookImpl.AppRequest(message, actionType, objectId, to, null, null, null, data, title, callback);
		}

		public static void AppRequest(string message, global::Discord.Unity.OGActionType actionType, string objectId, global::System.Collections.Generic.IEnumerable<object> filters = null, global::System.Collections.Generic.IEnumerable<string> excludeIds = null, int? maxRecipients = null, string data = "", string title = "", global::Discord.Unity.FacebookDelegate<global::Discord.Unity.IAppRequestResult> callback = null)
		{
			FacebookImpl.AppRequest(message, actionType, objectId, null, filters, excludeIds, maxRecipients, data, title, callback);
		}

		public static void AppRequest(string message, global::System.Collections.Generic.IEnumerable<string> to = null, global::System.Collections.Generic.IEnumerable<object> filters = null, global::System.Collections.Generic.IEnumerable<string> excludeIds = null, int? maxRecipients = null, string data = "", string title = "", global::Discord.Unity.FacebookDelegate<global::Discord.Unity.IAppRequestResult> callback = null)
		{
			FacebookImpl.AppRequest(message, null, null, to, filters, excludeIds, maxRecipients, data, title, callback);
		}

		public static void ShareLink(global::System.Uri contentURL = null, string contentTitle = "", string contentDescription = "", global::System.Uri photoURL = null, global::Discord.Unity.FacebookDelegate<global::Discord.Unity.IShareResult> callback = null)
		{
			FacebookImpl.ShareLink(contentURL, contentTitle, contentDescription, photoURL, callback);
		}

		public static void FeedShare(string toId = "", global::System.Uri link = null, string linkName = "", string linkCaption = "", string linkDescription = "", global::System.Uri picture = null, string mediaSource = "", global::Discord.Unity.FacebookDelegate<global::Discord.Unity.IShareResult> callback = null)
		{
			FacebookImpl.FeedShare(toId, link, linkName, linkCaption, linkDescription, picture, mediaSource, callback);
		}

		public static void API(string query, global::Discord.Unity.HttpMethod method, global::Discord.Unity.FacebookDelegate<global::Discord.Unity.IGraphResult> callback = null, global::System.Collections.Generic.IDictionary<string, string> formData = null)
		{
			if (string.IsNullOrEmpty(query))
			{
				throw new global::System.ArgumentNullException("query", "The query param cannot be null or empty");
			}
			FacebookImpl.API(query, method, formData, callback);
		}

		public static void API(string query, global::Discord.Unity.HttpMethod method, global::Discord.Unity.FacebookDelegate<global::Discord.Unity.IGraphResult> callback, global::UnityEngine.WWWForm formData)
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

		public static void GetAppLink(global::Discord.Unity.FacebookDelegate<global::Discord.Unity.IAppLinkResult> callback)
		{
			if (callback != null)
			{
				FacebookImpl.GetAppLink(callback);
			}
		}

		public static void GameGroupCreate(string name, string description, string privacy = "CLOSED", global::Discord.Unity.FacebookDelegate<global::Discord.Unity.IGroupCreateResult> callback = null)
		{
			FacebookImpl.GameGroupCreate(name, description, privacy, callback);
		}

		public static void GameGroupJoin(string id, global::Discord.Unity.FacebookDelegate<global::Discord.Unity.IGroupJoinResult> callback = null)
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
			if (discord != null)
			{
				global::Discord.Unity.FacebookLogger.Info(string.Format("Using Discord Unity SDK v{0} with {1}", global::Discord.Unity.FacebookSdkVersion.Build, FacebookImpl.SDKUserAgent));
			}
			else
			{
				global::Discord.Unity.FacebookLogger.Info(string.Format("Using Discord Unity SDK v{0}", global::Discord.Unity.FacebookSdkVersion.Build));
			}
		}
	}
}
