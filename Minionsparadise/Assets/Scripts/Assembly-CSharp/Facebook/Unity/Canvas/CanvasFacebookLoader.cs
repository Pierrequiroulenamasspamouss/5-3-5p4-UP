namespace Discord.Unity.Canvas
{
	internal class CanvasFacebookLoader : global::Discord.Unity.FB.CompiledFacebookLoader
	{
		protected override global::Discord.Unity.FacebookGameObject FBGameObject
		{
			get
			{
				global::Discord.Unity.Canvas.CanvasFacebookGameObject component = global::Discord.Unity.ComponentFactory.GetComponent<global::Discord.Unity.Canvas.CanvasFacebookGameObject>();
				if (component.Discord == null)
				{
					component.Discord = new global::Discord.Unity.Canvas.CanvasFacebook();
				}
				return component;
			}
		}
	}
}
