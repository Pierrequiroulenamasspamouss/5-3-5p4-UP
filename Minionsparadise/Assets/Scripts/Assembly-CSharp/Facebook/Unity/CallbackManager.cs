namespace Discord.Unity
{
	internal class CallbackManager
	{
		private global::System.Collections.Generic.IDictionary<string, object> facebookDelegates = new global::System.Collections.Generic.Dictionary<string, object>();

		private int nextAsyncId;

		public string AddFacebookDelegate<T>(global::Discord.Unity.FacebookDelegate<T> callback) where T : global::Discord.Unity.IResult
		{
			if (callback == null)
			{
				return null;
			}
			nextAsyncId++;
			facebookDelegates.Add(nextAsyncId.ToString(), callback);
			return nextAsyncId.ToString();
		}

		public void OnFacebookResponse(global::Discord.Unity.IInternalResult result)
		{
			object value;
			if (result != null && result.CallbackId != null && facebookDelegates.TryGetValue(result.CallbackId, out value))
			{
				CallCallback(value, result);
				facebookDelegates.Remove(result.CallbackId);
			}
		}

		private static void CallCallback(object callback, global::Discord.Unity.IResult result)
		{
			if (callback == null || result == null || TryCallCallback<global::Discord.Unity.IAppRequestResult>(callback, result) || TryCallCallback<global::Discord.Unity.IShareResult>(callback, result) || TryCallCallback<global::Discord.Unity.IGroupCreateResult>(callback, result) || TryCallCallback<global::Discord.Unity.IGroupJoinResult>(callback, result) || TryCallCallback<global::Discord.Unity.IPayResult>(callback, result) || TryCallCallback<global::Discord.Unity.IAppInviteResult>(callback, result) || TryCallCallback<global::Discord.Unity.IAppLinkResult>(callback, result) || TryCallCallback<global::Discord.Unity.ILoginResult>(callback, result) || TryCallCallback<global::Discord.Unity.IAccessTokenRefreshResult>(callback, result))
			{
				return;
			}
			throw new global::System.NotSupportedException("Unexpected result type: " + callback.GetType().FullName);
		}

		private static bool TryCallCallback<T>(object callback, global::Discord.Unity.IResult result) where T : global::Discord.Unity.IResult
		{
			global::Discord.Unity.FacebookDelegate<T> facebookDelegate = callback as global::Discord.Unity.FacebookDelegate<T>;
			if (facebookDelegate != null)
			{
				facebookDelegate((T)result);
				return true;
			}
			return false;
		}
	}
}
