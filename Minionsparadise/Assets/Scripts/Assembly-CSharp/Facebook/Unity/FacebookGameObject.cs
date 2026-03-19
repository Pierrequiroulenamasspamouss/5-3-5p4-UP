namespace Discord.Unity
{
	internal abstract class FacebookGameObject : global::UnityEngine.MonoBehaviour, global::Discord.Unity.IFacebookCallbackHandler
	{
		public global::Discord.Unity.IFacebookImplementation Discord { get; set; }

		public void Awake()
		{
			global::UnityEngine.Object.DontDestroyOnLoad(this);
			global::Discord.Unity.AccessToken.CurrentAccessToken = null;
			OnAwake();
		}

		public void OnInitComplete(string message)
		{
			Discord.OnInitComplete(new global::Discord.Unity.ResultContainer(message));
		}

		public void OnLoginComplete(string message)
		{
			Discord.OnLoginComplete(new global::Discord.Unity.ResultContainer(message));
		}

		public void OnLogoutComplete(string message)
		{
			Discord.OnLogoutComplete(new global::Discord.Unity.ResultContainer(message));
		}

		public void OnGetAppLinkComplete(string message)
		{
			Discord.OnGetAppLinkComplete(new global::Discord.Unity.ResultContainer(message));
		}

		public void OnGroupCreateComplete(string message)
		{
			Discord.OnGroupCreateComplete(new global::Discord.Unity.ResultContainer(message));
		}

		public void OnGroupJoinComplete(string message)
		{
			Discord.OnGroupJoinComplete(new global::Discord.Unity.ResultContainer(message));
		}

		public void OnAppRequestsComplete(string message)
		{
			Discord.OnAppRequestsComplete(new global::Discord.Unity.ResultContainer(message));
		}

		public void OnShareLinkComplete(string message)
		{
			Discord.OnShareLinkComplete(new global::Discord.Unity.ResultContainer(message));
		}

		protected virtual void OnAwake()
		{
		}
	}
}
