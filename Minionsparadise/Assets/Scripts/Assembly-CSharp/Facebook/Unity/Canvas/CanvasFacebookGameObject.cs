namespace Facebook.Unity.Canvas
{
	internal class CanvasFacebookGameObject : global::Facebook.Unity.FacebookGameObject, global::Facebook.Unity.Canvas.ICanvasFacebookCallbackHandler, global::Facebook.Unity.IFacebookCallbackHandler
	{
		protected global::Facebook.Unity.Canvas.ICanvasFacebookImplementation CanvasFacebookImpl
		{
			get
			{
				return (global::Facebook.Unity.Canvas.ICanvasFacebookImplementation)base.Facebook;
			}
		}

		public void OnPayComplete(string result)
		{
			CanvasFacebookImpl.OnPayComplete(new global::Facebook.Unity.ResultContainer(result));
		}

		public void OnFacebookAuthResponseChange(string message)
		{
			CanvasFacebookImpl.OnFacebookAuthResponseChange(new global::Facebook.Unity.ResultContainer(message));
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
			gameObject.AddComponent<global::Facebook.Unity.Canvas.JsBridge>();
			gameObject.transform.parent = base.gameObject.transform;
		}
	}
}
