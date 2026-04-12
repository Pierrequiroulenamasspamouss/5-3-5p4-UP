namespace Discord.Unity.Canvas
{
	internal class CanvasFacebookGameObject : global::Discord.Unity.FacebookGameObject, global::Discord.Unity.Canvas.ICanvasFacebookCallbackHandler, global::Discord.Unity.IFacebookCallbackHandler
	{
		protected global::Discord.Unity.Canvas.ICanvasFacebookImplementation CanvasFacebookImpl
		{
			get
			{
				return (global::Discord.Unity.Canvas.ICanvasFacebookImplementation)base.Discord;
			}
		}

		public void OnPayComplete(string result)
		{
			CanvasFacebookImpl.OnPayComplete(new global::Discord.Unity.ResultContainer(result));
		}

		public void OnFacebookAuthResponseChange(string message)
		{
			CanvasFacebookImpl.OnFacebookAuthResponseChange(new global::Discord.Unity.ResultContainer(message));
		}

		public void OnUrlResponse(string message)
		{
			CanvasFacebookImpl.OnUrlResponse(message);
		}

		public void OnHideUnity(bool hide)
		{
			CanvasFacebookImpl.OnHideUnity(hide);
		}

		protected override void OnAwake()
		{
			global::UnityEngine.GameObject gameObject = new global::UnityEngine.GameObject("FacebookJsBridge");
			gameObject.AddComponent<global::Discord.Unity.Canvas.JsBridge>();
			gameObject.transform.parent = base.gameObject.transform;
		}
	}
}
