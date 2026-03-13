namespace Facebook.Unity
{
	internal abstract class FacebookBase : global::Facebook.Unity.IFacebook, global::Facebook.Unity.IFacebookImplementation, global::Facebook.Unity.IFacebookResultHandler
	{
		private global::Facebook.Unity.InitDelegate onInitCompleteDelegate;

		private global::Facebook.Unity.HideUnityDelegate onHideUnityDelegate;

		public abstract bool LimitEventUsage { get; set; }

		public abstract string SDKName { get; }

		public abstract string SDKVersion { get; }

		public virtual string SDKUserAgent
		{
			get
			{
				return global::Facebook.Unity.Utilities.GetUserAgent(SDKName, SDKVersion);
			}
		}

		public bool LoggedIn
		{
			get
			{
				global::Facebook.Unity.AccessToken currentAccessToken = global::Facebook.Unity.AccessToken.CurrentAccessToken;
				return currentAccessToken != null && currentAccessToken.ExpirationTime > global::System.DateTime.UtcNow;
			}
		}

		public bool Initialized { get; private set; }

		protected global::Facebook.Unity.CallbackManager CallbackManager { get; private set; }

		protected FacebookBase(global::Facebook.Unity.CallbackManager callbackManager)
		{
			CallbackManager = callbackManager;
		}

		public virtual void Init(global::Facebook.Unity.HideUnityDelegate hideUnityDelegate, global::Facebook.Unity.InitDelegate onInitComplete)
		{
			onHideUnityDelegate = hideUnityDelegate;
			onInitCompleteDelegate = onInitComplete;
		}

		public abstract void LogInWithPublishPermissions(global::System.Collections.Generic.IEnumerable<string> scope, global::Facebook.Unity.FacebookDelegate<global::Facebook.Unity.ILoginResult> callback);

		public abstract void LogInWithReadPermissions(global::System.Collections.Generic.IEnumerable<string> scope, global::Facebook.Unity.FacebookDelegate<global::Facebook.Unity.ILoginResult> callback);

		public virtual void LogOut()
		{
			global::Facebook.Unity.AccessToken.CurrentAccessToken = null;
		}

		public void AppRequest(string message, global::System.Collections.Generic.IEnumerable<string> to = null, global::System.Collections.Generic.IEnumerable<object> filters = null, global::System.Collections.Generic.IEnumerable<string> excludeIds = null, int? maxRecipients = null, string data = "", string title = "", global::Facebook.Unity.FacebookDelegate<global::Facebook.Unity.IAppRequestResult> callback = null)
		{
			AppRequest(message, null, null, to, filters, excludeIds, maxRecipients, data, title, callback);
		}

		public abstract void AppRequest(string message, global::Facebook.Unity.OGActionType? actionType, string objectId, global::System.Collections.Generic.IEnumerable<string> to, global::System.Collections.Generic.IEnumerable<object> filters, global::System.Collections.Generic.IEnumerable<string> excludeIds, int? maxRecipients, string data, string title, global::Facebook.Unity.FacebookDelegate<global::Facebook.Unity.IAppRequestResult> callback);

		public abstract void ShareLink(global::System.Uri contentURL, string contentTitle, string contentDescription, global::System.Uri photoURL, global::Facebook.Unity.FacebookDelegate<global::Facebook.Unity.IShareResult> callback);

		public abstract void FeedShare(string toId, global::System.Uri link, string linkName, string linkCaption, string linkDescription, global::System.Uri picture, string mediaSource, global::Facebook.Unity.FacebookDelegate<global::Facebook.Unity.IShareResult> callback);

		public void API(string query, global::Facebook.Unity.HttpMethod method, global::System.Collections.Generic.IDictionary<string, string> formData, global::Facebook.Unity.FacebookDelegate<global::Facebook.Unity.IGraphResult> callback)
		{
			object dictionary2;
			if (formData != null)
			{
				global::System.Collections.Generic.IDictionary<string, string> dictionary = CopyByValue(formData);
				dictionary2 = dictionary;
			}
			else
			{
				dictionary2 = new global::System.Collections.Generic.Dictionary<string, string>();
			}
			global::System.Collections.Generic.IDictionary<string, string> dictionary3 = (global::System.Collections.Generic.IDictionary<string, string>)dictionary2;
			if (!dictionary3.ContainsKey("access_token") && !query.Contains("access_token="))
			{
				dictionary3["access_token"] = ((!global::Facebook.Unity.FB.IsLoggedIn) ? string.Empty : global::Facebook.Unity.AccessToken.CurrentAccessToken.TokenString);
			}
			global::Facebook.Unity.AsyncRequestString.Request(GetGraphUrl(query), method, dictionary3, callback);
		}

		public void API(string query, global::Facebook.Unity.HttpMethod method, global::UnityEngine.WWWForm formData, global::Facebook.Unity.FacebookDelegate<global::Facebook.Unity.IGraphResult> callback)
		{
			if (formData == null)
			{
				formData = new global::UnityEngine.WWWForm();
			}
			string value = ((global::Facebook.Unity.AccessToken.CurrentAccessToken == null) ? string.Empty : global::Facebook.Unity.AccessToken.CurrentAccessToken.TokenString);
			formData.AddField("access_token", value);
			global::Facebook.Unity.AsyncRequestString.Request(GetGraphUrl(query), method, formData, callback);
		}

		public abstract void GameGroupCreate(string name, string description, string privacy, global::Facebook.Unity.FacebookDelegate<global::Facebook.Unity.IGroupCreateResult> callback);

		public abstract void GameGroupJoin(string id, global::Facebook.Unity.FacebookDelegate<global::Facebook.Unity.IGroupJoinResult> callback);

		public abstract void ActivateApp(string appId = null);

		public abstract void GetAppLink(global::Facebook.Unity.FacebookDelegate<global::Facebook.Unity.IAppLinkResult> callback);

		public abstract void AppEventsLogEvent(string logEvent, float? valueToSum, global::System.Collections.Generic.Dictionary<string, object> parameters);

		public abstract void AppEventsLogPurchase(float logPurchase, string currency, global::System.Collections.Generic.Dictionary<string, object> parameters);

		public virtual void OnHideUnity(bool isGameShown)
		{
			if (onHideUnityDelegate != null)
			{
				onHideUnityDelegate(isGameShown);
			}
		}

		public virtual void OnInitComplete(global::Facebook.Unity.ResultContainer resultContainer)
		{
			Initialized = true;
			global::Facebook.Unity.FacebookDelegate<global::Facebook.Unity.ILoginResult> callback = delegate
			{
				if (onInitCompleteDelegate != null)
				{
					onInitCompleteDelegate();
				}
			};
			resultContainer.ResultDictionary["callback_id"] = CallbackManager.AddFacebookDelegate(callback);
			OnLoginComplete(resultContainer);
		}

		public abstract void OnLoginComplete(global::Facebook.Unity.ResultContainer resultContainer);

		public void OnLogoutComplete(global::Facebook.Unity.ResultContainer resultContainer)
		{
			global::Facebook.Unity.AccessToken.CurrentAccessToken = null;
		}

		public abstract void OnGetAppLinkComplete(global::Facebook.Unity.ResultContainer resultContainer);

		public abstract void OnGroupCreateComplete(global::Facebook.Unity.ResultContainer resultContainer);

		public abstract void OnGroupJoinComplete(global::Facebook.Unity.ResultContainer resultContainer);

		public abstract void OnAppRequestsComplete(global::Facebook.Unity.ResultContainer resultContainer);

		public abstract void OnShareLinkComplete(global::Facebook.Unity.ResultContainer resultContainer);

		protected void ValidateAppRequestArgs(string message, global::Facebook.Unity.OGActionType? actionType, string objectId, global::System.Collections.Generic.IEnumerable<string> to = null, global::System.Collections.Generic.IEnumerable<object> filters = null, global::System.Collections.Generic.IEnumerable<string> excludeIds = null, int? maxRecipients = null, string data = "", string title = "", global::Facebook.Unity.FacebookDelegate<global::Facebook.Unity.IAppRequestResult> callback = null)
		{
			if (string.IsNullOrEmpty(message))
			{
				throw new global::System.ArgumentNullException("message", "message cannot be null or empty!");
			}
			if (!string.IsNullOrEmpty(objectId) && actionType != global::Facebook.Unity.OGActionType.ASKFOR && (actionType.GetValueOrDefault() != global::Facebook.Unity.OGActionType.SEND || !actionType.HasValue))
			{
				throw new global::System.ArgumentNullException("objectId", "Object ID must be set if and only if action type is SEND or ASKFOR");
			}
			if (!actionType.HasValue && !string.IsNullOrEmpty(objectId))
			{
				throw new global::System.ArgumentNullException("actionType", "You cannot provide an objectId without an actionType");
			}
		}

		protected void OnAuthResponse(global::Facebook.Unity.LoginResult result)
		{
			if (result.AccessToken != null)
			{
				global::Facebook.Unity.AccessToken.CurrentAccessToken = result.AccessToken;
			}
			CallbackManager.OnFacebookResponse(result);
		}

		private global::System.Collections.Generic.IDictionary<string, string> CopyByValue(global::System.Collections.Generic.IDictionary<string, string> data)
		{
			global::System.Collections.Generic.Dictionary<string, string> dictionary = new global::System.Collections.Generic.Dictionary<string, string>(data.Count);
			foreach (global::System.Collections.Generic.KeyValuePair<string, string> datum in data)
			{
				dictionary[datum.Key] = ((datum.Value == null) ? null : new string(datum.Value.ToCharArray()));
			}
			return dictionary;
		}

		private global::System.Uri GetGraphUrl(string query)
		{
			if (!string.IsNullOrEmpty(query) && query.StartsWith("/"))
			{
				query = query.Substring(1);
			}
			return new global::System.Uri(global::Facebook.Unity.Constants.GraphUrl, query);
		}
	}
}
