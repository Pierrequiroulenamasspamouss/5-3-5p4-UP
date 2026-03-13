namespace Facebook.Unity.Canvas
{
	internal class JsBridge : global::UnityEngine.MonoBehaviour
	{
		private global::Facebook.Unity.Canvas.ICanvasFacebookCallbackHandler facebook;

		public void Start()
		{
			facebook = global::Facebook.Unity.ComponentFactory.GetComponent<global::Facebook.Unity.Canvas.CanvasFacebookGameObject>(global::Facebook.Unity.ComponentFactory.IfNotExist.ReturnNull);
		}

		public void OnLoginComplete(string responseJsonData = "")
		{
			facebook.OnLoginComplete(responseJsonData);
		}

		public void OnFacebookAuthResponseChange(string responseJsonData = "")
		{
			facebook.OnFacebookAuthResponseChange(responseJsonData);
		}

		public void OnPayComplete(string responseJsonData = "")
		{
			facebook.OnPayComplete(responseJsonData);
		}

		public void OnAppRequestsComplete(string responseJsonData = "")
		{
			facebook.OnAppRequestsComplete(responseJsonData);
		}

		public void OnShareLinkComplete(string responseJsonData = "")
		{
			facebook.OnShareLinkComplete(responseJsonData);
		}

		public void OnGroupCreateComplete(string responseJsonData = "")
		{
			facebook.OnGroupCreateComplete(responseJsonData);
		}

		public void OnJoinGroupComplete(string responseJsonData = "")
		{
			facebook.OnGroupJoinComplete(responseJsonData);
		}

		public void OnFacebookFocus(string state)
		{
			facebook.OnHideUnity(state != "hide");
		}

		public void OnInitComplete(string responseJsonData = "")
		{
			facebook.OnInitComplete(responseJsonData);
		}

		public void OnUrlResponse(string url = "")
		{
			facebook.OnUrlResponse(url);
		}
	}
}
