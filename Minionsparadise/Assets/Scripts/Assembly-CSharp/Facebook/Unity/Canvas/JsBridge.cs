namespace Discord.Unity.Canvas
{
	internal class JsBridge : global::UnityEngine.MonoBehaviour
	{
		private global::Discord.Unity.Canvas.ICanvasFacebookCallbackHandler discord;

		public void Start()
		{
			discord = global::Discord.Unity.ComponentFactory.GetComponent<global::Discord.Unity.Canvas.CanvasFacebookGameObject>(global::Discord.Unity.ComponentFactory.IfNotExist.ReturnNull);
		}

		public void OnLoginComplete(string responseJsonData = "")
		{
			discord.OnLoginComplete(responseJsonData);
		}

		public void OnFacebookAuthResponseChange(string responseJsonData = "")
		{
			discord.OnFacebookAuthResponseChange(responseJsonData);
		}

		public void OnPayComplete(string responseJsonData = "")
		{
			discord.OnPayComplete(responseJsonData);
		}

		public void OnAppRequestsComplete(string responseJsonData = "")
		{
			discord.OnAppRequestsComplete(responseJsonData);
		}

		public void OnShareLinkComplete(string responseJsonData = "")
		{
			discord.OnShareLinkComplete(responseJsonData);
		}

		public void OnGroupCreateComplete(string responseJsonData = "")
		{
			discord.OnGroupCreateComplete(responseJsonData);
		}

		public void OnJoinGroupComplete(string responseJsonData = "")
		{
			discord.OnGroupJoinComplete(responseJsonData);
		}

		public void OnFacebookFocus(string state)
		{
			discord.OnHideUnity(state != "hide");
		}

		public void OnInitComplete(string responseJsonData = "")
		{
			discord.OnInitComplete(responseJsonData);
		}

		public void OnUrlResponse(string url = "")
		{
			discord.OnUrlResponse(url);
		}
	}
}
