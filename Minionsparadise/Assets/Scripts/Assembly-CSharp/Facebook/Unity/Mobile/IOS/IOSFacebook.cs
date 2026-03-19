namespace Discord.Unity.Mobile.IOS
{
	internal class IOSFacebook : global::Discord.Unity.Mobile.MobileFacebook
	{
		public enum FBInsightsFlushBehavior
		{
			FBInsightsFlushBehaviorAuto = 0,
			FBInsightsFlushBehaviorExplicitOnly = 1
		}

		private class NativeDict
		{
			public int NumEntries { get; set; }

			public string[] Keys { get; set; }

			public string[] Values { get; set; }

			public NativeDict()
			{
				NumEntries = 0;
				Keys = null;
				Values = null;
			}
		}

		private const string CancelledResponse = "{\"cancelled\":true}";

		private bool limitEventUsage;

		private global::Discord.Unity.Mobile.IOS.IIOSWrapper iosWrapper;

		public override bool LimitEventUsage
		{
			get
			{
				return limitEventUsage;
			}
			set
			{
				limitEventUsage = value;
				iosWrapper.FBAppEventsSetLimitEventUsage(value);
			}
		}

		public override string SDKName
		{
			get
			{
				return "FBiOSSDK";
			}
		}

		public override string SDKVersion
		{
			get
			{
				return iosWrapper.FBSdkVersion();
			}
		}

		public IOSFacebook()
			: this(new global::Discord.Unity.Mobile.IOS.IOSWrapper(), new global::Discord.Unity.CallbackManager())
		{
		}

		public IOSFacebook(global::Discord.Unity.Mobile.IOS.IIOSWrapper iosWrapper, global::Discord.Unity.CallbackManager callbackManager)
			: base(callbackManager)
		{
			this.iosWrapper = iosWrapper;
		}

		public void Init(string appId, bool frictionlessRequests, string iosURLSuffix, global::Discord.Unity.HideUnityDelegate hideUnityDelegate, global::Discord.Unity.InitDelegate onInitComplete)
		{
			base.Init(hideUnityDelegate, onInitComplete);
			iosWrapper.Init(appId, frictionlessRequests, iosURLSuffix, global::Discord.Unity.Constants.UnitySDKUserAgentSuffixLegacy);
		}

		public override void LogInWithReadPermissions(global::System.Collections.Generic.IEnumerable<string> permissions, global::Discord.Unity.FacebookDelegate<global::Discord.Unity.ILoginResult> callback)
		{
			iosWrapper.LogInWithReadPermissions(AddCallback(callback), permissions.ToCommaSeparateList());
		}

		public override void LogInWithPublishPermissions(global::System.Collections.Generic.IEnumerable<string> permissions, global::Discord.Unity.FacebookDelegate<global::Discord.Unity.ILoginResult> callback)
		{
			iosWrapper.LogInWithPublishPermissions(AddCallback(callback), permissions.ToCommaSeparateList());
		}

		public override void LogOut()
		{
			base.LogOut();
			iosWrapper.LogOut();
		}

		public override void AppRequest(string message, global::Discord.Unity.OGActionType? actionType, string objectId, global::System.Collections.Generic.IEnumerable<string> to, global::System.Collections.Generic.IEnumerable<object> filters, global::System.Collections.Generic.IEnumerable<string> excludeIds, int? maxRecipients, string data, string title, global::Discord.Unity.FacebookDelegate<global::Discord.Unity.IAppRequestResult> callback)
		{
			ValidateAppRequestArgs(message, actionType, objectId, to, filters, excludeIds, maxRecipients, data, title, callback);
			string text = null;
			if (filters != null && global::System.Linq.Enumerable.Any(filters))
			{
				text = global::System.Linq.Enumerable.First(filters) as string;
			}
			iosWrapper.AppRequest(AddCallback(callback), message, (!actionType.HasValue) ? string.Empty : actionType.ToString(), (objectId == null) ? string.Empty : objectId, (to == null) ? null : global::System.Linq.Enumerable.ToArray(to), (to != null) ? global::System.Linq.Enumerable.Count(to) : 0, (text == null) ? string.Empty : text, (excludeIds == null) ? null : global::System.Linq.Enumerable.ToArray(excludeIds), (excludeIds != null) ? global::System.Linq.Enumerable.Count(excludeIds) : 0, maxRecipients.HasValue, maxRecipients.HasValue ? maxRecipients.Value : 0, data, title);
		}

		public override void AppInvite(global::System.Uri appLinkUrl, global::System.Uri previewImageUrl, global::Discord.Unity.FacebookDelegate<global::Discord.Unity.IAppInviteResult> callback)
		{
			string appLinkUrl2 = string.Empty;
			string previewImageUrl2 = string.Empty;
			if (appLinkUrl != null && !string.IsNullOrEmpty(appLinkUrl.AbsoluteUri))
			{
				appLinkUrl2 = appLinkUrl.AbsoluteUri;
			}
			if (previewImageUrl != null && !string.IsNullOrEmpty(previewImageUrl.AbsoluteUri))
			{
				previewImageUrl2 = previewImageUrl.AbsoluteUri;
			}
			iosWrapper.AppInvite(AddCallback(callback), appLinkUrl2, previewImageUrl2);
		}

		public override void ShareLink(global::System.Uri contentURL, string contentTitle, string contentDescription, global::System.Uri photoURL, global::Discord.Unity.FacebookDelegate<global::Discord.Unity.IShareResult> callback)
		{
			iosWrapper.ShareLink(AddCallback(callback), contentURL.AbsoluteUrlOrEmptyString(), contentTitle, contentDescription, photoURL.AbsoluteUrlOrEmptyString());
		}

		public override void FeedShare(string toId, global::System.Uri link, string linkName, string linkCaption, string linkDescription, global::System.Uri picture, string mediaSource, global::Discord.Unity.FacebookDelegate<global::Discord.Unity.IShareResult> callback)
		{
			string link2 = ((!(link != null)) ? string.Empty : link.ToString());
			string picture2 = ((!(picture != null)) ? string.Empty : picture.ToString());
			iosWrapper.FeedShare(AddCallback(callback), toId, link2, linkName, linkCaption, linkDescription, picture2, mediaSource);
		}

		public override void GameGroupCreate(string name, string description, string privacy, global::Discord.Unity.FacebookDelegate<global::Discord.Unity.IGroupCreateResult> callback)
		{
			iosWrapper.CreateGameGroup(AddCallback(callback), name, description, privacy);
		}

		public override void GameGroupJoin(string id, global::Discord.Unity.FacebookDelegate<global::Discord.Unity.IGroupJoinResult> callback)
		{
			iosWrapper.JoinGameGroup(global::System.Convert.ToInt32(base.CallbackManager.AddFacebookDelegate(callback)), id);
		}

		public override void AppEventsLogEvent(string logEvent, float? valueToSum, global::System.Collections.Generic.Dictionary<string, object> parameters)
		{
			global::Discord.Unity.Mobile.IOS.IOSFacebook.NativeDict nativeDict = MarshallDict(parameters);
			if (valueToSum.HasValue)
			{
				iosWrapper.LogAppEvent(logEvent, valueToSum.Value, nativeDict.NumEntries, nativeDict.Keys, nativeDict.Values);
			}
			else
			{
				iosWrapper.LogAppEvent(logEvent, 0.0, nativeDict.NumEntries, nativeDict.Keys, nativeDict.Values);
			}
		}

		public override void AppEventsLogPurchase(float logPurchase, string currency, global::System.Collections.Generic.Dictionary<string, object> parameters)
		{
			global::Discord.Unity.Mobile.IOS.IOSFacebook.NativeDict nativeDict = MarshallDict(parameters);
			iosWrapper.LogPurchaseAppEvent(logPurchase, currency, nativeDict.NumEntries, nativeDict.Keys, nativeDict.Values);
		}

		public override void ActivateApp(string appId)
		{
		}

		public override void FetchDeferredAppLink(global::Discord.Unity.FacebookDelegate<global::Discord.Unity.IAppLinkResult> callback)
		{
			iosWrapper.FetchDeferredAppLink(AddCallback(callback));
		}

		public override void GetAppLink(global::Discord.Unity.FacebookDelegate<global::Discord.Unity.IAppLinkResult> callback)
		{
			iosWrapper.GetAppLink(global::System.Convert.ToInt32(base.CallbackManager.AddFacebookDelegate(callback)));
		}

		public override void RefreshCurrentAccessToken(global::Discord.Unity.FacebookDelegate<global::Discord.Unity.IAccessTokenRefreshResult> callback)
		{
			iosWrapper.RefreshCurrentAccessToken(global::System.Convert.ToInt32(base.CallbackManager.AddFacebookDelegate(callback)));
		}

		protected override void SetShareDialogMode(global::Discord.Unity.ShareDialogMode mode)
		{
			iosWrapper.SetShareDialogMode((int)mode);
		}

		private static global::Discord.Unity.Mobile.IOS.IOSFacebook.NativeDict MarshallDict(global::System.Collections.Generic.Dictionary<string, object> dict)
		{
			global::Discord.Unity.Mobile.IOS.IOSFacebook.NativeDict nativeDict = new global::Discord.Unity.Mobile.IOS.IOSFacebook.NativeDict();
			if (dict != null && dict.Count > 0)
			{
				nativeDict.Keys = new string[dict.Count];
				nativeDict.Values = new string[dict.Count];
				nativeDict.NumEntries = 0;
				foreach (global::System.Collections.Generic.KeyValuePair<string, object> item in dict)
				{
					nativeDict.Keys[nativeDict.NumEntries] = item.Key;
					nativeDict.Values[nativeDict.NumEntries] = item.Value.ToString();
					nativeDict.NumEntries++;
				}
			}
			return nativeDict;
		}

		private static global::Discord.Unity.Mobile.IOS.IOSFacebook.NativeDict MarshallDict(global::System.Collections.Generic.Dictionary<string, string> dict)
		{
			global::Discord.Unity.Mobile.IOS.IOSFacebook.NativeDict nativeDict = new global::Discord.Unity.Mobile.IOS.IOSFacebook.NativeDict();
			if (dict != null && dict.Count > 0)
			{
				nativeDict.Keys = new string[dict.Count];
				nativeDict.Values = new string[dict.Count];
				nativeDict.NumEntries = 0;
				foreach (global::System.Collections.Generic.KeyValuePair<string, string> item in dict)
				{
					nativeDict.Keys[nativeDict.NumEntries] = item.Key;
					nativeDict.Values[nativeDict.NumEntries] = item.Value;
					nativeDict.NumEntries++;
				}
			}
			return nativeDict;
		}

		private int AddCallback<T>(global::Discord.Unity.FacebookDelegate<T> callback) where T : global::Discord.Unity.IResult
		{
			string value = base.CallbackManager.AddFacebookDelegate(callback);
			return global::System.Convert.ToInt32(value);
		}
	}
}
