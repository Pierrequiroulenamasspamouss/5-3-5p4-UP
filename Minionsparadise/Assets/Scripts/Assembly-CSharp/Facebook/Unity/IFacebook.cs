namespace Facebook.Unity
{
	internal interface IFacebook
	{
		bool LoggedIn { get; }

		bool LimitEventUsage { get; set; }

		string SDKName { get; }

		string SDKVersion { get; }

		string SDKUserAgent { get; }

		bool Initialized { get; }

		void LogInWithPublishPermissions(global::System.Collections.Generic.IEnumerable<string> permissions, global::Facebook.Unity.FacebookDelegate<global::Facebook.Unity.ILoginResult> callback);

		void LogInWithReadPermissions(global::System.Collections.Generic.IEnumerable<string> permissions, global::Facebook.Unity.FacebookDelegate<global::Facebook.Unity.ILoginResult> callback);

		void LogOut();

		[global::System.Obsolete]
		void AppRequest(string message, global::System.Collections.Generic.IEnumerable<string> to, global::System.Collections.Generic.IEnumerable<object> filters, global::System.Collections.Generic.IEnumerable<string> excludeIds, int? maxRecipients, string data, string title, global::Facebook.Unity.FacebookDelegate<global::Facebook.Unity.IAppRequestResult> callback);

		void AppRequest(string message, global::Facebook.Unity.OGActionType? actionType, string objectId, global::System.Collections.Generic.IEnumerable<string> to, global::System.Collections.Generic.IEnumerable<object> filters, global::System.Collections.Generic.IEnumerable<string> excludeIds, int? maxRecipients, string data, string title, global::Facebook.Unity.FacebookDelegate<global::Facebook.Unity.IAppRequestResult> callback);

		void ShareLink(global::System.Uri contentURL, string contentTitle, string contentDescription, global::System.Uri photoURL, global::Facebook.Unity.FacebookDelegate<global::Facebook.Unity.IShareResult> callback);

		void FeedShare(string toId, global::System.Uri link, string linkName, string linkCaption, string linkDescription, global::System.Uri picture, string mediaSource, global::Facebook.Unity.FacebookDelegate<global::Facebook.Unity.IShareResult> callback);

		void GameGroupCreate(string name, string description, string privacy, global::Facebook.Unity.FacebookDelegate<global::Facebook.Unity.IGroupCreateResult> callback);

		void GameGroupJoin(string id, global::Facebook.Unity.FacebookDelegate<global::Facebook.Unity.IGroupJoinResult> callback);

		void API(string query, global::Facebook.Unity.HttpMethod method, global::System.Collections.Generic.IDictionary<string, string> formData, global::Facebook.Unity.FacebookDelegate<global::Facebook.Unity.IGraphResult> callback);

		void API(string query, global::Facebook.Unity.HttpMethod method, global::UnityEngine.WWWForm formData, global::Facebook.Unity.FacebookDelegate<global::Facebook.Unity.IGraphResult> callback);

		void ActivateApp(string appId = null);

		void GetAppLink(global::Facebook.Unity.FacebookDelegate<global::Facebook.Unity.IAppLinkResult> callback);

		void AppEventsLogEvent(string logEvent, float? valueToSum, global::System.Collections.Generic.Dictionary<string, object> parameters);

		void AppEventsLogPurchase(float logPurchase, string currency, global::System.Collections.Generic.Dictionary<string, object> parameters);
	}
}
