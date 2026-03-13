namespace Facebook.Unity
{
	internal abstract class FacebookGameObject : global::UnityEngine.MonoBehaviour, global::Facebook.Unity.IFacebookCallbackHandler
	{
		public global::Facebook.Unity.IFacebookImplementation Facebook { get; set; }

		public void Awake()
		{
			global::UnityEngine.Object.DontDestroyOnLoad(this);
			global::Facebook.Unity.AccessToken.CurrentAccessToken = null;
			OnAwake();
		}

		public void OnInitComplete(string message)
		{
			Facebook.OnInitComplete(new global::Facebook.Unity.ResultContainer(message));
		}

		public void OnLoginComplete(string message)
		{
			Facebook.OnLoginComplete(new global::Facebook.Unity.ResultContainer(message));
		}

		public void OnLogoutComplete(string message)
		{
			Facebook.OnLogoutComplete(new global::Facebook.Unity.ResultContainer(message));
		}

		public void OnGetAppLinkComplete(string message)
		{
			Facebook.OnGetAppLinkComplete(new global::Facebook.Unity.ResultContainer(message));
		}

		public void OnGroupCreateComplete(string message)
		{
			Facebook.OnGroupCreateComplete(new global::Facebook.Unity.ResultContainer(message));
		}

		public void OnGroupJoinComplete(string message)
		{
			Facebook.OnGroupJoinComplete(new global::Facebook.Unity.ResultContainer(message));
		}

		public void OnAppRequestsComplete(string message)
		{
			Facebook.OnAppRequestsComplete(new global::Facebook.Unity.ResultContainer(message));
		}

		public void OnShareLinkComplete(string message)
		{
			Facebook.OnShareLinkComplete(new global::Facebook.Unity.ResultContainer(message));
		}

		protected virtual void OnAwake()
		{
		}
	}
}
