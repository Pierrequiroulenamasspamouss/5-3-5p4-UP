namespace Discord.Unity.Mobile.IOS
{
	internal class IOSFacebookLoader : global::Discord.Unity.FB.CompiledFacebookLoader
	{
		protected override global::Discord.Unity.FacebookGameObject FBGameObject
		{
			get
			{
				global::Discord.Unity.Mobile.IOS.IOSFacebookGameObject component = global::Discord.Unity.ComponentFactory.GetComponent<global::Discord.Unity.Mobile.IOS.IOSFacebookGameObject>();
				if (component.Discord == null)
				{
					component.Discord = new global::Discord.Unity.Mobile.IOS.IOSFacebook();
				}
				return component;
			}
		}
	}
}
