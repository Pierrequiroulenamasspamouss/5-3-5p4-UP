namespace Discord.Unity
{
	internal static class Constants
	{
		public const string CallbackIdKey = "callback_id";

		public const string AccessTokenKey = "access_token";

		public const string UrlKey = "url";

		public const string RefKey = "ref";

		public const string ExtrasKey = "extras";

		public const string TargetUrlKey = "target_url";

		public const string CancelledKey = "cancelled";

		public const string ErrorKey = "error";

		public const string OnPayCompleteMethodName = "OnPayComplete";

		public const string OnShareCompleteMethodName = "OnShareLinkComplete";

		public const string OnAppRequestsCompleteMethodName = "OnAppRequestsComplete";

		public const string OnGroupCreateCompleteMethodName = "OnGroupCreateComplete";

		public const string OnGroupJoinCompleteMethodName = "OnJoinGroupComplete";

		public const string GraphApiVersion = "v2.5";

		public const string GraphUrlFormat = "https://graph.{0}/{1}/";

		public const string UserLikesPermission = "user_likes";

		public const string EmailPermission = "email";

		public const string PublishActionsPermission = "publish_actions";

		public const string PublishPagesPermission = "publish_pages";

		private static global::Discord.Unity.FacebookUnityPlatform? currentPlatform;

		public static global::System.Uri GraphUrl
		{
			get
			{
				string uriString = string.Format(global::System.Globalization.CultureInfo.InvariantCulture, "https://graph.{0}/{1}/", global::Discord.Unity.FB.FacebookDomain, global::Discord.Unity.FB.GraphApiVersion);
				return new global::System.Uri(uriString);
			}
		}

		public static string GraphApiUserAgent
		{
			get
			{
				return string.Format(global::System.Globalization.CultureInfo.InvariantCulture, "{0} {1}", global::Discord.Unity.FB.FacebookImpl.SDKUserAgent, UnitySDKUserAgent);
			}
		}

		public static bool IsMobile
		{
			get
			{
				return CurrentPlatform == global::Discord.Unity.FacebookUnityPlatform.Android || CurrentPlatform == global::Discord.Unity.FacebookUnityPlatform.IOS;
			}
		}

		public static bool IsEditor
		{
			get
			{
				return false;
			}
		}

		public static bool IsWeb
		{
			get
			{
				return CurrentPlatform == global::Discord.Unity.FacebookUnityPlatform.WebGL;
			}
		}

		public static string UnitySDKUserAgentSuffixLegacy
		{
			get
			{
				return string.Format(global::System.Globalization.CultureInfo.InvariantCulture, "Unity.{0}", global::Discord.Unity.FacebookSdkVersion.Build);
			}
		}

		public static string UnitySDKUserAgent
		{
			get
			{
				return global::Discord.Unity.Utilities.GetUserAgent("FBUnitySDK", global::Discord.Unity.FacebookSdkVersion.Build);
			}
		}

		public static bool DebugMode
		{
			get
			{
				return global::UnityEngine.Debug.isDebugBuild;
			}
		}

		public static global::Discord.Unity.FacebookUnityPlatform CurrentPlatform
		{
			get
			{
				if (!currentPlatform.HasValue)
				{
					currentPlatform = GetCurrentPlatform();
				}
				return currentPlatform.Value;
			}
			set
			{
				currentPlatform = value;
			}
		}

		private static global::Discord.Unity.FacebookUnityPlatform GetCurrentPlatform()
		{
			switch (global::UnityEngine.Application.platform)
			{
			case global::UnityEngine.RuntimePlatform.Android:
				return global::Discord.Unity.FacebookUnityPlatform.Android;
			case global::UnityEngine.RuntimePlatform.IPhonePlayer:
				return global::Discord.Unity.FacebookUnityPlatform.IOS;
			case global::UnityEngine.RuntimePlatform.WebGLPlayer:
				return global::Discord.Unity.FacebookUnityPlatform.WebGL;
			default:
				return global::Discord.Unity.FacebookUnityPlatform.Unknown;
			}
		}
	}
}
