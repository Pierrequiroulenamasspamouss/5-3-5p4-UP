namespace Facebook.Unity.Canvas
{
	internal class CanvasFacebookLoader : global::Facebook.Unity.FB.CompiledFacebookLoader
	{
		protected override global::Facebook.Unity.FacebookGameObject FBGameObject
		{
			get
			{
				global::Facebook.Unity.Canvas.CanvasFacebookGameObject component = global::Facebook.Unity.ComponentFactory.GetComponent<global::Facebook.Unity.Canvas.CanvasFacebookGameObject>();
				if (component.Facebook == null)
				{
					component.Facebook = new global::Facebook.Unity.Canvas.CanvasFacebook();
				}
				return component;
			}
		}
	}
}
