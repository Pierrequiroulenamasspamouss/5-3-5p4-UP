namespace Facebook.Unity
{
	internal class CallbackManager
	{
		private global::System.Collections.Generic.IDictionary<string, object> facebookDelegates = new global::System.Collections.Generic.Dictionary<string, object>();

		private int nextAsyncId;

		public string AddFacebookDelegate<T>(global::Facebook.Unity.FacebookDelegate<T> callback) where T : global::Facebook.Unity.IResult
		{
			if (callback == null)
			{
				return null;
			}
			nextAsyncId++;
			facebookDelegates.Add(nextAsyncId.ToString(), callback);
			return nextAsyncId.ToString();
		}

		public void OnFacebookResponse(global::Facebook.Unity.IInternalResult result)
		{
			object value;
			if (result != null && result.CallbackId != null && facebookDelegates.TryGetValue(result.CallbackId, out value))
			{
				CallCallback(value, result);
				facebookDelegates.Remove(result.CallbackId);
			}
		}

		private static void CallCallback(object callback, global::Facebook.Unity.IResult result)
		{
			if (callback == null || result == null || TryCallCallback<global::Facebook.Unity.IAppRequestResult>(callback, result) || TryCallCallback<global::Facebook.Unity.IShareResult>(callback, result) || TryCallCallback<global::Facebook.Unity.IGroupCreateResult>(callback, result) || TryCallCallback<global::Facebook.Unity.IGroupJoinResult>(callback, result) || TryCallCallback<global::Facebook.Unity.IPayResult>(callback, result) || TryCallCallback<global::Facebook.Unity.IAppInviteResult>(callback, result) || TryCallCallback<global::Facebook.Unity.IAppLinkResult>(callback, result) || TryCallCallback<global::Facebook.Unity.ILoginResult>(callback, result) || TryCallCallback<global::Facebook.Unity.IAccessTokenRefreshResult>(callback, result))
			{
				return;
			}
			throw new global::System.NotSupportedException("Unexpected result type: " + callback.GetType().FullName);
		}

		private static bool TryCallCallback<T>(object callback, global::Facebook.Unity.IResult result) where T : global::Facebook.Unity.IResult
		{
			global::Facebook.Unity.FacebookDelegate<T> facebookDelegate = callback as global::Facebook.Unity.FacebookDelegate<T>;
			if (facebookDelegate != null)
			{
				facebookDelegate((T)result);
				return true;
			}
			return false;
		}
	}
}
